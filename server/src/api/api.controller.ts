import { Body, Controller, Post } from '@nestjs/common';
import { ApiService } from './api.service';

@Controller('api')
export class ApiController {
    constructor(private readonly service: ApiService) {}

    @Post("token") async token(@Body() body: { code: string }) {
        return this.service.token(body.code);
    }
}
