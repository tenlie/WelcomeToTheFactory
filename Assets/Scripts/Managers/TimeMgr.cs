using UnityEngine;
using System.Collections;

public class TimeMgr : MonoBehaviour {

    //싱글톤 패턴. {get; private set;} = readonly
    public static TimeMgr Instance { get; private set; }

    public float DeltaTime { get; set; }
    float _lastFrame;
    float _currentFrame;
    float _myDelta;

	void Awake ()
    {
        Instance = this;
        DeltaTime = 0f;
        _lastFrame = _currentFrame = Time.realtimeSinceStartup;
        _myDelta = Time.smoothDeltaTime;	
	}
	
	void Update ()
    {
        _currentFrame = Time.realtimeSinceStartup;
        _myDelta = _currentFrame - _lastFrame;
        _myDelta *= Time.timeScale;
        _lastFrame = _currentFrame;
	}

    void LateUpdate()
    {
        DeltaTime = (Time.deltaTime + Time.smoothDeltaTime + _myDelta) * 0.33333f;
        //Debug.Log("Deltatime: " + DeltaTime + "\nSmoothDeltaTime: " + Time.smoothDeltaTime);
    }
}
