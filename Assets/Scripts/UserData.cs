using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserData
{
    public static string userName { get; private set; }
    public static bool userNameSetted { get; private set; }
    public static int userMoney { get; private set; }
    public static List<int> OwningCards { get; private set; }   // won't actually use for now

    static UserData ()
    {
        // initialization
        userName = "player";
        userNameSetted = false;
        userMoney = 0;
        OwningCards = new List<int>{1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8,9 ,9, 10, 10};
    }

    public static void SetUserName (string name)
    {
        userName = name;
        userNameSetted = true;
        userMoney += 100; // give user 100 at first (for now)
    }

    public static void UserUseMoney (int money)
    {  
        userMoney -= money;
    }

    public static void UserGetCard (int cardID)
    {  
        OwningCards.Add(cardID);
    }
}
