using UnityEngine;
using System.Collections;

public class InstaKillBarrel : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();

        if (player == null)
            return;

        LevelManager.Instance.KillPlayer();
    }
}
