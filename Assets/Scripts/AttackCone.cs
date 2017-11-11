using UnityEngine;

public class AttackCone : MonoBehaviour {

    public EnemyAI enemyAI;

    public void Awake() {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Debug.Log("여기1");
            enemyAI.Attack();
        }
    }

    public void OnTriggerExit2D(Collider2D col) {
        //if (col.CompareTag("Player")) {
            //enemyAI._Animator.SetBool("Attack", false);
            //Debug.Log("여기4");
        //}
    }
}
