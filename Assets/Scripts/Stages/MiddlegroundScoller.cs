using UnityEngine;
using System.Collections;

public class MiddlegroundScoller : MonoBehaviour
{
    private Material _material;
    private float _offset;
    private float _speedIncr;
    private float _speedDest;

    public Player player;
    public AudioSource stageMusic;
    public float speed;

    public void Start()
    {
        _material = GetComponent<Renderer>().material;
        _offset = 0f;
        _speedIncr = speed / 2.0f / 60;
        _speedDest = speed * 1.5f;
    }

    public void LateUpdate()
    {
        if (player.IsDead == true)
            return;

        //Debug.Log(_offset);
        //_offset = (_material.mainTextureOffset.x + (speed * TimeMgr.Instance.DeltaTime/12.8f)) % 1f;
        _offset = (_material.mainTextureOffset.x + (speed * Time.smoothDeltaTime / 12.8f)) % 1f;
        _material.mainTextureOffset = new Vector2(_offset, 0);

        //?
        if (speed > _speedDest)
        {
            speed = _speedDest;
        }
        else if(speed < _speedDest)
        {
            //speed += _speedIncr * TimeMgr.Instance.DeltaTime;
            speed += _speedIncr * Time.smoothDeltaTime;
        }
    }
}
