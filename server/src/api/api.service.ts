import { BadRequestException, Injectable } from '@nestjs/common';
import { PrismaService } from 'src/prisma.service';

@Injectable()
export class ApiService {
    constructor(private readonly db: PrismaService) { }

    async token(code: string) {
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

        this.updateUser(token);
        return { access_token: token.access_token };
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

        return { access_token: token.access_token };
    }
}
