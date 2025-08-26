import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ApiModule } from './api/api.module';
import { SocketGateway } from './socket/socket.gateway';

@Module({
  imports: [ApiModule],
  controllers: [AppController],
  providers: [AppService, SocketGateway],
})
export class AppModule {}
