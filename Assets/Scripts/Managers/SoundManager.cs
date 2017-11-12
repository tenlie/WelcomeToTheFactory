using UnityEngine;
using System.Collections;
using System;

public enum InGameSFX
{
    COMMON_BTN,
    POPUP_BTN
}

public class SoundManager : MonoBehaviour
{
    //Basic Singleton
    public static SoundManager Instance { get; private set; }

    #region StandAlone BGM
    public AudioSource BgmAudioSource;
    public float StandAloneVolume;
    #endregion

    #region StandAlone SFX
    public AudioSource SFXAudioSource;
    public AudioClip CommonBtn;
    public AudioClip PopupBtn;
    #endregion

    #region AOS BGM
    public int AOSBgmID { get; private set; }
    private bool _isAOSBgmLoaded;
    public string AOSBgmFile;
    public float VolumeForAndroid;
    #endregion

    #region AOS SFX
    public float AOSSFXVolume;
    public string AOSCommonBtnFile;
    public string AOSPopupBtnFile;
    private int _AOSCommonBtnID;
    private int _AOSPopupBtnID;
    #endregion

    //Note for StandAlone
    public AudioSource[] NoteAudio;
    private int NoteAudioIdx;

    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }
    private DateTime _started;
    private DateTime _previousFrameTime;
    private float _lastReportedPlayheadPosition;
    private float _songTime;
    public Transform Foreground;

    void Awake()
    {
        Instance = this;
        NoteAudioIdx = 0;

        if (Application.platform == RuntimePlatform.Android)
        {
            LoadAOSAudio();
        }
    }

    public void LoadAOSAudio()
    {
        AndroidNativeAudio.makePool();

        AOSBgmFile = "Android Native Audio/" + AOSBgmFile.Trim();
        AOSBgmID = ANAMusic.load(AOSBgmFile, false, true, BgmLoaded);
        ANAMusic.setPlayInBackground(AOSBgmID, false);

        AOSCommonBtnFile = "Android Native Audio/" + AOSCommonBtnFile.Trim();
        _AOSCommonBtnID = AndroidNativeAudio.load(AOSCommonBtnFile);

        AOSPopupBtnFile = "Android Native Audio/" + AOSPopupBtnFile.Trim();
        _AOSPopupBtnID = AndroidNativeAudio.load(AOSPopupBtnFile);
    }

    public void BgmLoaded(int musicID)
    {
        _isAOSBgmLoaded = true;
    }

    public void PlayBgm()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.play(AOSBgmID);
        }
        else
        {
            BgmAudioSource.Play();
        }
    }

    public void PauseBgm()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.pause(AOSBgmID);
        }
        else
        {
            BgmAudioSource.Pause();
        }
    }

    public void StopBgm()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.pause(AOSBgmID);
            ANAMusic.seekTo(AOSBgmID, 0);
        }
        else
        {
            BgmAudioSource.Stop();
        }
    }

    public void ReleaseBgm()
    {
        Debug.Log("ReleaseBgm");
        ANAMusic.release(AOSBgmID);
        _isAOSBgmLoaded = false;
    }

    public void SetSync()
    {
        StartCoroutine(SetSyncCo());
    }

    IEnumerator SetSyncCo()
    {
        _started = DateTime.UtcNow;
        _previousFrameTime = DateTime.UtcNow;
        _lastReportedPlayheadPosition = 0.0f;
        _songTime = 0.0f;
        yield return new WaitForSeconds(2.0f);
        if (Application.platform == RuntimePlatform.Android)
        {
            Foreground.position = new Vector3((float)Math.Round(-((ANAMusic.getCurrentPosition(AOSBgmID) / 1000.0f) * 12.8f - 12.8f - 3.0f), 3), Foreground.position.y, Foreground.position.z);
        }
        else
        {
            Foreground.position = new Vector3(((float)Math.Round(-(Math.Round(BgmAudioSource.time, 3) * 12.8f - 12.8f - 3.0f), 3)), Foreground.position.y, Foreground.position.z);
        }
    }

    public void PlayNote(AudioClip clip)
    {
        NoteAudio[NoteAudioIdx].clip = clip;
        NoteAudio[NoteAudioIdx].PlayOneShot(clip, 1f);

        if (NoteAudioIdx < NoteAudio.Length - 1)
        {
            NoteAudioIdx++;
        }
        else
        {
            NoteAudioIdx = 0;
        }
    }

    public void PlaySFX(InGameSFX inGameSFX)
    {
        switch (inGameSFX)
        {
            case InGameSFX.COMMON_BTN:
                if (Application.platform == RuntimePlatform.Android) AndroidNativeAudio.play(_AOSCommonBtnID, AOSSFXVolume, AOSSFXVolume); else SFXAudioSource.PlayOneShot(CommonBtn, 1f);
                break;
            case InGameSFX.POPUP_BTN:
                if (Application.platform == RuntimePlatform.Android) AndroidNativeAudio.play(_AOSPopupBtnID, AOSSFXVolume, AOSSFXVolume); else SFXAudioSource.PlayOneShot(PopupBtn, 1f);
                break;
            default:
                break;
        }
    }

    void OnApplicationQuit()
    {
        //ReleaseBgm();
    }
}
