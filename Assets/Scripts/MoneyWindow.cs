using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyWindow : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI money;

    void Start ()
    {
        money.text = UserData.userMoney.ToString();
    }

    public void UpdateMoneyWindow() {
        money.text = UserData.userMoney.ToString();
    }
}
