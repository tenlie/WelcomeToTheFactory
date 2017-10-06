using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Rebuild_LevelManager : MonoBehaviour {

    public static Rebuild_LevelManager instance { get; private set; }
    public int frameRate;
    public bool isPaused { get; set; }

    public AudioSource stageMusic;

    //Each piece of music needs a music ID
    public string bgmFileName;
    public int musicID { get; set; }
    private bool isLoaded;
    private bool isGameStarted;

    void Awake()
    {
        Debug.Log(this.name + " >>> Awake()");

        GC.Collect();
        instance = this;
        isPaused = false;
        isGameStarted = false;

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidNativeAudio.makePool();
            // Load the native music
            bgmFileName = "Android Native Audio/" + bgmFileName.Trim();
            Load();
        }
    }

    void Start()
    {
        Debug.Log(this.name + " >>> Start()");

        if (Application.platform == RuntimePlatform.Android)
        {
            ANAMusic.play(musicID);
        }
        else
        {
            stageMusic.Play();
        }

        //prevents background from stuttering
        QualitySettings.vSyncCount = 0; 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isGameStarted = true;
    }

    void OnApplicationPause(bool pause)
    {
        Debug.Log(this.name + " >>> OnApplicationPause() : " + pause);

        if (pause)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void OnApplicationQuit()
    {
        // Clean up when done
        Stop();
        Release();
    }

    void PauseGame()
    {
        Debug.Log(this.name + " >>> PauseGame()");
        isPaused = true;
        Time.timeScale = 0;

        if (Application.platform == RuntimePlatform.Android)
            ANAMusic.pause(musicID);
        else
            stageMusic.Pause();

        //TODO : Show Pause Screen

    }

    void ResumeGame()
    {
        Debug.Log(this.name + " >>> ResumeGame()");

        if (!isGameStarted)
            return;

        isPaused = false;
        Time.timeScale = 1;

        if (Application.platform == RuntimePlatform.Android)
            ANAMusic.play(musicID);
        else
            stageMusic.Play();
    }

    void Load()
    {
        // Load the music with callback
        musicID = ANAMusic.load(bgmFileName, false, true, Loaded);
        //Player.musicID = musicID;
        ANAMusic.setPlayInBackground(musicID, false);
    }

    void Loaded(int musicID)
    {
        isLoaded = true;
    }

    void Stop()
    {
        // To "stop", pause and seek to beginning
        ANAMusic.pause(musicID);
        ANAMusic.seekTo(musicID, 0);
    }

    void Release()
    {
        // Release music resources
        ANAMusic.release(musicID);
        isLoaded = false;
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }

    private IEnumerator KillPlayerCo()
    {
        //Player.Kill();

        if (Application.platform == RuntimePlatform.Android)
        {
            Stop();
            Release();
        }
        else
        {
            stageMusic.Stop();
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
