import { Logger, Module } from '@nestjs/common';
import { OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit, SubscribeMessage, WebSocketGateway, WebSocketServer } from '@nestjs/websockets';
import { Server, Socket } from 'socket.io';
import { ApiService } from 'src/api/api.service';

@WebSocketGateway({ cors: { origin: "*" } })
export class SocketGateway implements OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit {

    constructor(private readonly apiService: ApiService){}

  private readonly logger = new Logger(SocketGateway.name);

  @WebSocketServer() server: Server;

  users: [string, string][] = [];

  async handleConnection(client: Socket) {
    const auth: string = client.handshake.query.token as string;

    const isValid = await this.apiService.AuthSock(auth);

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
