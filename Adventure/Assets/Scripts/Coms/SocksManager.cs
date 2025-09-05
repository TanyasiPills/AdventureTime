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
    private static extern void ConnectSocket(string url = "", string path = "/socket.io");

    [DllImport("__Internal")]
    private static extern void SendSocketMessage(string eventName, string message);
    public void SetupSocketComs(string accessToken)
    {
        Debug.Log("<< Connecting to socket...");
        ConnectSocket(path: "/server/socket.io");
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
