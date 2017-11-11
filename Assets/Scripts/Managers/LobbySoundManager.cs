using UnityEngine;
using System.Collections;

public enum LobbySFX
{
    COMMON_BTN,
    POPUP_BTN,
    STAGE_PLAY_BTN,
    STAGE_SWIPE,
    STAGE_START
}

public class LobbySoundManager : MonoBehaviour
{
    // Singleton
    public static LobbySoundManager Instance { get; private set; }
    
    /// <summary>
    /// BGM Preview
    /// </summary>
    public AudioSource BGMAudioSource;

    public AudioClip Stage00;
    public AudioClip Stage01;
    public AudioClip Stage02;
    public AudioClip Stage03;
    public AudioClip Stage04;
    public AudioClip Stage05;

    /// <summary>
    /// StandAlone SFX
    /// </summary>
    public AudioSource SFXAudioSource;

    public AudioClip CommonBtn;
    public AudioClip PopupBtn;
    public AudioClip StagePlayBtn;
    public AudioClip StageSwipe;

    /// <summary>
    /// Android SFX
    /// </summary>
    public string AOSCommonBtnFile;
    public string AOSPopupBtnFile;
    public string AOSStagePlayBtnFile;
    public string AOSStageSwipeFile;

    public float AOSVolume;

    private int _AOSCommonBtnID;
    private int _AOSPopupBtnID;
    private int _AOSStagePlayBtnID;
    private int _AOSStageSwipeID;


    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        BGMAudioSource.clip = Stage00;

        if (Application.platform == RuntimePlatform.Android)
        {
            LoadAOSAudio();
        }
    }

    public void LoadAOSAudio()
    {
        AndroidNativeAudio.makePool();

        AOSCommonBtnFile = "Android Native Audio/" + AOSCommonBtnFile.Trim();
        _AOSCommonBtnID = AndroidNativeAudio.load(AOSCommonBtnFile);

        AOSPopupBtnFile = "Android Native Audio/" + AOSPopupBtnFile.Trim();
        _AOSPopupBtnID = AndroidNativeAudio.load(AOSPopupBtnFile);

        AOSStagePlayBtnFile = "Android Native Audio/" + AOSStagePlayBtnFile.Trim();
        _AOSStagePlayBtnID = AndroidNativeAudio.load(AOSStagePlayBtnFile);

        AOSStageSwipeFile = "Android Native Audio/" + AOSStageSwipeFile.Trim();
        _AOSStageSwipeID = AndroidNativeAudio.load(AOSStageSwipeFile);
    }

    public void PlayBGM()
    {
        BGMAudioSource.Play();
    }

    public void PauseBGM()
    {
        BGMAudioSource.Pause();
    }

    public void ResumeBGM()
    {
        BGMAudioSource.Play();
    }

    public void StopBGM()
    {
        BGMAudioSource.Stop();
    }

    public void MuteBGM()
    {
        BGMAudioSource.volume = 0.0f;
    }

    public void UnmuteBGM()
    {
        BGMAudioSource.volume = 1.0f;
    }

    public void ChangeBGM(int stageIdx)
    {
        switch (stageIdx)
        {
            case 0:
                BGMAudioSource.clip = Stage00;
                break;
            case 1:
                BGMAudioSource.clip = Stage01;
                break;
            case 2:
                BGMAudioSource.clip = Stage02;
                break;
            case 3:
                BGMAudioSource.clip = Stage03;
                break;
            case 4:
                BGMAudioSource.clip = Stage04;
                break;
            case 5:
                BGMAudioSource.clip = Stage05;
                break;
            default:
                break;
        }
    }

    public void PlaySFX(LobbySFX lobbySFX)
    {
        switch (lobbySFX)
        {
            case LobbySFX.COMMON_BTN:
                if (Application.platform == RuntimePlatform.Android) AndroidNativeAudio.play(_AOSCommonBtnID, AOSVolume, AOSVolume); else SFXAudioSource.PlayOneShot(CommonBtn, 1f);
                break;
            case LobbySFX.POPUP_BTN:
                if (Application.platform == RuntimePlatform.Android) AndroidNativeAudio.play(_AOSPopupBtnID, AOSVolume, AOSVolume); else SFXAudioSource.PlayOneShot(PopupBtn, 1f);
                break;
            case LobbySFX.STAGE_PLAY_BTN:
                if (Application.platform == RuntimePlatform.Android) AndroidNativeAudio.play(_AOSStagePlayBtnID, AOSVolume, AOSVolume); else SFXAudioSource.PlayOneShot(StagePlayBtn, 1f);
                break;
            case LobbySFX.STAGE_SWIPE:
                if (Application.platform == RuntimePlatform.Android) AndroidNativeAudio.play(_AOSStageSwipeID, AOSVolume, AOSVolume); else SFXAudioSource.PlayOneShot(StageSwipe, 1f);
                break;
            default:
                break;
        }
    }
}
