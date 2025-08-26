using System;
using TMPro;
using UnityEngine;

public class UserData
{
    public string username;
    public string iconUrl;
    public string access_token;
}

public class GameManager : MonoBehaviour
{
    public TMP_Text username;
    private UserData userData;

    SocksManager socksManager;

    void Awake()
    {
        socksManager = gameObject.GetComponent<SocksManager>();
        UserDataChanged += (userData) =>
        {
            username.text = userData.username + "\n" + userData.iconUrl + "\n" + userData.access_token;
        };
    }

    public UserData UserData
    {
        get { return userData; }
        set
        {
            userData = value;
            UserDataChanged.Invoke(value);
        }
    }
    public event Action<UserData> UserDataChanged = delegate { };


    public void SetUserData(string value)
    {
        UserData = JsonUtility.FromJson<UserData>(value);
        socksManager.SetupSocketComs(userData.access_token);
    }
}
