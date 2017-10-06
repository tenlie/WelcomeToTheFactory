using UnityEngine;
using System.Collections;
using Facebook.Unity;
//using GooglePlayGames;

public class MainInput : MonoBehaviour
{
    public UICamera UICamera;
    public MainGM mainGM { get; private set; }
    public GameObject stageSelect;
    public UICenterOnChild centerOnChild;
    public Transform DragCtrl;
    public Transform Pnl_StageSelect;

    public float scrollPerSecond { get; set; }
    public float minScrollPosition;
    public float maxScrollPosition;
    public float targetStage { get; set; }
    public bool isScrollingLeft { get; set; }
    public bool isScrollingRight { get; set; }
    public bool isScrolling { get; set; }
    public string officalWebsiteURL;
    public string composerURL;
    public string illustratorURL;

    void Start()
    {
        isScrollingLeft = false;
        isScrollingRight = false;
        isScrolling = false;

        mainGM = GetComponent<MainGM>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainGM.pn_Popup_Quit.activeInHierarchy)
            {
                CancelQuitGame();
            }
            else if (mainGM.pn_Popup_Credits.activeInHierarchy)
            {
                CloseCreditsPopup();
            }
            else if (mainGM.pn_Popup_TermsOfUse.activeInHierarchy)
            {
                CloseTermsOfUsePopup();
            }
            else if (mainGM.pn_Popup_Settings.activeInHierarchy)
            {
                CloseSettingsPopup();
            }
            else
            {
                if (!mainGM.pn_Popup_Quit.activeInHierarchy)
                {
                    NGUITools.SetActive(mainGM.pn_Popup_Quit, true);
                }
            }
        }

        float PosX = Pnl_StageSelect.localPosition.x;
        PosX -= PosX * 2;
        DragCtrl.localPosition = new Vector3(PosX, 0, 0);
    }

    public void ScrollLeft()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ScrollLeft()");
#endif
        StartCoroutine(ScrollLeftCo());
        /*
        if (isScrollingLeft) return;
        isScrollingLeft = true;
        mainGM.HideUI();
        SpringPanel.Begin(stageSelect, new Vector3(stageSelect.transform.localPosition.x + 1280f, 0, 0), 8.3f);
        Debug.Log("ScrollLeft: " + stageSelect.transform.localPosition.x);
        mainGM.ShowUI(0.05f);

        mainGM.previousStage = mainGM.gridStages[mainGM.currStageIdx];
        if (mainGM.currStageIdx==0)
        {
            mainGM.currStageIdx = 5;
        }
        else
        {
            mainGM.currStageIdx--;
        }

        Invoke("ButtonScrollCallback", 0.2f);
        */
    }

    IEnumerator ScrollLeftCo()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ScrollLeftCo()");
#endif
        if (isScrolling) yield break;
        isScrolling = true;
        mainGM.InstaHideAllUI();
        LobbySoundManager.Instance.StopBGM();
        LobbySoundManager.Instance.PlaySFX(LobbySFX.STAGE_SWIPE);
        //yield return new WaitForSeconds(0.2f);

        Debug.Log("before");
        SpringPanel.Begin(stageSelect, new Vector3(stageSelect.transform.localPosition.x + 1280f, 0, 0), 5f);
        Debug.Log("after");
        Debug.Log("ScrollLeft: " + stageSelect.transform.localPosition.x);
        mainGM.previousStage = mainGM.gridStages[mainGM.currStageIdx];
        if (mainGM.currStageIdx == 0)
        {
            mainGM.currStageIdx = 5;
        }
        else
        {
            mainGM.currStageIdx--;
        }
        yield return new WaitForSeconds(0.5f);

        centerOnChild.Recenter();
        yield return new WaitForSeconds(0.2f);

        mainGM.ShowAllUIElements(0f);
        yield return new WaitForSeconds(0.2f);
        isScrolling = false;
    }

    public void ScrollRight()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ScrollRight()");
#endif
        StartCoroutine(ScrollRightCo());
        /*
        if (isScrollingRight) return;
        Debug.Log("right");
        isScrollingRight = true;
        mainGM.HideUI();
        SpringPanel.Begin(stageSelect, new Vector3(stageSelect.transform.localPosition.x - 1280f, 0, 0), 8.3f);
        mainGM.ShowUI(0.05f);

        mainGM.previousStage = mainGM.gridStages[mainGM.currStageIdx];
        if (mainGM.currStageIdx == 5)
        {
            mainGM.currStageIdx = 0;
        }
        else
        {
            mainGM.currStageIdx++;
        }
        Invoke("ButtonScrollCallback", 0.2f);
        */
    }

    IEnumerator ScrollRightCo()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ScrollRightCo()");
