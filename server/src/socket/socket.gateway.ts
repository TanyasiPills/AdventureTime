import { Logger, Module } from '@nestjs/common';
import { OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit, SubscribeMessage, WebSocketGateway, WebSocketServer } from '@nestjs/websockets';
import { Server, Socket } from 'socket.io';
import { ApiService } from 'src/api/api.service';

@WebSocketGateway({ cors: { origin: "*" }, allowEIO3: true })
export class SocketGateway implements OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit {

    constructor(private readonly apiService: ApiService){}

  private readonly logger = new Logger(SocketGateway.name);

  @WebSocketServer() server: Server;

  users: [string, string][] = [];

  async handleConnection(client: Socket) {
    const auth: string = client.handshake.query.token as string;

    if (auth == undefined){
      this.logger.warn(`Invalid connection: ${client.id}`);
      client.disconnect();
      return;
    }

    this.logger.log(`Token: ${auth}`);

    const isValid = await this.apiService.AuthSock(auth);

    if (!isValid.valid) {
        this.logger.warn(`Invalid connection: ${client.id}`);
        client.disconnect();
        return;
    }

    this.users.push([client.id, isValid.userId!]);

    this.logger.log(`Client connected: ${client.id}`);

    let uname = await this.apiService.Username(isValid.userId!);

    client.broadcast.emit("userJoined", {id: client.id, username: uname});
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

  @SubscribeMessage('position')
  handlePositionUpdate(client: Socket, payload: any) {
    //this.logger.log(payload);
    let pos = JSON.parse(payload);
    client.broadcast.emit("position", {client: client.id, position: pos});
  }
}
