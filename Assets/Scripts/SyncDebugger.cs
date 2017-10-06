using UnityEngine;
using System;

public class SyncDebugger : MonoBehaviour {

    public static SyncDebugger Instance {get; set;}
    public UILabel lbl_fps;
    public UILabel lbl_songTime;
    public UILabel lbl_runningTime;
    public UILabel lbl_songTimePos1;
    public UILabel lbl_songTimePos2;
    public UILabel lbl_songTimePos3;
    public UILabel lbl_runTimePos;
    public UILabel lbl_debugLog;

    private int _fps;

    void Start()
    {
        Instance = this;
    }

    void Update ()
    {
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            lbl_songTime.text = "PlayTime : " + Math.Round((ANAMusic.getCurrentPosition(LevelManager.Instance.BgmID) / 1000.0f), 3).ToString();
            lbl_songTimePos1.text = "PlayTime : " + (ANAMusic.getCurrentPosition(LevelManager.Instance.BgmID) / 1000.0f).ToString();
            //    lbl_songTimePos.text = "SongPos: " + (float)Math.Round(-((ANAMusic.getCurrentPosition(MusicID) / 1000) * 12.8f - 12.8f), 3);
            //lbl_songTimePos1.text = "BgmAudio.time : " + (float)Math.Round(-((ANAMusic.getCurrentPosition(LevelManager.Instance.BgmID) / 1000.0f) * 12.8f - 12.8f), 3);

            // lbl_songTimePos1.text = "BgmAudio.time : " + (float)Math.Round(-((ANAMusic.getCurrentPosition(LevelManager.Instance.BgmID) / 1000.0f) * 12.8f - 12.8f), 3);
            //    lbl_songTimePos2.text = "SongPos2: " + (float)(-(ANAMusic.getCurrentPosition(LevelManager.Instance.BgmID)) * 12.8f / 1000.0f - 12.8f);
            //    lbl_songTimePos3.text = "SongPos3: " + (float)Math.Round(-((ANAMusic.getCurrentPosition(LevelManager.Instance.BgmID)) * 12.8f / 1000.0f - 12.8f), 3);
            lbl_debugLog.text = "BgmID : " + LevelManager.Instance.BgmID;
        }
        else
        */
        {
            lbl_songTime.text = "PlayTime : " + Math.Round(LevelManager.Instance.BgmAudio.time, 3).ToString();

            lbl_songTimePos1.text = "PlayTime : " + LevelManager.Instance.BgmAudio.time;

            //lbl_songTimePos1.text = "SongPos: " + (float)Math.Round(-(LevelManager.Instance.BgmAudio.time * 12.8f - 12.8f), 3);
        }

        //lbl_runningTime.text = "RunTime: " + LevelManager.Instance.RunningTime.TotalMilliseconds.ToString();
        //lbl_runTimePos.text = "RunPos: " + LevelManager.Instance.Foreground.position.x;
        _fps = (int)(1f / Time.smoothDeltaTime);
        lbl_fps.text = "FPS : " + _fps;
        
        /*
        _songTime += (float)(DateTime.UtcNow - _previousFrameTime).TotalMilliseconds;
        _previousFrameTime = DateTime.UtcNow;
        if (_songPosition != _lastReportedPlayheadPosition)
        {
            _songTime = (_songTime + _songPosition) / 2;
            _lastReportedPlayheadPosition = _songPosition;
        }
        */
    }
}
