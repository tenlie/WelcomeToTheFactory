using UnityEngine;
using System.Collections;

public class MissNote : MonoBehaviour
{
    public string InputType;
    public bool isRealNote { get; set; }
    public PlayerController2D playerController;

    void Start()
    {
        isRealNote = false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (col.CompareTag("UNLOAD"))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void PlayNote(string type)
    {
        Debug.Log("MissNote >>> PlayNote");
        if (InputType.Equals(type))
        {
            //판정
            Evaluate();
        }
    }

    public void Evaluate()
    {
        Debug.Log("MissNote >>> Evaluate");

        int resultIdx = 3;
        int points = 0;

        LevelManager.Instance.HandleEvaluationResult(resultIdx, points);
    }
}
