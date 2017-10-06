using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainGM : MonoBehaviour
{
    public static MainGM Instance { get; private set; }
    public bool isPaused { get; set; }
    public bool isLANGOptionChanged { get; set; }
    public bool isAndroidAudioLoaded { get; set; }
    public bool isFirstOnCenter { get; set; }
    public GameObject previousStage { get; set; }

    //Background Array
    public GameObject[] BgImg;

    public GameObject Pnl_Input;

    public LanguageSelection langSelection;
    public UIScrollView stageSelect_Scrollview;
    public Transform DragController;
    public UICenterOnChild centerOnChild;
    public GameObject[] gridStages;
    public GameObject[] mainButtons;
    public GameObject pn_FadeOut;
    public GameObject pn_Main;
    public GameObject pn_Popup_Settings;
    public UIScrollView settings_ScrollView;
    public GameObject Img_MusicOn;
    public GameObject Img_MusicOff;
    public GameObject Img_SFXOn;
    public GameObject Img_SFXOff;
    public GameObject Img_AutoRestartOn;
    public GameObject Img_AutoRestartOff;

    public UILabel Lb_Language;
    public UILabel Lb_Controls;
    public UILabel Lb_AutoRestartOnOff;
    public GameObject GooglePlayLogin_Btn_Check;
    public GameObject FacebookLogin_Btn_Check;

    public GameObject pn_Popup_Credits;
    public UIScrollView credits_ScrollView;
    public GameObject pn_Popup_TermsOfUse;
    public UIScrollView termsOfUse_ScrollView;
    public GameObject pn_Popup_Quit;
    public GameObject img_Main_Character;

    public GameObject btn_Credits;
    public GameObject btn_TermsOfUse;

    public int currStageIdx { get; set; }

    enum Difficulty
    {
        Locked, //0
        Easy,   //1
        Normal, //2
        Hard    //3
    }

    void Awake()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> Awake()");
#endif
        GC.Collect();

        //SNS Settings
        //isConnectedToGoogleServices = Social.Active.localUser.authenticated;
        //PlayGamesPlatform.Activate();
        //PlayGamesPlatform.DebugLogEnabled = false;
        //if (!FB.IsInitialized) FB.Init(); else FB.ActivateApp();
        GPGSMgr.Instance.Initialize();
        FacebookMgr.GetInstance.Initialize();

        Instance = this;
        isPaused = false;
        isLANGOptionChanged = false;
        isFirstOnCenter = true;
        previousStage = gridStages[0];

        //설정정보 로드
        SaveData.LoadOption();
        //스테이지별 난이도 로드
        LoadAndSetDifficulty();
        //최고점수 로드
        LoadAndSetHighScore();
        //언어설정
        SetLocalization();

        pn_FadeOut.transform.position = new Vector3(0, 0, 0);
        pn_Main.transform.position = new Vector3(0, 0, 0);
        pn_Popup_Settings.transform.position = new Vector3(0, 0, 0);
        pn_Popup_Credits.transform.position = new Vector3(0, 0, 0);
        pn_Popup_TermsOfUse.transform.position = new Vector3(0, 0, 0);
        pn_Popup_Quit.transform.position = new Vector3(0, 0, 0);

        NGUITools.SetActive(pn_FadeOut, false);
        NGUITools.SetActive(pn_Main, true);
        NGUITools.SetActive(pn_Popup_Settings, false);
        NGUITools.SetActive(pn_Popup_Credits, false);
        NGUITools.SetActive(pn_Popup_TermsOfUse, false);
        NGUITools.SetActive(pn_Popup_Quit, false);
    }

    void Start()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> Start()");
#endif
        stageSelect_Scrollview.onDragStarted = HideAllUI;
        centerOnChild.onCenter = KnowWhatIsCentered;

        if (SaveData.MusicOnOff.Equals("ON"))
        {
            LobbySoundManager.Instance.UnmuteBGM();
        }
        else
        {
            LobbySoundManager.Instance.MuteBGM();
        }
        //stage00Music.Play();
        LobbySoundManager.Instance.PlayBGM();

        StartCoroutine("MainCharacterMotion");
    }

    IEnumerator MainCharacterMotion()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> Start()");
#endif
        InstaHideAllUI();
        ShowAllUIElements(0.0f);
        img_Main_Character.GetComponent<TweenTransform>().enabled = true;
        yield return new WaitForSeconds(1.05f);
        img_Main_Character.GetComponent<TweenTransform>().enabled = false;
        img_Main_Character.GetComponent<TweenPosition>().enabled = true;
        isFirstOnCenter = false;
        yield break;
    }

    void OnApplicationPause(bool pause)
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> OnApplicationPause() : " + pause);
#endif
        if (pause) PauseGame(); else ResumeGame();
    }

    void OnApplicationQuit()
    {

    }

    void PauseGame()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> PauseGame()");
