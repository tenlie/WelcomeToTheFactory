using UnityEngine;
using System.Collections;

public class EOneTypeTwoAI : EnemyAI
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
        StartCoroutine(FallCo());
    }

    IEnumerator FallCo()
    {
        yield return new WaitForSeconds(2f);
        iTween.MoveBy(transform.parent.gameObject, iTween.Hash("y", -6.25, "easeType", iTween.EaseType.linear, "time", 1f));
    }
}
