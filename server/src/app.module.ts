import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ApiModule } from './api/api.module';
import { SocketGateway } from './socket/socket.gateway';
import { SocketModule } from './socket/socket.module';

@Module({
  imports: [ApiModule, SocketModule],
  controllers: [AppController],
  providers: [AppService, SocketGateway],
})
export class AppModule {}
