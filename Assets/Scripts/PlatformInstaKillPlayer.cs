using UnityEngine;
using System.Collections;

public class PlatformInstaKillPlayer : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();

        if (player == null || player.IsDead)
            return;

        LevelManager.Instance.KillPlayer();
    }
}
