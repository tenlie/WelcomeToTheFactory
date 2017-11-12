using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;
    private Collider2D _collider;

    public bool IsDead { get; private set; }
    public bool HasEnteredCamera { get; private set; }

    public Transform ParentAnchor;
    public GameObject DeathEffect;
    public GameObject DamageEffect;

    public virtual void Awake()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();    
        _animator = GetComponent<Animator>();
        _animator.SetBool("Attack", false);

        IsDead = false;
    }

    public virtual void Attack()
    {

    }

    public virtual void KillSelf()
    {
        if (IsDead)
        {
            return;
        }

        GameObject deathEffect = Instantiate(DeathEffect, new Vector3(_transform.position.x, _transform.position.y + 1f, _transform.position.z), Quaternion.identity) as GameObject;
        deathEffect.transform.parent = ParentAnchor;

        IsDead = true;
        _collider.enabled = false;
    }

    public virtual void TakeDamage(Vector3 hitPos)
    {
        Instantiate(DamageEffect, hitPos, Quaternion.identity);
        KillSelf();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("LOAD") && !HasEnteredCamera)
        {
            HasEnteredCamera = true;
            _collider.enabled = true;
        }

        //Block Hits For Tests
        //return;

        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<Player>();
            if (player == null)
            {
                return;
            }

            if (!player.IsDead)
            {
                LevelManager.Instance.KillPlayer();
                player._controller.SetForce(new Vector2(7, 17));
                player.DeadJumpCount++;
                Debug.Log("DeadJumpCount : 0");
            }
            else
            {
                if (player.DeadJumpCount == 1)
                {
                    Debug.Log("DeadJumpCount : 1 " + player._controller.Velocity.x);

                    if (player._controller.Velocity.x >= 0)
                    {
                        player._controller.SetHorizontalForce(4);
                    }
                    else
                    {
                        player._controller.SetHorizontalForce(-4);
                    }

                    player._controller.SetVerticalForce(9);
                    player.DeadJumpCount++;
                }
                else if (player.DeadJumpCount == 2)
                {
                    Debug.Log("DeadJumpCount : 2 " + player._controller.Velocity.x);

                    if (player._controller.Velocity.x >= 0)
                    {
                        player._controller.SetHorizontalForce(2);
                    }
                    else
                    {
                        player._controller.SetHorizontalForce(-2);
                    }

                    player._controller.SetVerticalForce(5);
                    player.DeadJumpCount++;
                }
            }
        }
        else if (col.CompareTag("AttackTrigger"))
        {
            TakeDamage(col.transform.position);
        }
        else if (col.CompareTag("Player Bounds") && !HasEnteredCamera)
        {
            HasEnteredCamera = true;
            _collider.enabled = true;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("UNLOAD") && HasEnteredCamera)
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}

/*attack함수
missileTimer += Time.deltaTime;

if (missileTimer >= attackInterval)
{
    Vector2 direction = (target.transform.position - transform.position) + new Vector3(1.5f,0,0);
    direction.Normalize();

    //공격 방향 계산하기
    GameObject missileClone;
    missileClone = Instantiate(missile, missileSpawn.position, missileSpawn.rotation) as GameObject;
    missileClone.GetComponent<Rigidbody2D>().velocity = direction * missileSpeed;
    Animator.SetBool("Attack", true);
    //missileTimer = 0;
}
*/
