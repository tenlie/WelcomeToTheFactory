using UnityEngine;

public class SpannerEnemyAI : MonoBehaviour, IPlayerRespawnListener
{
    private EnemyController2D _controller;
    private Transform _transform;
    private MissileAfterImageEffect _missileAfterImageEffect;
    private float _hp;

    public float attackInterval;
    public float missileSpeed;
    public float missileTimer;

    public GameObject missile;
    public Transform target;
    public Animator Animator;

    public Transform missileSpawn;

    public GameObject DeathEffect;
    public GameObject DamageEffect;
    public bool IsDead { get; private set; }
    public bool HasEnteredCamera { get; private set; }
    public Collider2D _collider { get; private set; }
    public Collider2D SpannerBodyCol;

    public void Awake()
    {
        _controller = GetComponent<EnemyController2D>();
        _transform = GetComponent<Transform>();
        _missileAfterImageEffect = GetComponent<MissileAfterImageEffect>();

        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;

        missileTimer = 0.2f;
        _hp = 100f;
        IsDead = false;
        HasEnteredCamera = false;
        missile.SetActive(false);

        Animator = GetComponent<Animator>();
        Animator.SetBool("IsDead", IsDead);
        Animator.SetBool("IsAttacking", false);
        Animator.enabled = false;
    }

    public void Attack()
    {
        missile.SetActive(true);
        _missileAfterImageEffect.afterImageEnabled = true;
        _missileAfterImageEffect.StartCoroutine("AfterImageUpdate");
        Animator.SetBool("IsAttacking", true);
    }

    public void KillSelf()
    {
        if (IsDead)
        {
            return;
        }

        Instantiate(DeathEffect, new Vector2(_transform.position.x + 0.5f, _transform.position.y + 0.5f), Quaternion.identity);
        IsDead = true;
        Animator.SetBool("IsDead", IsDead);
        SpannerBodyCol.enabled = false;
        _controller.SetVerticalForce(9);
        _controller.SetHorizontalForce(0);
        _missileAfterImageEffect.StopCoroutine("AfterImageUpdate");
        missile.SetActive(false);
    }

    public void Damage(Vector3 hitPos, float damage)
    {
        Instantiate(DamageEffect, hitPos, Quaternion.identity);
        _hp -= damage;
        if (_hp<=0)
        {
            KillSelf();
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("LOAD") && !HasEnteredCamera)
        {
            HasEnteredCamera = true;
            Animator.enabled = true;
            _controller.Parameters.Gravity = -25f;
            //_controller.SetHorizontalForce(-1);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("UNLOAD") && HasEnteredCamera)
        {
            gameObject.SetActive(false);
        }
    }

}
