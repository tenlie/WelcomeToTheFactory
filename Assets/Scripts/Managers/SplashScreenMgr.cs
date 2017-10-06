using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SplashScreenMgr : MonoBehaviour
{
    public GameObject pn_Popup_Quit;
    public GameObject studioLogo;
    public GameObject teamLogo;
    public GameObject titleBG;
    public GameObject titleName;
    public GameObject progressBar;
    public GameObject touchToStart;
    public GameObject copyrights;
    private UIProgressBar progressBarFg;

    private AsyncOperation _asyncOperation;
    private bool _isPaused;
    private bool _isLoadingFinished;
    private bool _isTabbed;
    private bool _isGameStarted;

    void Awake()
    {
        //Garbage Collect
        GC.Collect();
        //Hardware Settings
        Screen.autorotateToPortrait = false;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        //Connect to GPGS
        GPGSMgr.Instance.Initialize();
        GPGSMgr.Instance.SignInToGooglePlay();

        progressBarFg = progressBar.GetComponent<UIProgressBar>();
        pn_Popup_Quit.transform.position = new Vector3(0, 0, 0);

        NGUITools.SetActive(pn_Popup_Quit, false);
        NGUITools.SetActive(titleBG, false);
        NGUITools.SetActive(titleName, false);
        NGUITools.SetActive(progressBar, false);
        NGUITools.SetActive(touchToStart, false);

        _isGameStarted = false;
        _isPaused = false;
        _isLoadingFinished = false;
        _isTabbed = false;
    }

    void Start()
    {
        //Invoke("LoadMain", 2.0f);
        StartCoroutine(LoadGameLobbyCo());
        _isGameStarted = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pn_Popup_Quit.activeInHierarchy) NGUITools.SetActive(pn_Popup_Quit, true);
        }
    }

    void OnPress(bool isDown)
    {
        if (_isLoadingFinished) _isTabbed = true;
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) PauseGame(); else ResumeGame();
    }

    void LoadMain()
    {
        NGUITools.SetActive(studioLogo, false);
        NGUITools.SetActive(titleBG, true);
        NGUITools.SetActive(titleName, true);

        StartCoroutine("LoadMainCo");
    }

    IEnumerator LoadGameLobbyCo()
    {
        yield return new WaitForSeconds(2.0f);
        NGUITools.SetActive(studioLogo, false);

        yield return new WaitForSeconds(2.0f);
        NGUITools.SetActive(teamLogo, false);

        NGUITools.SetActive(titleBG, true);
        NGUITools.SetActive(titleName, true);
        NGUITools.SetActive(progressBar, true);
        _asyncOperation = SceneManager.LoadSceneAsync(1);
        _asyncOperation.allowSceneActivation = false;

        while (!_asyncOperation.isDone)
        {
            progressBarFg.value = _asyncOperation.progress;

            while (_asyncOperation.progress == 0.9f)
            {
                _isLoadingFinished = true;
                yield return new WaitForSeconds(0.5f);
                NGUITools.SetActive(progressBar, false);

                yield return new WaitForSeconds(0.5f);
                NGUITools.SetActive(touchToStart, true);

                if (_isTabbed && !pn_Popup_Quit.activeInHierarchy)
                {
                    TweenAlpha.Begin(titleName, 0.5f, 0);
                    TweenAlpha.Begin(touchToStart, 0.5f, 0);
                    TweenAlpha.Begin(copyrights, 0.5f, 0);
                    yield return new WaitForSeconds(0.5f);
                    _asyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

    void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        if (!_isGameStarted)
            return;

        _isPaused = false;
        Time.timeScale = 1;
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void CancelQuitGame()
    {
        NGUITools.SetActive(pn_Popup_Quit, false);
    }
}
