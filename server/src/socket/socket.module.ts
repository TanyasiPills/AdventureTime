import { Module } from '@nestjs/common';
import { ApiModule } from 'src/api/api.module';

@Module({
    imports: [ApiModule]
})
export class SocketModule {}
