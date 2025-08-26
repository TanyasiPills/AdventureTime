import { Injectable } from '@nestjs/common';

@Injectable()
export class ApiService {
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

        // Retrieve the access_token from the response
        const { access_token } = await response.json();

        console.log("Access Token:", access_token);

        return { access_token };
    }
}
