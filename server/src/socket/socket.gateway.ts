import { Logger, Module } from '@nestjs/common';
import { OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit, SubscribeMessage, WebSocketGateway, WebSocketServer } from '@nestjs/websockets';
import { Server, Socket } from 'socket.io';
import { ApiModule } from 'src/api/api.module';
import { ApiService } from 'src/api/api.service';

@Module({
  imports: [ApiModule]
})

@WebSocketGateway({ cors: { origin: "*" } })
export class SocketGateway implements OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit {

    constructor(private readonly apiService: ApiService){}

  private readonly logger = new Logger(SocketGateway.name);

  @WebSocketServer() server: Server;

  users: [string, string][] = [];

  async handleConnection(client: Socket) {
    const { auth, session } = client.handshake.auth;

    const isValid = await this.apiService.AuthSock(session,auth);

    if (!isValid.valid) {
        this.logger.warn(`Invalid connection: ${client.id}`);
        client.disconnect();
        return;
    }

    this.users.push([client.id, isValid.userId!]);

    this.logger.log(`Client connected: ${client.id}`);
  }
  handleDisconnect(client: Socket) {
    this.logger.log(`Client disconnected: ${client.id}`);
  }
  afterInit() {
    this.logger.log(`WebSocket server initialized`);
  }

  @SubscribeMessage('message')
  handleMessage(client: Socket, payload: any): string {
    return 'Hello world!';
  }
}
