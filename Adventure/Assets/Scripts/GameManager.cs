using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Bridge bridge;
    public TMP_Text username;

    void Awake()
    {
        Debug.Log("<< start");
        bridge.UserDataChanged += (userData) =>
        {
            Debug.Log("<< Got data: " + userData);
            UserData data = JsonUtility.FromJson<UserData>(userData);
            username.text = data.username + "\n" + data.iconUrl + "\n" + data.access_token;
        };
    }

    void Update()
    {
    }
}
