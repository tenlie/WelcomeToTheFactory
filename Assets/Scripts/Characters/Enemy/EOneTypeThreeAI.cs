using UnityEngine;
using System.Collections;

public class EOneTypeThreeAI : EnemyAI
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

    public override void Init()
    {
        base.Init();
    }

    public override void Move()
    {
        StartCoroutine(DashCo());
    }

    IEnumerator DashCo()
    {
        yield return new WaitForSeconds(0.5f);
        iTween.MoveBy(transform.parent.gameObject, iTween.Hash("x", -12.8f, "easeType", iTween.EaseType.linear, "time", 2f));
    }
}
