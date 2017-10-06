using UnityEngine;
using System.Collections;

public class ScrollingObject : MonoBehaviour {

    public float ScrollSpeed;
    private Rigidbody2D _rb2D;

	void Start ()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _rb2D.velocity = new Vector2(ScrollSpeed, 0);
	}
	
	void Update ()
    {
	    if(LevelManager.Instance.Player.IsDead)
        {
            _rb2D.velocity = Vector2.zero;
        }
	}
}
