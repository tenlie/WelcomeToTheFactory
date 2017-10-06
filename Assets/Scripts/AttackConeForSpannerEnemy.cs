using UnityEngine;

public class AttackConeForSpannerEnemy : MonoBehaviour {

    public SpannerEnemyAI spannerEnemyAI;

    public void Awake()
    {
        spannerEnemyAI = GetComponentInParent<SpannerEnemyAI>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && (col.GetComponent<Player>().IsDead == false))
        {
            spannerEnemyAI.Attack();
        }
    }
}
