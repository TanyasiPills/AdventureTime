using TMPro;
using UnityEngine;

public class RandomButton : MonoBehaviour
{
    public TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Gomb()
    {
        text.enabled = true;
        text.text = "Your mom is gay!";
    }
}
