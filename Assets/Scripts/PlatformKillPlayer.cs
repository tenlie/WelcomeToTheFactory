using UnityEngine;

public class PlatformKillPlayer : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !col.GetComponent<Player>().IsDead)
        {
            LevelManager.Instance.KillPlayer();
        }
    }
}
