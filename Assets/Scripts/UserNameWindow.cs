using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNameWindow : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI userName;
    
    void Awake ()
    {
        if (UserData.userNameSetted)
        {
            // no need for this
            Destroy(this.gameObject);
        }
    }

    void Start ()
    {
        userName.text = "";
    }

    public void SetName ()
    {
        UserData.SetUserName(userName.text);
        Destroy(this.gameObject);
    }
}
