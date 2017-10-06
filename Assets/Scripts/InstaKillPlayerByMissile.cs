using UnityEngine;

public class InstaKillPlayerByMissile : MonoBehaviour
{
    public EnemyController2D enemyController2D;

    public void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (player == null)
            return;

        if (gameObject.CompareTag("Missile"))
        {
            //LevelManager.Instance.KillPlayer();
            //enemyController2D.SetHorizontalForce(0);

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
    } 
}
