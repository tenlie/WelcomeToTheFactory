using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    //싱글톤 패턴. {get; private set;} = readonly
    public static LevelManager Instance { get; private set; }

    //스테이지
    public int StageFPS;
    public string StageName { get; private set; }
    public int StageIdx { get; private set; }
    public string SongName;
    public bool IsGameStarted { get; set; }
    public bool IsPaused { get; private set; }
    public bool IsStageClear { get; private set; }

    //점수/콤보
    public bool HasNewRecord { get; set; }
    public int HighScore { get; set; }
    public int CurrScore { get; set; }
    public int PrevScore { get; set; }
    private bool _isShowingResult;
    private int _comboCnt;
    private int _comboArrIdx;
    public GameObject[] ComboLblArr;
    public GameObject ComboEffect;
    public Vector2 ComboEffectPos;
    private Color _yellow;
    private Color _grey;
    public Player Player;

    //일시정지 팝업
    public GameObject Pnl_Popup_Paused;
    public UILabel Paused_Lbl_Songname;
    public UILabel Paused_Txt_HighScore;

    //성공 팝업
    public GameObject Pnl_Popup_Clear;
    public UILabel Clear_Lbl_Songname;
    public UILabel Clear_Lbl_HighScore;
    public UILabel Clear_Txt_HighScore;
    public UILabel Clear_Lbl_Score;
    public UILabel Clear_Txt_Score;
    public UILabel Clear_Lbl_NewRecord;

    //실패 팝업
    public GameObject Pnl_Popup_Fail;
    public UILabel Fail_Lbl_Songname;
    public UILabel Fail_Lbl_HighScore;
    public UILabel Fail_Txt_HighScore;
    public UILabel Fail_Lbl_Score;
    public UILabel Fail_Txt_Score;
    public UILabel Fail_Lbl_NewRecord;

    //스테이지
    public GameObject Pnl_Stage;
    public GameObject VerticalControlUI;
    public GameObject HorizontalControlUI;
    public GameObject Pnl_FadeOut;

    //점수판정
    //public GameObject[] EvalQuotes;
    public Transform EvalQuote;
    public Transform EvalQuotePos;
    public UILabel Lbl_Evaluation;
    public UILabel Lbl_CurrScore;
    public CEvalQuoteCtrl EvalQuoteCtrl;
    private EvalState state;

    //=========================================================================
    //Bgm 제어를 위한 변수
    //Bgm for Android
    public string BgmFileName;
    public int BgmID;
    private bool _isBgmLoaded;
    //Bgm for StandAlone
    public AudioSource BgmAudio;
    //Sync
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }
    private DateTime _started;
    private DateTime _previousFrameTime;
    private float _lastReportedPlayheadPosition;
    private float _songTime;
    public Transform Foreground;
    //==========================================================================

    public void Awake()
    {
        Instance = this;

        //가비지 컬렉트
        GC.Collect();
        //인게임 프레임 고정
        Application.targetFrameRate = StageFPS;
        QualitySettings.vSyncCount = 0;
        //노트음원 메모리 로드
        AndroidNativeAudio.makePool();
        //LoadBgm();
        //데이터 로드
        //재생 및 싱크 처리
        SaveData.LoadOption();
        SaveData.LoadHiScore();
        ScreenFade.Fade(Color.black, 1f, 0f, 2f, 0f, true);


        StageName = SceneManager.GetActiveScene().name;
        StageIdx = int.Parse(StageName.Substring(5, 2));
        IsGameStarted = false;
        IsPaused = false;
        IsStageClear = false;

        HighScore = SaveData.HiScore[StageIdx];
        PrevScore = 0;
        CurrScore = 0;
        HasNewRecord = false;

        _comboCnt = 0;
        _comboArrIdx = 0;

        _isShowingResult = false;
        _yellow = new Color(); ColorUtility.TryParseHtmlString("#FFEB06", out _yellow);
        _grey = new Color(); ColorUtility.TryParseHtmlString("#C8C8C8", out _grey);

        Paused_Lbl_Songname.text = SongName;
        Paused_Txt_HighScore.text = SaveData.HiScore[StageIdx].ToString();
        Pnl_Popup_Paused.transform.localPosition = Vector3.zero;
        NGUITools.SetActive(Pnl_Popup_Paused, false);

        Fail_Lbl_Songname.text = SongName;
        Fail_Txt_HighScore.text = SaveData.HiScore[0].ToString();
        Pnl_Popup_Fail.transform.localPosition = Vector3.zero;
        NGUITools.SetActive(Fail_Lbl_NewRecord.gameObject, false);
        NGUITools.SetActive(Pnl_Popup_Fail, false);

        Clear_Lbl_Songname.text = SongName;
        Clear_Txt_HighScore.text = SaveData.HiScore[StageIdx].ToString();
        Pnl_Popup_Clear.transform.localPosition = Vector3.zero;
        NGUITools.SetActive(Clear_Lbl_NewRecord.gameObject, false);
        NGUITools.SetActive(Pnl_Popup_Clear, false);

        Pnl_FadeOut.transform.localPosition = Vector3.zero;
        //NGUITools.SetActive(Lbl_Evaluation.gameObject, false);

        if (SaveData.Controls.Equals("Vertical"))
        {
            NGUITools.SetActive(VerticalControlUI, true);
            NGUITools.SetActive(HorizontalControlUI, false);
        }
        else
        {
            NGUITools.SetActive(VerticalControlUI, false);
            NGUITools.SetActive(HorizontalControlUI, true);
        }

        NGUITools.SetActive(Pnl_FadeOut, false);
        state = EvalState.PERFECT;

    }

    public void Start()
    {
        StartCoroutine(LoadBgmCo());
    }

    public void FixedUpdate()
    {
        EvalQuote.localPosition = new Vector3(-300, EvalQuotePos.position.y * 100, 0);
        //ScoreCount();
    }

    private IEnumerator LoadBgmCo()
    {
        IsGameStarted = true;
        if(BgmAudio.clip.loadState != AudioDataLoadState.Loaded)
        {
            Debug.Log("audio loading...");
            yield return null;
        }
        LoadBgm();
        yield return new WaitForSeconds(2f);
        SetSync();
    }

    public void ScoreCount()
    {
        if (Player.IsDead || IsStageClear)
            return;

        //Debug.Log("Currscore: " + CurrScore + " PrevScore: " + PrevScore);
        if (CurrScore > PrevScore)
        {
            PrevScore = PrevScore + 3;
            Lbl_CurrScore.text = PrevScore.ToString();
            if (CurrScore <= PrevScore)
            {
                PrevScore = CurrScore;
                Lbl_CurrScore.text = CurrScore.ToString();
            }
        }
    }

    public void HandleEvaluationResult(int resultIdx, int points)
    {
        AddScore(points);
        StartCoroutine(CalculateComboCo(resultIdx, points));

        //for (int i = 0; i < EvalQuotes.Length; i++)
        //{
        //    NGUITools.SetActive(EvalQuotes[i], false);
        //}

        //StartCoroutine(EvaluationResultCo(resultIdx, points));
        ShowEvalQuote(resultIdx);
    }

    public void ShowEvalQuote(int resultIdx)
    {
        if (resultIdx == 0)
        {
            state = EvalState.PERFECT;
        }
        else if (resultIdx == 1)
        {
            state = EvalState.GREAT;
        }
        else if (resultIdx == 2)
        {
            state = EvalState.GOOD;
        }
        else if (resultIdx == 3)
        {
            state = EvalState.MISS;
        }
        EvalQuoteCtrl.ResetQuote(state);
        Debug.Log("hehe");
    }

    public void AddScore(int points)
    {
        if (points == 0)
            return;

        CurrScore += points;
        Lbl_CurrScore.text = CurrScore.ToString();

        //최고점수 갱신여부 확인
        if (CurrScore > HighScore)
        {
            if (!HasNewRecord)
            {
                Debug.Log("NEW HIGHSCORE!");
                Lbl_CurrScore.gameObject.GetComponent<TweenColor>().enabled = true;
                HasNewRecord = true;
            }
        }
    }

    //콤보 처리
    IEnumerator CalculateComboCo(int resultIdx, float points)
    {
        switch (resultIdx)
        {
            case 3:
            case 2:
            case 1:
                _comboCnt = 0;
                break;
            case 0:
                _comboCnt++;
                break;
            default:
                break;
        }

        if (_comboCnt <= 1)
            yield break;

        UILabel lbl_ComboNum = ComboLblArr[_comboArrIdx].GetComponent<UILabel>();
        NGUITools.SetActive(lbl_ComboNum.gameObject, true);
        lbl_ComboNum.text = _comboCnt.ToString();
        TweenScale.Begin(lbl_ComboNum.gameObject, 0.2f, Vector3.one);
        if (_comboCnt >= 10)
        {
            Instantiate(ComboEffect, ComboEffectPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.2f);

        TweenAlpha.Begin(lbl_ComboNum.gameObject, 2f, 0);
        TweenPosition.Begin(lbl_ComboNum.gameObject, 2f, new Vector2(-580, 232));
        if (_comboArrIdx == 9) _comboArrIdx = 0; else _comboArrIdx++;

        yield return new WaitForSeconds(2.0f);

        NGUITools.SetActive(lbl_ComboNum.gameObject, false);
        TweenAlpha.Begin(lbl_ComboNum.gameObject, 0.001f, 1);
        TweenScale.Begin(lbl_ComboNum.gameObject, 0.001f, Vector3.one * 3);
        TweenPosition.Begin(lbl_ComboNum.gameObject, 0.001f, new Vector2(-530, 12));
    }

    //판정 처리
    /*
    IEnumerator EvaluationResultCo(int resultIdx, float points)
    {
        _isShowingResult = true;
        NGUITools.SetActive(EvalQuotes[resultIdx], true);
        yield return new WaitForSeconds(2.0f);
        NGUITools.SetActive(EvalQuotes[resultIdx], false);
        _isShowingResult = false;
    }
    */

    //점수 디스플레이
    IEnumerator DisplayScore(UILabel label, int increment)
    {
        if (CurrScore > 0)
        {
            int score = 0;
            while (score < CurrScore)
            {
                score = score + increment;
                label.text = score.ToString();
                yield return null;
            }
        }
    }

    //스테이지 클리어 처리
    public void LevelClear()
    {
        StartCoroutine(LevelClearCo());
        Invoke("FinishStage", 5f);
    }

    IEnumerator LevelClearCo()
    {
        StopBgm();
        IsStageClear = true;
        GameObject resultLevel = Pnl_Popup_Clear.transform.Find("Img_Level").gameObject;
        TweenAlpha.Begin(resultLevel, 0.001f, 0.0f);
        NGUITools.SetActive(Pnl_Popup_Clear, true);

        StartCoroutine(DisplayScore(Clear_Txt_Score, 10));

        if (HasNewRecord)
        {
            HighScore = CurrScore;
            SaveData.HiScore[StageIdx] = (int)HighScore;
            SaveData.SaveHiScore();
            NGUITools.SetActive(Clear_Lbl_NewRecord.gameObject, true);
            Clear_Txt_Score.gameObject.GetComponent<TweenColor>().enabled = true;
        }

        yield return new WaitForSeconds(0.4f);

        TweenPosition.Begin(resultLevel, 0.2f, new Vector2(290, -45));
        TweenScale.Begin(resultLevel, 0.2f, new Vector3(1, 1, 1));
        TweenAlpha.Begin(resultLevel, 0.2f, 1.0f);
    }

    void FinishStage()
    {
        Time.timeScale = 0f;
    }

    public void KillPlayer()
    {
        //StopAllCoroutines();
        NGUITools.SetActive(EvalQuoteCtrl.gameObject, false);
        _isShowingResult = false;
        StartCoroutine(KillPlayerCo());
    }

    private IEnumerator KillPlayerCo()
    {
        Player.Kill();
        StopBgm();

        yield return new WaitForSeconds(2.0f);

        if (HasNewRecord)
        {
            HighScore = CurrScore;
            SaveData.HiScore[StageIdx] = (int)HighScore;
            SaveData.SaveHiScore();
            NGUITools.SetActive(Fail_Lbl_NewRecord.gameObject, true);
            Fail_Lbl_Score.color = _yellow;
            Fail_Txt_Score.color = _yellow;
            Fail_Lbl_HighScore.color = _grey;
            Fail_Txt_HighScore.color = _grey;
        }

        if (SaveData.AutoRestartOnOff.Equals("ON"))
        {
            RestartStage();
        }
        else
        {
            NGUITools.SetActive(Pnl_Popup_Fail, true);
            StartCoroutine(DisplayScore(Fail_Txt_Score, 10));
            Time.timeScale = 0f;
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PauseGame();
        }
    }

    void OnApplicationQuit()
    {
        StopBgm();
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            ReleaseBgm();
            AndroidNativeAudio.releasePool();
        }
        */
    }

    //게임 일시정지
    public void PauseGame()
    {
        Debug.Log(this.name + " >>> PauseGame()");

        if (Player._controller.IsDead || IsStageClear)
            return;

        IsPaused = true;
        PauseBgm();
        Time.timeScale = 0;
        NGUITools.SetActive(Pnl_Popup_Paused, true);
    }

    //계속하기
    public void ResumeGame()
    {
        Debug.Log(this.name + " >>> ResumeGame()");

        if (!IsGameStarted || IsStageClear)
            return;

        IsPaused = false;
        Time.timeScale = 1.0f;
        PlayBgm();
        NGUITools.SetActive(Pnl_Popup_Paused, false);
    }

    //메인으로 돌아가기
    public void ReturnToMain()
    {
        if (HasNewRecord)
        {
            HighScore = CurrScore;
            SaveData.HiScore[StageIdx] = (int)HighScore;
            SaveData.SaveHiScore();
        }
        NGUITools.SetActive(Pnl_FadeOut, true);
        Time.timeScale = 1.0f;
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            ReleaseBgm();
            AndroidNativeAudio.releasePool();
        }
        */
        StartCoroutine(LoadMainScreenCo());
    }

    IEnumerator LoadMainScreenCo()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
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
    }

    //스테이지 재시작
    public void RestartStage()
    {
        if (HasNewRecord)
        {
            HighScore = CurrScore;
            SaveData.HiScore[StageIdx] = (int)HighScore;
            SaveData.SaveHiScore();
        }
        NGUITools.SetActive(Pnl_FadeOut, true);
        Time.timeScale = 1.0f;

        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    ReleaseBgm();
        //    AndroidNativeAudio.releasePool();
        //}
        StartCoroutine(LoadCurrScene());
    }

    IEnumerator LoadCurrScene()
    {
        //Splash 0, Main 1이므로 각 stage의 index는 currStageIdx + 2
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(StageIdx + 2);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            while (asyncOperation.progress == 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                asyncOperation.allowSceneActivation = true;
            }
        }
    }

    public void LoadBgm()
    {
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            //BGM 음원용
            BgmFileName = "Android Native Audio/" + BgmFileName.Trim();
            BgmID = ANAMusic.load(BgmFileName, false, true, BgmLoaded, false);
        }
        else
        */
        {
            PlayBgm();
        }
    }

    public void BgmLoaded(int musicID)
    {
        _isBgmLoaded = true;
        IsGameStarted = true;
        if (BgmID == musicID)
        {
            PlayAndroidBgm();
        }
    }

    public void PlayBgm()
    {
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.play(BgmID);
        }
        else
        */
        {
            BgmAudio.Play();
        }
    }

    public void PlayAndroidBgm()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.play(BgmID);
        }
    }

    public void PauseBgm()
    {
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.pause(BgmID);
        }
        else
        */
        {
            BgmAudio.Pause();
        }
    }

    public void StopBgm()
    {
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.pause(BgmID);
            ANAMusic.seekTo(BgmID, 0);
        }
        else
        */
        {
            BgmAudio.Stop();
        }
    }

    public void ReleaseBgm()
    {
        ANAMusic.release(BgmID);
        _isBgmLoaded = false;
    }

    public double GetBGMPlayHeadPosition()
    {
        double pos = 0f;
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            pos = (ANAMusic.getCurrentPosition(BgmID) / 1000f);
        }
        else
        */
        {
            pos = Math.Round(BgmAudio.time, 3);
        }

        return pos;
    }

    public void SetSync()
    {
        /*
        _started = DateTime.UtcNow;
        _previousFrameTime = DateTime.UtcNow;
        _lastReportedPlayheadPosition = 0.0f;
        _songTime = 0.0f;
        */
        if (Application.platform == RuntimePlatform.Android)
        {
        /*
            //double bgmAudioTime = ANAMusic.getCurrentPosition(BgmID) / 1000f;
            //float posX = (float)Math.Round((bgmAudioTime * 6.4f) + 3f - 1f, 3);
            //float posX = (float)Math.Round((bgmAudioTime * 6.4f) + 3f, 3) - 1f;
            //float posX = (float)Math.Round((bgmAudioTime * 6.4f) + 3f, 3);

            //double bgmAudioTime = ANAMusic.getCurrentPosition(BgmID);
            //float posX = (float)Math.Round((bgmAudioTime * 6.4f/1000 + 3f), 3);
            // Foreground.position = new Vector3(-posX+1.2f, Foreground.position.y, Foreground.position.z);
            //Foreground.position = new Vector3(-posX, Foreground.position.y, Foreground.position.z);
            //SyncDebugger.Instance.lbl_songTimePos3.text = string.Format("bgmAudioTime : {0}, posX : {1}", bgmAudioTime, posX);
        */

            double bgmAudioTime = Math.Round(BgmAudio.time, 3);
            //float posX = (float)Math.Round(bgmAudioTime * 6.4f + 3f, 3);
            float posX = (float)Math.Round((bgmAudioTime * 6.4f) + 3f - 1f, 3);

            Foreground.position = new Vector3(-posX, Foreground.position.y, Foreground.position.z);
            SyncDebugger.Instance.lbl_songTimePos3.text = string.Format("bgmAudioTime : {0}, posX : {1}", bgmAudioTime, posX);
        }
        else
        {
            double bgmAudioTime = Math.Round(BgmAudio.time, 3);
            //float posX = (float)Math.Round(bgmAudioTime * 6.4f + 3f, 3);
            float posX = (float)Math.Round((bgmAudioTime * 6.4f) + 3f, 3);

            Foreground.position = new Vector3(-posX, Foreground.position.y, Foreground.position.z);
            SyncDebugger.Instance.lbl_songTimePos3.text = string.Format("bgmAudioTime : {0}, posX : {1}", bgmAudioTime, posX);
            //Foreground.position = new Vector3(((float)Math.Round(-(Math.Round(BgmAudio.time, 3) * 6.4f + 3f), 3)), Foreground.position.y, Foreground.position.z);
        }
    }

    public void OnPauseBtnClicked()
    {
        SoundManager.Instance.PlaySFX(InGameSFX.POPUP_BTN);
        PauseGame();
    }

    public void OnResumeBtnClicked()
    {
        SoundManager.Instance.PlaySFX(InGameSFX.POPUP_BTN);
        ResumeGame();
    }

    public void OnRestartBtnClicked()
    {
        SoundManager.Instance.PlaySFX(InGameSFX.POPUP_BTN);
        RestartStage();
    }

    public void OnQuitBtnClicked()
    {
        SoundManager.Instance.PlaySFX(InGameSFX.POPUP_BTN);
        ReturnToMain();
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
