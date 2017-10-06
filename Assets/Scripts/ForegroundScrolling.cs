using UnityEngine;

public class ForegroundScrolling : MonoBehaviour {

    Transform _transform;

    public void Awake()
    {
        _transform = transform;
    }

    public void Scroll(Vector3 targetPosition, float movement)
    {
        _transform.position = Vector3.MoveTowards(_transform.position, targetPosition, movement); 
    }
}
