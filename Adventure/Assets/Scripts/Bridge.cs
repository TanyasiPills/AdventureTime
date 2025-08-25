using System.ComponentModel;
using UnityEngine;

public delegate void CustomEventHandler<T>(T e);
public class UserData
{
    public string username;
    public string iconUrl;
}
public class Bridge : MonoBehaviour
{
    private string userData;

    public string UserData
    {
        get { return userData; }
        set
        {
            userData = value;
            OnUserDataChanged(value);
        }
    }

    protected EventHandlerList EventDelegateCollection = new EventHandlerList();
    static readonly object NameChangedEventKey = new object();

    public event CustomEventHandler<string> UserDataChanged
    {
        add => EventDelegateCollection.AddHandler(NameChangedEventKey, value);
        remove => EventDelegateCollection.RemoveHandler(NameChangedEventKey, value);
    }

    public void OnUserDataChanged(string value)
    {
        CustomEventHandler<string> subscribeDelegates = (CustomEventHandler<string>)this.EventDelegateCollection[NameChangedEventKey];
        subscribeDelegates(value);
    }

    public void SetUserData(string value)
    {
        Debug.Log("<< " + value);
        UserData = value;
    }
}
