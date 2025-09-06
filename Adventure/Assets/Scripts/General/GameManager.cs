using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class UserData
{
    public string username;
    public string iconUrl;
    public string access_token;
}

[System.Serializable]
public class User{
    public string username;
    public Vector3 position;
    public GameObject prefab;
    public bool left;
    public bool needFlip;

    public override string ToString()
    {
        return $"Username: {username}, position: {position}";
    }
}

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private UserData userData;

    SocksManager socksManager;

    public Dictionary<string, User> users;

    void Awake()
    {
        users = new Dictionary<string, User>();

        socksManager = gameObject.GetComponent<SocksManager>();
        UserDataChanged += (userData) =>
        {
        };


        #if UNITY_EDITOR
            UserData user = new UserData();
            user.username = "Zsa";
            user.iconUrl = "https://cdn.discordapp.com/avatars/467031166718050304/b5845bdc47c3383a763453ba343d430e.png?size=256";
            user.access_token = "MTQwOTUwMjc5MDQwNTA2MjgzOA.4pgMU1TqoR1NRBYjkEJ84KllAIXEBn";
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


    private readonly Queue<Action> executionQueue = new Queue<Action>();

    public void Enqueue(Action action)
    {
        if (action == null) return;
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }

        foreach (KeyValuePair<string, User> user in users)
        {
            user.Value.prefab.transform.position = user.Value.position;
            if (user.Value.needFlip) {
                foreach (Transform child in user.Value.prefab.transform){
                    if (child.name == "body"){
                        child.GetComponent<SpriteRenderer>().flipX ^= true;
                        user.Value.needFlip = false;
                    }
                }

            }
        }
    }


    public void AddUser(string idIn, string usernameIn)
    {
        Debug.Log("newUser");
        User newUser = new User();
        newUser.username = usernameIn;
        newUser.position = Vector3.zero;
        newUser.left = false;
        newUser.needFlip = false;

        Debug.Log(playerPrefab);

        newUser.prefab = Instantiate(playerPrefab, newUser.position, Quaternion.identity);

        for (int i = 0; i < newUser.prefab.transform.childCount; i++)
        {
            Transform child = newUser.prefab.transform.GetChild(i);
            if(child.name == "name")child.GetComponent<TMP_Text>().text = newUser.username;
        }

        users.Add(idIn, newUser);
    }

    public void AddOldUser(string idIn, string usernameIn, Vector2 positionIn)
    {
        Debug.Log("newUser");
        User newUser = new User();
        newUser.username = usernameIn;
        newUser.position = new Vector3(positionIn.x, positionIn.y, 0);
        newUser.left = false;
        newUser.needFlip = false;

        Debug.Log(playerPrefab);

        newUser.prefab = Instantiate(playerPrefab, newUser.position, Quaternion.identity);

        for (int i = 0; i < newUser.prefab.transform.childCount; i++)
        {
            Transform child = newUser.prefab.transform.GetChild(i);
            if (child.name == "name") child.GetComponent<TMP_Text>().text = newUser.username;
        }

        users.Add(idIn, newUser);
    }

    public void UpdatePos(string id, Vector2 pos)
    {
        Vector3 curPos = users[id].position;
        users[id].position = new Vector3(curPos.x + pos.x, curPos.y + pos.y, curPos.z);
        if (pos.x < 0 && !users[id].left)
        {
            users[id].needFlip = true;
            users[id].left = true;
        }
        else if (pos.x > 0 && users[id].left)
        {
            users[id].needFlip = true;
            users[id].left = false;
        }
    }
}
