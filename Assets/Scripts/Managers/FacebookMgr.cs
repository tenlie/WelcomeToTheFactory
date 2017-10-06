using UnityEngine;
using Facebook.Unity;
using System.Collections;

public class FacebookMgr
{
    //Singleton Pattern
    private static FacebookMgr _instance;
    public static FacebookMgr GetInstance
    {
        get
        {
            if (_instance == null) _instance = new FacebookMgr();
            return _instance;
        }
    }

    public bool IsLoggedIn
    {
        get { return FB.IsLoggedIn; }
    }

    public void Initialize()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }
    }

    public void SignIn()
    {
        FB.LogInWithReadPermissions();
    }

    public void SignOut()
    {
        FB.LogOut();
    }
}
