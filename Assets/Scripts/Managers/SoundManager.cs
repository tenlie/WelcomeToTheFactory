using UnityEngine;
using System.Collections;
using System;

public enum InGameSFX
{
    COMMOM_BTN
}

public class SoundManager : MonoBehaviour
{
    //싱글톤 패턴
    public static SoundManager Instance { get; private set; }

    //Bgm for Android
    public int BgmID { get; private set; }
    private bool _isBgmLoaded;
    public string BgmFileName;
    public float VolumeForAndroid;
    //Bgm for StandAlone
    public AudioSource BgmAudio;
    public float VolumeForStandAlone;

    //SFX
    public AudioSource SFXAudioSource;
    public AudioClip ComonBtn;

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
    }

    public void LoadBgm()
    {
        AndroidNativeAudio.makePool();
        BgmFileName = "Android Native Audio/" + BgmFileName.Trim();
        BgmID = ANAMusic.load(BgmFileName, false, true, BgmLoaded);
        ANAMusic.setPlayInBackground(BgmID, false);
    }

    public void BgmLoaded(int musicID)
    {
        _isBgmLoaded = true;
    }

    public void PlayBgm()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.play(BgmID);
        }
        else
        {
            BgmAudio.Play();
        }
    }

    public void PauseBgm()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.pause(BgmID);
        }
        else
        {
            BgmAudio.Pause();
        }
    }

    public void StopBgm()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.pause(BgmID);
            ANAMusic.seekTo(BgmID, 0);
        }
        else
        {
            BgmAudio.Stop();
        }
    }

    public void ReleaseBgm()
    {
        Debug.Log("ReleaseBgm");
        ANAMusic.release(BgmID);
        _isBgmLoaded = false;
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
            Foreground.position = new Vector3((float)Math.Round(-((ANAMusic.getCurrentPosition(BgmID) / 1000.0f) * 12.8f - 12.8f - 3.0f), 3), Foreground.position.y, Foreground.position.z);
        }
        else
        {
            Foreground.position = new Vector3(((float)Math.Round(-(Math.Round(BgmAudio.time, 3) * 12.8f - 12.8f - 3.0f), 3)), Foreground.position.y, Foreground.position.z);
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
            case InGameSFX.COMMOM_BTN:
                SFXAudioSource.PlayOneShot(ComonBtn, 1f);
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
