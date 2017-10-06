using UnityEngine;
using System.Collections;

public class EffectController2D : MonoBehaviour
{
    public Vector2 Increment;
    public float Duration;
    public float SpeedAcceleration;
    private Transform _transform;
    private Vector2 _targetPosition;

    public void Start()
    {
        _transform = transform;
        _targetPosition = new Vector2(transform.position.x + Increment.x, transform.position.y + Increment.y);
        Destroy();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, SpeedAcceleration * Time.deltaTime);
    }

    public virtual void Destroy()
    {
        Destroy(gameObject, Duration);
    }
}
