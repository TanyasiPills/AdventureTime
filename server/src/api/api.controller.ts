import { Body, Controller, Post } from '@nestjs/common';
import { ApiService } from './api.service';

@Controller('api')
export class ApiController {
    constructor(private readonly service: ApiService) {}

    @Post("token") async Token(@Body() body: { code: string }) {
        return this.service.Token(body.code);
    }

    @Post("token/validate") async Validate(@Body() body: { token: string }) {
        return this.service.Validate(body.token);
    }
}
