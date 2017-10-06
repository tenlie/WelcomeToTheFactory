using UnityEngine;

public class AttackTrigger : MonoBehaviour {

    public EnemyAI enemyAI { get; private set; }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemyAI = col.GetComponent<EnemyAI>();
            if (enemyAI == null)
                return;

            enemyAI.Kill();
        }
    }
}
