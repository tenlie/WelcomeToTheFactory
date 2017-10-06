using UnityEngine;

public class InstaKillPlayer : MonoBehaviour {

    public EnemyController2D enemyController2D;
    public SpannerEnemyAI spannerEnemyAI;
    public Collider2D spannerCollider2D { get; set; }

    public void Awake()
    {
        spannerCollider2D = GetComponent<Collider2D>();
    }

    public void Damage()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<Player>();
            if (player == null)
                return;

            if (!player.IsDead)
            {
                LevelManager.Instance.KillPlayer();
                enemyController2D.SetHorizontalForce(0);
                player._controller.SetForce(new Vector2(7, 13));
                player.DeadJumpCount++;
            }
            else
            {
                if (player.DeadJumpCount == 1)
                {
                    player._controller.SetVerticalForce(7);
                    player.DeadJumpCount++;
                }
                else if (player.DeadJumpCount == 2)
                {
                    player._controller.SetVerticalForce(5);
                    player.DeadJumpCount++;
                }
            }
        }
        else if (col.CompareTag("AttackTrigger"))
        {
            Player player = col.GetComponentInParent<Player>();

            if (player.IsDead)
            {
                return;
            }

            spannerEnemyAI.Damage(col.transform.position, 100f);
        }             
    }
}
