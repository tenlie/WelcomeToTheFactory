using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectPool : MonoBehaviour
{
    public float CreateTime;
    public int PooledAmount;
    public GameObject[] Prefabs;
    List<GameObject> Effects;

    void Start()
    {
        Effects = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = (GameObject)Instantiate(Prefabs[(i%PooledAmount)]);
            //Debug.Log(obj);
            obj.SetActive(false);
            Effects.Add(obj);
        }
    }

    void OnEnable()
    {
        StartCoroutine(CreateEffectCo());
    }

    void OnDisable()
    {
        StopCoroutine(CreateEffectCo());
    }

    IEnumerator CreateEffectCo()
    {
        for (int i = 0; i < PooledAmount; i++)
        {
            yield return new WaitForSeconds(CreateTime); 
            Effects[i].transform.position = transform.position;
            Effects[i].transform.rotation = transform.rotation;
        }
    }
}
