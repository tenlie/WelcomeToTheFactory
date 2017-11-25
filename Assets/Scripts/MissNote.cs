using UnityEngine;
using System.Collections;

public class MissNote : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("UNLOAD"))
        {
            gameObject.SetActive(false);
        }
    }

    public void PlayNote(string type)
    {
        Debug.Log("MissNote >>> PlayNote");
        Evaluate();
    }

    public void Evaluate()
    {
        Debug.Log("MissNote >>> Evaluate");

        int resultIdx = 3;
        int points = 0;

        LevelManager.Instance.HandleEvaluationResult(resultIdx, points);
    }
}