#endif
        if (isScrolling) yield break;
        isScrolling = true;
        mainGM.InstaHideAllUI();
        LobbySoundManager.Instance.StopBGM();
        LobbySoundManager.Instance.PlaySFX(LobbySFX.STAGE_SWIPE);
        //yield return new WaitForSeconds(0.2f);
        Debug.Log("before");
        SpringPanel.Begin(stageSelect, new Vector3(stageSelect.transform.localPosition.x - 1280f, 0, 0), 5f);
        Debug.Log("after");
        Debug.Log("ScrollRight: " + stageSelect.transform.localPosition.x);
        mainGM.previousStage = mainGM.gridStages[mainGM.currStageIdx];
        if (mainGM.currStageIdx == 0)
        {
            mainGM.currStageIdx = 5;
        }
        else
        {
            mainGM.currStageIdx--;
        }
        yield return new WaitForSeconds(0.5f);

        centerOnChild.Recenter();
        yield return new WaitForSeconds(0.2f);

        mainGM.ShowAllUIElements(0f);
        isScrolling = false;
        yield return new WaitForSeconds(0.2f);
    }

    public void ButtonScrollCallback()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> PreventDoubleTouch()");
#endif
        //mainGM.stageSelect_Scrollview.SendMessage("Drag");
        mainGM.ChangeStageBackground(mainGM.currStageIdx);
        LobbySoundManager.Instance.StopBGM();
        LobbySoundManager.Instance.ChangeBGM(mainGM.currStageIdx);
        LobbySoundManager.Instance.PlayBGM();

        if (isScrollingLeft)
        {
            isScrollingLeft = false;
        }
        else
        {
            isScrollingRight = false;
        }
    }

    public void OpenSettingsPopUp()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> OpenSettingsPopUp()");
#endif
        StartCoroutine("OpenSettingsCo");
    }

    IEnumerator OpenSettingsCo()
    {
        LobbySoundManager.Instance.MuteBGM();
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        UICamera.enabled = false;
        mainGM.HideAllUI();
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(mainGM.mainButtons[2], true);
        mainGM.SetOptionPopup();
        NGUITools.SetActive(mainGM.pn_Popup_Settings, true);
        TweenAlpha.Begin(mainGM.pn_Popup_Settings, 0.5f, 1);
        yield return new WaitForSeconds(0.5f);
        UICamera.enabled = true;
        yield break;
    }

    public void CloseSettingsPopup()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> CloseSettingsPopUp()");
#endif
        StartCoroutine("CloseSettingsCo");
    }

    IEnumerator CloseSettingsCo()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        SaveData.SaveOption();
        //TweenAlpha.Begin(mainGM.mainButtons[2], 0f, 0f);
        TweenAlpha.Begin(mainGM.pn_Popup_Settings, 0.5f, 0);
        yield return new WaitForSeconds(0.5f);
        mainGM.settings_ScrollView.ResetPosition();
        NGUITools.SetActive(mainGM.pn_Popup_Settings, false);
        if (SaveData.Language != Localization.language)
        {
            mainGM.ReloadMainScreen();
        }
        mainGM.ShowAllUIElements(0.5f);
        for (int i = 0; i < mainGM.mainButtons.Length-1; i++)
        {
            TweenAlpha.Begin(mainGM.mainButtons[i], 0.5f, 1f);
        }
        yield return new WaitForSeconds(0.5f);

        if (SaveData.MusicOnOff.Equals("ON"))
        {
            LobbySoundManager.Instance.UnmuteBGM();
        }
        yield break;
    }

    public void OpenCreditsPopup()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> OpenCreditsPopup()");
#endif
        StartCoroutine("OpenCreditsCo");
    }

    IEnumerator OpenCreditsCo()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        mainGM.HideSettingsUI();
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(mainGM.btn_Credits, true);
        NGUITools.SetActive(mainGM.pn_Popup_Credits, true);
        TweenAlpha.Begin(mainGM.pn_Popup_Credits, 0.5f, 1);
        yield break;
    }

    public void CloseCreditsPopup()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> CloseCreditsPopup()");
#endif
        mainGM.credits_ScrollView.ResetPosition();
        StartCoroutine("CloseCreditsCo");
    }

    IEnumerator CloseCreditsCo()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        TweenAlpha.Begin(mainGM.pn_Popup_Credits, 0.5f, 0);
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(mainGM.pn_Popup_Credits, false);
        mainGM.ShowSettingsUI();
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(mainGM.btn_Credits, true);
        yield break;
    }

    public void OpenTermsOfUsePopup()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> OpenTermsOfUsePopup()");
