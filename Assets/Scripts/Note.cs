﻿using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour
{
    //Sound for Android
    public string AudioFileName;
    private int _fileID;
    private int _soundID;
    //Sound for StandAlone
    public AudioClip _audioClip;
    public AudioSource _audioSource { get; set; }

    private Transform _transform;
    private BoxCollider2D _boxCollider2D;
    public float ScrollSpeed;
    public string InputType;
    private float _notePosX;
    private float _playerPosX;
    public bool isRealNote { get; set; }
    public bool hadInput { get; set; }

    public DifficultyLevel Difficulty;
    public enum DifficultyLevel
    {
        Easy,   //0
        Normal, //1
        Hard    //2
    }

    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _playerPosX = -3.0f;
        _transform = transform;
        _notePosX = _transform.position.x;
        _fileID = 0;
        _soundID = 0;
        hadInput = false;
        isRealNote = true;

        if (Application.platform != RuntimePlatform.Android)
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (col.CompareTag("LOAD"))
            {
                AudioFileName = "Android Native Audio/" + AudioFileName.Trim();
                _fileID = AndroidNativeAudio.load(AudioFileName);
            }
            else if (col.CompareTag("UNLOAD"))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void PlayNote(string type)
    {
        hadInput = true;

        if (InputType.Equals(type))
        {
            //점수 판정
            Evaluate();
        }
    }

    public void Evaluate()
    {
        int resultIdx = 0;
        int points = 0;
        bool isBeforeNote = _transform.position.x >= _playerPosX ? true : false;
        float distance = Mathf.Abs(_transform.position.x - _playerPosX);
        Debug.Log("Distance : " + distance);
        if (isBeforeNote)
        {
            if (distance > ScrollSpeed * 0.4f)
            {
                return;
            }
            else if (distance > ScrollSpeed * 0.2f)
            {
                resultIdx = 3;
                points = 0;
            }
            else if (distance > ScrollSpeed * 0.1f)
            {
                resultIdx = 2;
                points = 100;
            }
            else if (distance > ScrollSpeed * 0.04f)
            {
                resultIdx = 1;
                points = 200;
            }
            else
            {
                resultIdx = 0;
                points = 300;
            }
        }
        else
        {
            if (distance > ScrollSpeed * 0.2f)
            {
                return;
            }
            else if (distance > ScrollSpeed * 0.1f)
            {
                resultIdx = 3;
                points = 0;
            }
            else if (distance > ScrollSpeed * 0.05f)
            {
                resultIdx = 2;
                points = 100;
            }
            else if (distance > ScrollSpeed * 0.02f)
            {
                resultIdx = 1;
                points = 200;
            }
            else
            {
                resultIdx = 0;
                points = 300;
            }
        }

        LevelManager.Instance.HandleEvaluationResult(resultIdx, points);

        if (SaveData.SFXOnOff == "OFF")
            return;

        if (Application.platform == RuntimePlatform.Android)
        {
            _soundID = AndroidNativeAudio.play(_fileID, 0.3f, 0.3f); //Android
        }
        else
        {
            SoundManager.Instance.PlayNote(_audioClip); //StandAlone
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (pause && _soundID != 0)
        {
            AndroidNativeAudio.stop(_soundID);
        }
    }

    void OnDisable()
    {
        if (_fileID != 0)
        {
            AndroidNativeAudio.unload(_fileID);
        }
    }
}
