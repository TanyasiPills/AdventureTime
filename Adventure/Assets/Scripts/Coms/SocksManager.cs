using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using System;

public class SocksManager : MonoBehaviour
{
    GameManager manager;

    [System.Serializable]
    struct UserJoinData
    {
        public string id;
        public string username;
    }

    [System.Serializable]
    struct PosUpdateData
    {
        public string client;
        public Vector2 position;
    }

    [System.Serializable]
    struct AlreadyUsers
    {
        public List<AlreadyUserData> users;
    }

    [System.Serializable]
    struct AlreadyUserData
    {
        public string client;
        public string username;
        public Vector2 position;
    }

#if !UNITY_WEBGL || UNITY_EDITOR
    private Socket socket;
#endif
    private void Start()
    {
        manager = gameObject.GetComponent<GameManager>();
    }
    [DllImport("__Internal")]
    private static extern void ConnectSocket(string url = "", string path = "/socket.io", string token = "");

    [DllImport("__Internal")]
    private static extern void DisconnectSocket();

    [DllImport("__Internal")]
    private static extern void SendSocketMessage(string eventName, string message);

    public void SetupSocketComs(string accessToken)
    {
        Debug.Log("<< Connecting to socket...");
#if UNITY_WEBGL && !UNITY_EDITOR
            ConnectSocket(path: "/server/socket.io", token: accessToken);
#else
        var options = new IO.Options();
        options.Transports = ImmutableList.Create(Quobject.EngineIoClientDotNet.Client.Transports.WebSocket.NAME);
        options.Query = new Dictionary<string, string> { { "token", accessToken } };

        socket = IO.Socket("ws://localhost:3001", options);

        socket.On(Socket.EVENT_CONNECT, () =>
        {
            OnJSConnected();
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

        socket.On("userJoined", (data) =>
        {
            OnUserJoin(data.ToString());
        });

        socket.On("position", (data) =>
        {
            OnPositionUpdate(data.ToString());
        });

        socket.On("init", (data) =>
        {
            OnInit(data.ToSafeString());
        });

#endif
    }

    public void SendMessage(string eventName, string message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            SendSocketMessage(eventName, message);
#else
            //Debug.Log("sending message: " + message+", on evenet: "+eventName);
            socket.Emit(eventName, message);
        #endif
    }

    public void OnJSConnected()
    {
        Debug.Log("<< WS Connected to socket");
    }

    public void OnInit(string data)
    {
        if (data != "(null)")
        {
            Debug.Log(data);
            AlreadyUsers aldUsers = JsonUtility.FromJson<AlreadyUsers>(data);
            foreach (AlreadyUserData user in aldUsers.users)
            {
                Debug.Log(user);
                manager.Enqueue(() =>
                {
                    manager.AddOldUser(user.client, user.username, user.position);
                });
            }
        }
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

    public void OnUserJoin(string data)
    {
        UserJoinData user = JsonUtility.FromJson<UserJoinData>(data);
        manager.Enqueue(() =>
        {
            manager.AddUser(user.id, user.username);
        });
    }

    public void OnPositionUpdate(string data)
    {
        PosUpdateData user = JsonUtility.FromJson<PosUpdateData>(data);
        manager.Enqueue(() =>
        {
            manager.UpdatePos(user.client,user.position);
        });
    }

    private void OnDisable()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        DisconnectSocket();
#else
        socket.Disconnect();
#endif
    }
}