#endif
        StartCoroutine("OpenTermsOfUseCo");
    }

    IEnumerator OpenTermsOfUseCo()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        mainGM.HideSettingsUI();
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(mainGM.btn_TermsOfUse, true);
        NGUITools.SetActive(mainGM.pn_Popup_TermsOfUse, true);
        TweenAlpha.Begin(mainGM.pn_Popup_TermsOfUse, 0.5f, 1);
        yield break;
    }

    public void CloseTermsOfUsePopup()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> CloseTermsOfUsePopup()");
#endif
        mainGM.termsOfUse_ScrollView.ResetPosition();
        StartCoroutine("CloseTermsOfUseCo");
    }

    IEnumerator CloseTermsOfUseCo()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        TweenAlpha.Begin(mainGM.pn_Popup_TermsOfUse, 0.5f, 0);
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(mainGM.pn_Popup_TermsOfUse, false);
        mainGM.ShowSettingsUI();
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(mainGM.btn_TermsOfUse, true);
        yield break;
    }

    public void OpenQuitPopup()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> OpenQuitPopup()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);
        NGUITools.SetActive(mainGM.pn_Popup_Settings, true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> QuitGame()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);
        Application.Quit();
    }

    public void CancelQuitGame()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> CancelQuitGame()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);
        NGUITools.SetActive(mainGM.pn_Popup_Quit, false);
    }

    public void ChangeMusicOption()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ChangeMusicOption()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        if (SaveData.MusicOnOff.Equals("ON"))
        {
            SaveData.MusicOnOff = "OFF";
            NGUITools.SetActive(mainGM.Img_MusicOn, false);
            NGUITools.SetActive(mainGM.Img_MusicOff, true);
        }
        else
        {
            SaveData.MusicOnOff = "ON";
            NGUITools.SetActive(mainGM.Img_MusicOn, true);
            NGUITools.SetActive(mainGM.Img_MusicOff, false);
        }
    }

    public void ChangeSFXOption()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ChangeSFXOption()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        if (SaveData.SFXOnOff.Equals("ON"))
        {
            SaveData.SFXOnOff = "OFF";
            NGUITools.SetActive(mainGM.Img_SFXOn, false);
            NGUITools.SetActive(mainGM.Img_SFXOff, true);
        }
        else
        {
            SaveData.SFXOnOff = "ON";
            NGUITools.SetActive(mainGM.Img_SFXOn, true);
            NGUITools.SetActive(mainGM.Img_SFXOff, false);
        }
    }

    public void ChangeLANGOption()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ChangeLANGOption()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        if (SaveData.Language.Equals("한국어"))
        {
            mainGM.Lb_Language.text = "English";
        }
        else
        {
            mainGM.Lb_Language.text = "한국어";
        }
        SaveData.Language = mainGM.Lb_Language.text;
        Debug.Log("Language: " + SaveData.Language);
        //StartCoroutine("LANGOptionCoroutine");
    }
    /*
    IEnumerator LANGOptionCoroutine()
    {
        yield return null;
        if (SaveData.Language != Localization.language)
        {
            mainGM.isLANGOptionChanged = true;
            SaveData.Language = Localization.language;
            mainGM.Lb_MusicOn.text = Localization.Get("Music") + " " + SaveData.MusicOnOff;
            mainGM.Lb_SFXOn.text = Localization.Get("SFX") + " " + SaveData.SFXOnOff;
            mainGM.Lb_AutoRestartOnOff.text = Localization.Get("AutoRestart") + " " + SaveData.AutoRestartOnOff;
        }
        yield break;
    }
    */

    public void ChangeControlsOption()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ChangeControlsOption()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        if (SaveData.Controls.Equals("Vertical")) SaveData.Controls = "Horizontal"; else SaveData.Controls = "Vertical";
        mainGM.Lb_Controls.text = Localization.Get("Controls") + " : " + Localization.Get(SaveData.Controls);
    }

    public void ChangeAutoRestartOption()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ChangeAutoRestartOption()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        if (SaveData.AutoRestartOnOff.Equals("ON"))
        {
            SaveData.AutoRestartOnOff = "OFF";
            NGUITools.SetActive(mainGM.Img_AutoRestartOn, false);
            NGUITools.SetActive(mainGM.Img_AutoRestartOff, true);
        }
        else
        {
            SaveData.AutoRestartOnOff = "ON";
            NGUITools.SetActive(mainGM.Img_AutoRestartOn, true);
            NGUITools.SetActive(mainGM.Img_AutoRestartOff, false);
        }
    }

    public void ConnectToGoogleServicesFromSettings()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        if (GPGSMgr.Instance.Authenticated)
        {
            GPGSMgr.Instance.SignOutFromGooglePlay();
            NGUITools.SetActive(mainGM.GooglePlayLogin_Btn_Check, false);
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    NGUITools.SetActive(mainGM.GooglePlayLogin_Btn_Check, true);
                }
                else
                {
                    NGUITools.SetActive(mainGM.GooglePlayLogin_Btn_Check, false);
                }
            });
        }

        /*
        if (Social.Active.localUser.authenticated)
        {
            GooglePlayGames.PlayGamesPlatform.Instance.SignOut();
            NGUITools.SetActive(mainGM.GooglePlayLogin_Btn_Check, false);
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                mainGM.isConnectedToGoogleServices = success;
                if (success)
                    NGUITools.SetActive(mainGM.GooglePlayLogin_Btn_Check, true);
                else
                    NGUITools.SetActive(mainGM.GooglePlayLogin_Btn_Check, false);
                }
            );
        }
        */
    }

    public void ConnectToFacebookFromSettings()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(null, AuthCallback);
        }
        else
        {
            FB.LogOut();
            NGUITools.SetActive(mainGM.FacebookLogin_Btn_Check, false);
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            NGUITools.SetActive(mainGM.FacebookLogin_Btn_Check, true);
        }
        else
        {
            NGUITools.SetActive(mainGM.FacebookLogin_Btn_Check, false);
        }
    }

    public void ConnectToOfficalWebsite()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        Application.OpenURL(string.Format("http://{0}", officalWebsiteURL));
    }

    public void ConnectToComposer()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        Application.OpenURL(string.Format("http://{0}", composerURL));
    }

    public void ConnectToIllustrator()
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        Application.OpenURL(string.Format("http://{0}", illustratorURL));
    }

    public void OpenStageContactsPopup(GameObject go)
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> OpenStageContactsPopup()");
#endif
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        GameObject pn_Popup_Contacts = go.transform.Find("Pn_Popup_Contacts").gameObject;
        NGUITools.SetActive(pn_Popup_Contacts, true);
    }

    IEnumerator OpenStageContactsCo(GameObject go)
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        mainGM.HideAllUI();
        TweenAlpha.Begin(go.transform.Find("Icon_MUSIC").gameObject, 0.3f, 0.0f);
        TweenAlpha.Begin(go.transform.Find("Icon_ART").gameObject, 0.3f, 0.0f);
        TweenAlpha.Begin(go.transform.Find("Lb_ART_MUSIC").gameObject, 0.3f, 0.0f);
        TweenAlpha.Begin(go.transform.Find("Lb_ART_MUSIC_Credits").gameObject, 0.3f, 0.0f);
        //yield return new WaitForSeconds(0.5f);
        //NGUITools.SetActive(go, true);
        yield return new WaitForSeconds(0.5f);
        GameObject pn_Popup_Contacts = go.transform.Find("Pn_Popup_Contacts").gameObject;
        NGUITools.SetActive(pn_Popup_Contacts, true);
        TweenAlpha.Begin(pn_Popup_Contacts, 0.5f, 1.0f);
        yield return new WaitForSeconds(0.5f);
    }

    public void CloseStageContactsPopup(GameObject go)
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

         //StartCoroutine("CloseStageContactsCo", go);
        GameObject pn_Popup_Contacts = go.transform.Find("Pn_Popup_Contacts").gameObject;
        NGUITools.SetActive(pn_Popup_Contacts, false);
    }

    IEnumerator CloseStageContactsCo(GameObject go)
    {
        LobbySoundManager.Instance.PlaySFX(LobbySFX.COMMON_BTN);

        TweenAlpha.Begin(go.transform.Find("Icon_MUSIC").gameObject, 0.0f, 0.0f);
        TweenAlpha.Begin(go.transform.Find("Icon_ART").gameObject, 0.0f, 0.0f);
        TweenAlpha.Begin(go.transform.Find("Lb_ART_MUSIC").gameObject, 0.0f, 0.0f);
        TweenAlpha.Begin(go.transform.Find("Lb_ART_MUSIC_Credits").gameObject, 0.0f, 0.0f);
        GameObject pn_Popup_Contacts = go.transform.Find("Pn_Popup_Contacts").gameObject;
        TweenAlpha.Begin(pn_Popup_Contacts, 0.7f, 0);
        yield return new WaitForSeconds(0.5f);
        NGUITools.SetActive(pn_Popup_Contacts, false);
        TweenAlpha.Begin(go.transform.Find("Icon_MUSIC").gameObject, 0.5f, 1.0f);
        TweenAlpha.Begin(go.transform.Find("Icon_ART").gameObject, 0.5f, 1.0f);
        TweenAlpha.Begin(go.transform.Find("Lb_ART_MUSIC").gameObject, 0.5f, 1.0f);
        TweenAlpha.Begin(go.transform.Find("Lb_ART_MUSIC_Credits").gameObject, 0.5f, 1.0f);
        mainGM.ShowAllUIElements(0.5f);
        yield return new WaitForSeconds(0.6f);
        yield break;
    }
}
