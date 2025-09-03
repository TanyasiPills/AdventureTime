mergeInto(LibraryManager.library, {
    ConnectSocket: function(urlPtr, pathPtr) {
        var url = UTF8ToString(urlPtr);
        var path = UTF8ToString(pathPtr);

        console.log("<< JS Socket connecting to:", url, "with path:", path);

        window.socket = io(url, { path: path, transports: ['websocket'] });

        console.log("<< JS Socket created:", window.socket.id);

        window.socket.on('connect', function() {
            console.log("<< JS Socket connected:", window.socket.id);
            SendMessage('GameManager', 'OnJSConnected', window.socket.id);
        });

        window.socket.on('message', function(data) {
            console.log("<< JS Socket message:", data);
            SendMessage('GameManager', 'OnJSMessage', JSON.stringify(data));
        });

        window.socket.on('disconnect', function() {
            console.log("<< JS Socket disconnected:", window.socket.id);
            SendMessage('GameManager', 'OnJSDisconnected', '');
        });

        window.socket.on('error', function(error) {
            console.error("<< JS Socket error:", error);
            SendMessage('GameManager', 'OnJSError', JSON.stringify(error));
        });

        window.socket.connect();
    },

    SendSocketMessage: function(eventPtr, messagePtr) {
        var eventName = UTF8ToString(eventPtr);
        var message = UTF8ToString(messagePtr);
        if(window.socket) window.socket.emit(eventName, message);
    }
});
