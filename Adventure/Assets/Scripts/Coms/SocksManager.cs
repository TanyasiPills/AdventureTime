using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
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
    public void SetupSocketComs(string accessToken)
    {
        var options = new IO.Options();
        options.QueryString = $"token={accessToken}";

        Socket socket = IO.Socket("http://localhost:3001", options);

        socket.On(Socket.EVENT_CONNECT, () =>
        {
            Debug.Log("<< Connected to the server");
        });
    }
}
