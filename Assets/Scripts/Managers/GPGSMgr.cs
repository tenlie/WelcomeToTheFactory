using UnityEngine;
using GooglePlayGames;
using System.Collections.Generic;

public class GPGSMgr
{
    //싱글톤 패턴
    private static GPGSMgr _instance;
    public static GPGSMgr Instance
    {
        get
        {
            if (_instance == null) _instance = new GPGSMgr();
            return _instance;
        }
    }

    private bool _authenticating = false;
    public bool Authenticated { get { return Social.Active.localUser.authenticated;  } }

    //list of achievements we know we have unlocked (to avoid making repeated calls to the API)
    private Dictionary<string, bool> _unlockedAchievements = new Dictionary<string, bool>();

    //achievement increments we are accumulating locally, waiting to send to the games API
    private Dictionary<string, int> _pendingIncrements = new Dictionary<string, int>();

    //GooglePlayGames 초기화
    public void Initialize()
    {
        // PlayGamesPlatform 로그 활성화/비활성화
        PlayGamesPlatform.DebugLogEnabled = false;
        // Social.Active 초기화
        PlayGamesPlatform.Activate();
    }

    //GooglePlayGames 로그인
    public void SignInToGooglePlay()
    {
        if (Authenticated || _authenticating)
        {
            Debug.LogWarning("Ignoring repeated call to Authenticate().");
            return;
        }

        _authenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            _authenticating = false;
            if (success)
            {
                Debug.Log("Sign in successful!");
            }
            else
            {
                Debug.LogWarning("Failed to sign in with Google Play");
            }
        });
    }

    //GooglePlayGames 로그아웃
    public void SignOutFromGooglePlay()
    {
        GooglePlayGames.PlayGamesPlatform.Instance.SignOut();
    }

    //업적 조회하기
    public void ShowGooglePlayAchievements()
    {
        if (Authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    //리더보드 조회하기
    public void ShowLeaderboardUI()
    {
        if (Authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }
}