#endif
        isPaused = true;
        LobbySoundManager.Instance.PauseBGM();
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> ResumeGame()");
#endif
        isPaused = false;
        Time.timeScale = 1;
        LobbySoundManager.Instance.PlayBGM();
    }

    public void ClickPlayStage(GameObject go)
    {
        StartCoroutine(GoOnToSelectedStage(go));
    }

    IEnumerator GoOnToSelectedStage(GameObject go)
    {
#if UNITY_EDITOR
        Debug.Log(this.name + " >>> PlayStage >>> " + go.name + " Pressed. Loading " + go.name.Substring(0, 7) + "...");
#endif
        LobbySoundManager.Instance.StopBGM();
        LobbySoundManager.Instance.PlaySFX(LobbySFX.PLAY_BTN);
        GameObject Btn_PlayStage = go.transform.Find("Btn_PlayStage").gameObject;
        GameObject Img_PlayBrightness = Btn_PlayStage.transform.Find("Img_PlayBrightness").gameObject;
        GameObject Img_Play = Btn_PlayStage.transform.Find("Img_Play").gameObject;
        GameObject Img_Brightness = Btn_PlayStage.transform.Find("Img_Brightness").gameObject;
        HideAllUI();
        //플레이 스프라이트 교체
        NGUITools.SetActive(Img_PlayBrightness, false);
        NGUITools.SetActive(Img_Play, true);
        NGUITools.SetActive(Img_Brightness, true);
        //Brightness 효과
        TweenAlpha.Begin(Img_Brightness, 0.2f, 0.0f);
        //Scale 효과 0.0f->1.2f>3.0f'
        TweenScale.Begin(Img_Play, 0.2f, new Vector3(2.5f, 2.5f, 0.0f));
        //Alpha 효과
        TweenAlpha.Begin(Img_Play, 0.2f, 0.0f);
        NGUITools.SetActive(pn_FadeOut, true);
        pn_FadeOut.GetComponent<UIPanel>().alpha = 0f;
        yield return new WaitForSeconds(0.5f);

        LobbySoundManager.Instance.PlaySFX(LobbySFX.STAGE_START);
        yield return new WaitForSeconds(0.3f);
        TweenAlpha.Begin(pn_FadeOut.gameObject, 0.7f, 1f);

        //Splash 0, Main 1이므로 각 stage의 index는 currStageIdx + 2
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currStageIdx + 2);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            while (asyncOperation.progress == 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

        yield break;
    }

    public void SetOptionPopup()
    {
        Img_MusicOn.transform.Find("Lb_MusicOn").GetComponent<UILabel>().text = Localization.Get("Music") + " ON";
        Img_MusicOff.transform.Find("Lb_MusicOff").GetComponent<UILabel>().text = Localization.Get("Music") + " OFF";
        if (SaveData.MusicOnOff.Equals("ON"))
        {
            NGUITools.SetActive(Img_MusicOn, true);
            NGUITools.SetActive(Img_MusicOff, false);
        }
        else
        {
            NGUITools.SetActive(Img_MusicOn, false);
            NGUITools.SetActive(Img_MusicOff, true);
        }

        Img_SFXOn.transform.Find("Lb_SFXOn").GetComponent<UILabel>().text = Localization.Get("SFX") + " ON";
        Img_SFXOff.transform.Find("Lb_SFXOff").GetComponent<UILabel>().text = Localization.Get("SFX") + " OFF";
        if (SaveData.SFXOnOff.Equals("ON"))
        {
            NGUITools.SetActive(Img_SFXOn, true);
            NGUITools.SetActive(Img_SFXOff, false);
        }
        else
        {
            NGUITools.SetActive(Img_SFXOn, false);
            NGUITools.SetActive(Img_SFXOff, true);
        }

        Lb_Language.text = Localization.Get("Language");

        Lb_Controls.text = Localization.Get("Controls") + " : ";
        if (SaveData.Controls.Equals("Vertical")) Lb_Controls.text += Localization.Get("Vertical"); else Lb_Controls.text += Localization.Get("Horizontal");

        Img_AutoRestartOn.transform.Find("Lb_AutoRestartOn").GetComponent<UILabel>().text = Localization.Get("AutoRestart") + " ON";
        Img_AutoRestartOff.transform.Find("Lb_AutoRestartOff").GetComponent<UILabel>().text = Localization.Get("AutoRestart") + " OFF";
        if (SaveData.AutoRestartOnOff.Equals("ON"))
        {
            NGUITools.SetActive(Img_AutoRestartOn, true);
            NGUITools.SetActive(Img_AutoRestartOff, false);
        }
        else
        {
            NGUITools.SetActive(Img_AutoRestartOn, false);
            NGUITools.SetActive(Img_AutoRestartOff, true);
        }

        //GooglePlay 연동 확인
        if (GPGSMgr.Instance.Authenticated)
        {
            NGUITools.SetActive(GooglePlayLogin_Btn_Check, true);
        }
        else
        {
            NGUITools.SetActive(GooglePlayLogin_Btn_Check, false);
        }

        //Facebook 연동 확인
        if (FacebookMgr.GetInstance.IsLoggedIn)
        {
            NGUITools.SetActive(FacebookLogin_Btn_Check, true);
        }
        else
        {
            NGUITools.SetActive(FacebookLogin_Btn_Check, false);
        }
    }

    public void ReloadMainScreen()
    {
        // Restart MainScreen
        SceneManager.LoadScene("Main");
    }

    void SetLocalization()
    {
        if (SaveData.Language.Equals("")) Localization.language = "한국어;"; else Localization.language = SaveData.Language;
    }

    public void KnowWhatIsCentered(GameObject centeredStage)
    {
        if (isFirstOnCenter)
            return;

        if (centeredStage == gridStages[0]) currStageIdx = 0;
        else if (centeredStage == gridStages[1]) currStageIdx = 1;
        else if (centeredStage == gridStages[2]) currStageIdx = 2;
        else if (centeredStage == gridStages[3]) currStageIdx = 3;
        else if (centeredStage == gridStages[4]) currStageIdx = 4;
        else if (centeredStage == gridStages[5]) currStageIdx = 5;

        Debug.Log("KnowWhatIsCentered");

        ChangeStageBackground(currStageIdx);
        LobbySoundManager.Instance.ChangeBGM(currStageIdx);
        LobbySoundManager.Instance.PlayBGM();

        previousStage = centeredStage;
    }

    public void ChangeStageBackground(int stageIndex)
    {
        for (int i = 0; i < BgImg.Length; i++)
        {
            if (i == stageIndex)
            {
                NGUITools.SetActive(BgImg[i], true);
            }
            else
            {
                NGUITools.SetActive(BgImg[i], false);
            }
        }
    }

    public void LoadAndSetHighScore()
    {
        SaveData.LoadHiScore();
        for (int i = 0; i < SaveData.HiScore.Length; i++)
        {
            Debug.Log(i + ": " + SaveData.HiScore[i]);
            UILabel label = gridStages[i].transform.Find("Lb_ScoreHighscore").GetComponent<UILabel>();
            label.text = SaveData.HiScore[i].ToString();
        }
    }

    public void LoadAndSetDifficulty()
    {
        SaveData.LoadStageDifficulty();
        for (int i = 1; i < gridStages.Length; i++)
        {
            if (SaveData.StageDifficulty[i] > (int)Difficulty.Locked)
            {
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_PlayBrightness").gameObject, true);
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Play").gameObject, true);
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Brightness").gameObject, true);
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Locked").gameObject, false);
            }
            else
            {
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_PlayBrightness").gameObject, false);
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Play").gameObject, false);
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Brightness").gameObject, false);
                NGUITools.SetActive(gridStages[i].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Locked").gameObject, true);
            }
        }
    }

    public void ChangeDifficulty(GameObject easy, GameObject normal, GameObject hard)
    {
        int difficulty = (int)Difficulty.Locked;
        bool isUnlocked = false;

        if (easy.activeInHierarchy)
        {
            NGUITools.SetActive(easy, false);
            NGUITools.SetActive(normal, false);
            NGUITools.SetActive(hard, false);
            NGUITools.SetActive(normal, true);
            difficulty = (int)Difficulty.Easy;
        }
        else if (normal.activeInHierarchy)
        {
            NGUITools.SetActive(easy, false);
            NGUITools.SetActive(normal, false);
            NGUITools.SetActive(hard, false);
            NGUITools.SetActive(hard, true);
            difficulty = (int)Difficulty.Normal;
        }
        else if (hard.activeInHierarchy)
        {
            NGUITools.SetActive(easy, false);
            NGUITools.SetActive(normal, false);
            NGUITools.SetActive(hard, false);
            NGUITools.SetActive(easy, true);
            difficulty = (int)Difficulty.Hard;
        }

        if (difficulty <= SaveData.StageDifficulty[currStageIdx])
        {
            isUnlocked = true;
        }
        else
        {
            isUnlocked = false;
        }

        NGUITools.SetActive(gridStages[currStageIdx].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Play").gameObject, isUnlocked);
        NGUITools.SetActive(gridStages[currStageIdx].transform.Find("Btn_PlayStage").gameObject.transform.Find("Img_Locked").gameObject, !isUnlocked);
    }

    public void ShowAllUIElements(float duration)
    {
        //for (int i = 0; i < mainButtons.Length; i++)
        //{
        //    TweenAlpha.Begin(mainButtons[i], duration, 1.0f);
        //}
        TweenAlpha.Begin(Pnl_Input, duration, 1.0f);
        GameObject BtnSettings = Pnl_Input.transform.Find("Btn_Settings").gameObject;
        TweenAlpha.Begin(BtnSettings, duration, 1.0f);

        for (int i = 0; i < gridStages.Length; i++)
        {
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageNum").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageName").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageTime").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Difficulty").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_TextHighscore").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_ScoreHighscore").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_Contacts").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_PlayStage").gameObject, duration, 1.0f);
            //임시처리
            if (i == 0)
            {
                GameObject BtnPlayStage = gridStages[i].transform.Find("Btn_PlayStage").gameObject;
                BtnPlayStage.transform.Find("Img_PlayBrightness").gameObject.SetActive(true);
            }
        }
    }

    public void HideAllUI()
    {
        for (int i = 0; i < mainButtons.Length; i++)
        {
            TweenAlpha.Begin(mainButtons[i], 0.5f, 0.0f);
        }

        for (int i = 0; i < gridStages.Length; i++)
        {
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageNum").gameObject, 0.6f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageName").gameObject, 0.6f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageTime").gameObject, 0.6f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Difficulty").gameObject, 0.55f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_TextHighscore").gameObject, 0.5f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_ScoreHighscore").gameObject, 0.5f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_Contacts").gameObject, 0.5f, 0.0f);
            GameObject BtnPlayStage = gridStages[i].transform.Find("Btn_PlayStage").gameObject;
            TweenAlpha.Begin(BtnPlayStage, 0.5f, 0.0f);
            BtnPlayStage.transform.Find("Img_PlayBrightness").gameObject.SetActive(false);
        }
    }

    public void InstaHideAllUI()
    {
        //for (int i = 0; i < mainButtons.Length; i++)
        //{
        //TweenAlpha.Begin(mainButtons[i], 0.001f, 0.0f);
        //}

        TweenAlpha.Begin(Pnl_Input, 0.001f, 0.0f);

        for (int i = 0; i < gridStages.Length; i++)
        {
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageNum").gameObject, 0.001f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageName").gameObject, 0.001f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageTime").gameObject, 0.001f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Difficulty").gameObject, 0.001f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_TextHighscore").gameObject, 0.001f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_ScoreHighscore").gameObject, 0.001f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_Contacts").gameObject, 0.001f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_PlayStage").gameObject, 0.001f, 0.0f);
            GameObject BtnPlayStage = gridStages[i].transform.Find("Btn_PlayStage").gameObject;
            BtnPlayStage.transform.Find("Img_PlayBrightness").gameObject.SetActive(false);
        }
    }

    public void ShowSettingsUI()
    {
        TweenAlpha.Begin(pn_Popup_Settings.transform.Find("Btn_Close").gameObject, 0.5f, 1.0f);
        TweenAlpha.Begin(pn_Popup_Settings.transform.Find("ScrollBar").gameObject, 0.5f, 1.0f);
        TweenAlpha.Begin(pn_Popup_Settings.transform.Find("ScrollView").gameObject, 0.5f, 1.0f);
    }

    public void HideSettingsUI()
    {
        TweenAlpha.Begin(pn_Popup_Settings.transform.Find("Btn_Close").gameObject, 0.5f, 0.0f);
        TweenAlpha.Begin(pn_Popup_Settings.transform.Find("ScrollBar").gameObject, 0.5f, 0.0f);
        TweenAlpha.Begin(pn_Popup_Settings.transform.Find("ScrollView").gameObject, 0.5f, 0.0f);
    }

    public void ShowUI(float duration)
    {
        for (int i = 0; i < gridStages.Length; i++)
        {
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageNum").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageName").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageTime").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Difficulty").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_TextHighscore").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_ScoreHighscore").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_Contacts").gameObject, duration, 1.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_PlayStage").gameObject, duration, 1.0f);
            //임시처리
            if (i == 0)
            {
                GameObject BtnPlayStage = gridStages[i].transform.Find("Btn_PlayStage").gameObject;
                BtnPlayStage.transform.Find("Img_PlayBrightness").gameObject.SetActive(true);
            }
        }
    }

    public void HideUI()
    {
        for (int i = 0; i < gridStages.Length; i++)
        {
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageNum").gameObject, 0.6f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageName").gameObject, 0.6f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_StageTime").gameObject, 0.6f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Difficulty").gameObject, 0.55f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_TextHighscore").gameObject, 0.5f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Lb_ScoreHighscore").gameObject, 0.5f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_Contacts").gameObject, 0.5f, 0.0f);
            TweenAlpha.Begin(gridStages[i].transform.Find("Btn_PlayStage").gameObject, 0.5f, 0.0f);
        }
    }
}
