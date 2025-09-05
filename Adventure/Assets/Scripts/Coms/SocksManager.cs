using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Runtime.InteropServices;
public class SocksManager : MonoBehaviour
{
    GameObject gameManager;
    #if !UNITY_WEBGL || UNITY_EDITOR
        private Socket socket;
    #endif
    /*
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("manager");
        bridge = gameManager.GetComponent<Bridge>();
    }
    */
    [DllImport("__Internal")]
    private static extern void ConnectSocket(string url = "", string path = "/socket.io");

    [DllImport("__Internal")]
    private static extern void SendSocketMessage(string eventName, string message);
    public void SetupSocketComs(string accessToken)
    {
        Debug.Log("<< Connecting to socket...");
        #if UNITY_WEBGL && !UNITY_EDITOR
            ConnectSocket(path: "/server/socket.io");
        #else
            var options = new IO.Options();
            options.Transports = ImmutableList.Create(Quobject.EngineIoClientDotNet.Client.Transports.WebSocket.NAME);
            options.Query = new Dictionary<string, string> { { "token", accessToken } };

            socket = IO.Socket("ws://localhost:3001", options);

            socket.On(Socket.EVENT_CONNECT, (id) =>
            {
                OnJSConnected(id.ToString());
            });

            socket.On(Socket.EVENT_CONNECT_ERROR, (err) =>
            {
                OnJSError(JsonUtility.ToJson(err));
            });

            socket.On(Socket.EVENT_DISCONNECT, (id) =>
            {
                OnJSDisconnected(id.ToString());
                socket.Close();
            });

            socket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                OnJSMessage(data.ToString());
            });
        #endif
    }

    public void SendMessage(string eventName, string message)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
                            SendSocketMessage(eventName, message);
        #else
            socket.Send(eventName, message);
        #endif
    }

    public void OnJSConnected(string socketId)
    {
        Debug.Log("<< WS Connected Socket id: " + socketId);
    }

    public void OnJSDisconnected(string socketId)
    {
        Debug.Log("<< WS Disconnected");
    }

    public void OnJSError(string errorJSON)
    {
        Debug.Log("<< WS Error: " + errorJSON);
    }

    public void OnJSMessage(string data)
    {
        Debug.Log("<< WS Message: " + data);
    }
}
