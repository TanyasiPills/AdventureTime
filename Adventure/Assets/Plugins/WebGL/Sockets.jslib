mergeInto(LibraryManager.library, {
    ConnectSocket: function(urlPtr, pathPtr, tokenPtr) {
        var url = UTF8ToString(urlPtr);
        var path = UTF8ToString(pathPtr);
        var token = UTF8ToString(tokenPtr);

        console.log("<< JS Socket connecting to:", url, "with path:", path);
        window.socket = io(url, { path: path, transports: ['websocket'], query: {token: token} });


        window.socket.on('connect', function(data) {
            SendMessage('GameManager', 'OnJSConnected', JSON.stringify(data));
        });

        window.socket.on('message', function(data) {
            SendMessage('GameManager', 'OnJSMessage', JSON.stringify(data));
        });

        window.socket.on('disconnect', function() {
            SendMessage('GameManager', 'OnJSDisconnected', '');
        });

        window.socket.on('error', function(error) {
            SendMessage('GameManager', 'OnJSError', JSON.stringify(error));
        });

        window.socket.on('userJoined', function(data){
            SendMessage('GameManager', 'OnUserJoin', JSON.stringify(data));
        });

        window.socket.on('position', function(data){
            SendMessage('GameManager', 'OnPositionUpdate', JSON.stringify(data));
        });

        window.socket.connect();
    },

    SendSocketMessage: function(eventPtr, messagePtr) {
        var eventName = UTF8ToString(eventPtr);
        var message = UTF8ToString(messagePtr);
        if(window.socket) window.socket.emit(eventName, message);
    },

    DisconnectSocket: function() {
        window.socket.disconnect();
    }
});
