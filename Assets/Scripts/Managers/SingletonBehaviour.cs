using UnityEngine;
using System.Collections;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance { get; protected set; }

    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            Debug.Log("An instance of this singleton already exists");
        }
        else
        {
            Instance = (T)this;
        }
    }
}
