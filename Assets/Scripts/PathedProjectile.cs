using UnityEngine;
using System.Collections;

public class PathedProjectile : MonoBehaviour {

    public Transform _destination;
    public float _speed;

    public GameObject DestroyEffect;

    public void Initialize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);

        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;

        if (distanceSquared > 0.01f * 0.01f)
            return;

        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
