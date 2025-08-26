import { BadRequestException, Injectable } from '@nestjs/common';
import { PrismaService } from 'src/prisma.service';
import crypto from "crypto"

@Injectable()
export class ApiService {
    constructor(private readonly db: PrismaService) { }

    async Token(code: string) {
        const params = new URLSearchParams();
        params.append("client_id", process.env.VITE_DISCORD_CLIENT_ID!);
        params.append("client_secret", process.env.DISCORD_CLIENT_SECRET!);
        params.append("grant_type", "authorization_code");
        params.append("code", code);

        const response = await fetch(`https://discord.com/api/oauth2/token`, {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded",
            },
            body: params.toString()
        });

        if (!response.ok) {
            throw new BadRequestException("Failed to retrieve access token");
        }

        const token = await response.json();

        const randomToken = crypto.randomBytes(32);
        const shaToken = crypto.createHash('sha256').update(randomToken).digest('hex');

        const {userId} = await this.updateUser(token);

        await this.db.session.create({data:{
            token: shaToken,
            userId: userId
        }});

        return { access_token: token.access_token, session_token:  shaToken};
    }

    async AuthSock(authToken: string){
        const access = await this.db.user.findFirst({where: {access_token: authToken},});
        let isValid = access ? true : false;
        return { valid: isValid, userId: access?.id };
    }

    async Validate(sessionToken: string) {
        const session = await this.db.session.findUnique({
            where: { token: sessionToken },
        });

        let isInvalid = !session;
        let auth: any = null;

        if (session) {
            const user = await this.db.user.findUnique({
                where: { id: session.userId },
                select: { access_token: true },
            });

            if (user?.access_token) {
                console.log("<< validate check start");
                const userResponse = await fetch("https://discord.com/api/users/@me", {
                headers: { Authorization: `Bearer ${user.access_token}` },
                });
                console.log("<< validate check end");

                if (userResponse.ok) {
                    auth = user.access_token;
                } else {
                    isInvalid = true;
                }
            } else {
                isInvalid = true;
            }
        }

        return { need: isInvalid, auth };
    }

    async updateUser(token) {
        const userResponse = await fetch('https://discord.com/api/users/@me', {
            headers: { Authorization: `Bearer ${token.access_token}` }
        });

        const user = await userResponse.json();

        await this.db.user.upsert({
            where: { id: user.id }, update:
            {
                name: user.global_name, avatar: user.avatar, access_token: token.access_token
            },
            create:
            {
                id: user.id, name: user.global_name, avatar: user.avatar, access_token: token.access_token
            }
        });

        console.log("Token:", token);
        console.log("User Info:", user);

        return { userId: user.id };
    }
}
