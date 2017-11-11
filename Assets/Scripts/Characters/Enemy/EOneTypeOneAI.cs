using UnityEngine;
using System.Collections;

public class EOneTypeOneAI : EnemyAI
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void KillSelf()
    {
        base.KillSelf();
        gameObject.SetActive(false);
    }

    public override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
    }

    public override void OnTriggerExit2D(Collider2D col)
    {
        base.OnTriggerExit2D(col);
    }
}
