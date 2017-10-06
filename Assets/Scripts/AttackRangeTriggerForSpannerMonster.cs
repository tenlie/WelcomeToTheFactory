using UnityEngine;

public class AttackRangeTriggerForSpannerMonster : MonoBehaviour {

    public SpannerEnemyAI spannerEnemyAI;
    public EnemyController2D enemyController2D;

    public void Awake()
    {
        spannerEnemyAI = GetComponentInParent<SpannerEnemyAI>();
        enemyController2D = GetComponentInParent<EnemyController2D>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && (col.GetComponent<Player>().IsDead == false))
        {
            spannerEnemyAI.Attack();
            LevelManager.Instance.KillPlayer();
            enemyController2D.SetHorizontalForce(0);
        }
    }
}
