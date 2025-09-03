using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Runtime.InteropServices;
public class SocksManager : MonoBehaviour
{
    GameObject gameManager;
    /*
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("manager");
        bridge = gameManager.GetComponent<Bridge>();
    }
    */
    [DllImport("__Internal")]
    private static extern void ConnectSocket(string url, string path);

    [DllImport("__Internal")]
    private static extern void SendSocketMessage(string eventName, string message);
    public void SetupSocketComs(string accessToken)
    {
        Debug.Log("<< Connecting to socket...");
        ConnectSocket("wss://1409589415721959496.discordsays.com", "/server/socket.io");
        /*
        var options = new IO.Options();
        options.Transports = ImmutableList.Create(Quobject.EngineIoClientDotNet.Client.Transports.WebSocket.NAME);
        options.Query = new Dictionary<string, string> { { "token", accessToken } };
        options.Path = "/server/socket.io/";

        Socket socket = IO.Socket("wss://1409589415721959496.discordsays.com", options);

        socket.On(Socket.EVENT_CONNECT, () =>
        {
            Debug.Log("<< Connected to the server");
        });

        socket.On(Socket.EVENT_CONNECT_ERROR, (err) =>
        {
            Debug.LogError("Connect error: " + err);
        });

        socket.On(Socket.EVENT_DISCONNECT, () =>
        {
            Debug.LogWarning("Disconnected");
            socket.Close();
        });*/
    }

    [ContextMenu("Testing")]
    public void Test()
    {
        Debug.Log("testing socket");
        string accessToken = "bob";
        var options = new IO.Options();
        options.Transports = ImmutableList.Create(Quobject.EngineIoClientDotNet.Client.Transports.WebSocket.NAME);
        options.Query = new Dictionary<string, string> { { "token", accessToken } };
        options.Path = "/server/socket.io/";

        Socket socket = IO.Socket("https://1409589415721959496.discordsays.com", options);

        socket.On(Socket.EVENT_CONNECT, () =>
        {
            Debug.Log("<< Connected to the server");
        });

        socket.On(Socket.EVENT_CONNECT_ERROR, (err) =>
        {
            Debug.LogError("Connect error: " + err);
        });

        socket.On(Socket.EVENT_DISCONNECT, () =>
        {
            Debug.LogWarning("Disconnected");
            socket.Close();
        });
    }
}
