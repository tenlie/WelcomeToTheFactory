using UnityEngine;
using System.Collections;

public enum LobbySFX
{
    COMMON_BTN,
    PLAY_BTN,
    STAGE_SWIPE,
    STAGE_START
}

public class LobbySoundManager : MonoBehaviour
{
    public static LobbySoundManager Instance { get; private set; }

    public AudioClip Stage00;
    public AudioClip Stage01;
    public AudioClip Stage02;
    public AudioClip Stage03;
    public AudioClip Stage04;
    public AudioClip Stage05;

    public AudioClip CommonBtn;
    public AudioClip PlayBtn;
    public AudioClip StageSwipe;
    public AudioClip StageStart;

    public AudioSource BGMAudioSource;
    public AudioSource SFXAudioSource;

    void Awake()
    {
        Instance = this;
        BGMAudioSource.clip = Stage00;
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
                SFXAudioSource.PlayOneShot(CommonBtn, 1f);
                break;
            case LobbySFX.PLAY_BTN:
                SFXAudioSource.PlayOneShot(PlayBtn, 1f);
                break;
            case LobbySFX.STAGE_SWIPE:
                SFXAudioSource.PlayOneShot(StageSwipe, 1f);
                break;
            case LobbySFX.STAGE_START:
                SFXAudioSource.PlayOneShot(StageStart, 1f);
                break;
            default:
                break;
        }
    }
}
