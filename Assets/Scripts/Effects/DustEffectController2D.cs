using UnityEngine;
using System.Collections;

public class DustEffectController2D : EffectController2D
{
    public override void Destroy()
    {
        Invoke("DestroyEffect", Duration);
    }

    void DestroyEffect()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke("DestroyEffect");
    }
}
