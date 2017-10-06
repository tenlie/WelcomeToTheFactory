using UnityEngine;
using System.Collections;

public class CFollowCtrl : MonoBehaviour
{
    public Transform Target;
    protected Transform _transform;

	public virtual void Start ()
    {
        _transform = GetComponent<Transform>();
	}
	
	public virtual void Update ()
    {
        Follow();	
	}

    public virtual void Follow()
    {
        _transform.position = Target.position;
    }
}
