using System;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;

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


        #if UNITY_EDITOR
            UserData user = new UserData();
            user.username = "RBence";
            user.iconUrl = "https://cdn.discordapp.com/avatars/467031166718050304/b5845bdc47c3383a763453ba343d430e.png?size=256";
            user.access_token = "MTQwOTU4OTQxNTcyMTk1OTQ5Ng.9MvdWRvjcG9YR7Kht5Bbw7OFqbsVvO";
            SetUserData(JsonUtility.ToJson(user));
        #endif
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
