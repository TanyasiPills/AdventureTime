# Messages

handleConnection(client: Socket)
handleDisconnect(client: Socket)
afterInit()
 

  *chat message*
@SubscribeMessage('Message')
handleMessage(client: Socket, payload: string): string <-(this is the return)


  *positionUpdate*
@SubscribeMessage('PlayerMove')
handleMessage(client: Socket, delta: any)

  *interaction*
@SubscribeMessage('PlayerInteract')
handleMessage(client: Socket, object: any, type: string)



