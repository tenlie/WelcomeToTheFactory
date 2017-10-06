using UnityEngine;

public class PlayerInstaKillEnemy : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) {
            //Debug.Log("PlayerInstaKillEnemy");
            var spannerEnemyAI = col.GetComponent<SpannerEnemyAI>();

            if (spannerEnemyAI == null)
                return;

            spannerEnemyAI.KillSelf();
        }
    }
}
