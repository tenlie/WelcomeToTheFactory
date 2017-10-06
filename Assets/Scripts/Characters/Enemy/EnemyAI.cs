using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    //public float distance;
    //public float wakeRange;
    public float attackInterval;
    public float missileSpeed;
    public float missileTimer;

    public GameObject missile;
    public Transform target;
    public Animator Animator;

    public Transform missileSpawn;

    public bool IsDead { get; private set; }

    public void Awake()
    {
        Animator = GetComponent<Animator>();
        missileTimer = 0.2f;
        IsDead = false;
        Animator.SetBool("Attack", false);
    }

    public void Attack()
    {
        //missileTimer += Time.deltaTime;

        //if (missileTimer >= attackInterval)
        //{
            Vector2 direction = (target.transform.position - transform.position) + new Vector3(1.5f,0,0);
            direction.Normalize();

            //공격 방향 계산하기
            GameObject missileClone;
            missileClone = Instantiate(missile, missileSpawn.position, missileSpawn.rotation) as GameObject;
            missileClone.GetComponent<Rigidbody2D>().velocity = direction * missileSpeed;
            Animator.SetBool("Attack", true);
            //missileTimer = 0;
        //}
    }

    public void Kill() {
        IsDead = true;
    }
}
