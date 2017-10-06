using UnityEngine;
using System.Collections;

public class FullyChargedCheckpoint : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Player>().IsFullyCharged = true;
        }
    }

}
