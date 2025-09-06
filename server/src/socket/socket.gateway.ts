import { Logger, Module } from '@nestjs/common';
import { OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit, SubscribeMessage, WebSocketGateway, WebSocketServer } from '@nestjs/websockets';
import { Server, Socket } from 'socket.io';
import { ApiService } from 'src/api/api.service';

type vec2 = {
  x: number;
  y: number;
};

type User = {
  id: string;
  username: string;
  position: vec2;
}

@WebSocketGateway({ cors: { origin: "*" }, allowEIO3: true })
export class SocketGateway implements OnGatewayConnection, OnGatewayDisconnect, OnGatewayInit {

    constructor(private readonly apiService: ApiService){}

  private readonly logger = new Logger(SocketGateway.name);

  @WebSocketServer() server: Server;

  users: Record<string, User> = {};

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

    let uname = await this.apiService.Username(isValid.userId!);

    this.users[client.id] = { id: isValid.userId!, username: uname!, position: { x:0, y:0 }};

    this.logger.log(`Client connected: ${client.id}`);

    client.broadcast.emit("userJoined", {id: client.id, username: uname});

    const simplifiedList = Object.entries(this.users) .filter(([clientId, _]) => clientId !== client.id).map(([clientId, user]) => ({
      client: clientId,
      username: user.username,
      position: user.position
    }));

    client.emit("init", {users: simplifiedList});
  }
  handleDisconnect(client: Socket) {
    delete this.users[client.id];
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
    let pos = JSON.parse(payload);
    let curpos = this.users[client.id].position;
    this.users[client.id].position = { x: curpos.x+pos.x, y: curpos.y+pos.y }; 
    client.broadcast.emit("position", {client: client.id, position: pos});
  }
}
