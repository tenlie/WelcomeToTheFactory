using UnityEngine;
using System.Collections;

public class PathedProjectileSpawner : MonoBehaviour {

    public Transform Destination;
    public PathedProjectile Projectile;
    //public GameObject SpawnEffect;
    
    public float Speed;

    public Animator Animator;

    public bool HasFired;

    public void Awake() {
        HasFired = false;
    }

    public void FireProjectile() {
        if (HasFired)
            return;

        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(Destination, Speed);

        //if (SpawnEffect != null)
        //    Instantiate(SpawnEffect, transform.position, transform.rotation);

        if (Animator != null)
            Animator.SetTrigger("Fire");
    }

    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
