using UnityEngine;

public class InstaKill : MonoBehaviour {

    public void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("InstaKill1");

        if (col.CompareTag("Player"))
        {
            Debug.Log("InstaKill2");
            LevelManager.Instance.KillPlayer();
            Destroy(gameObject);
        }

        //if (player == null)
        //    return;
    }
}
