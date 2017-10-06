using UnityEngine;
using System.Collections;

public class InstaKillEnemy : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D col)
    {
        var enemyAI = col.GetComponent<EnemyAI>();

        if (enemyAI == null)
            return;

        enemyAI.Kill();
    }
}
