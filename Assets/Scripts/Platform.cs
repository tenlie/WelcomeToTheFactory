using UnityEngine;

public class Platform : MonoBehaviour {

    public bool hasEnteredCamera { get; private set; }
    public Collider2D _collider1;
    public Collider2D _collider2;
    public Collider2D _collider3;

    void Awake()
    {
        hasEnteredCamera = false;
        _collider1.enabled = false;
        _collider2.enabled = false;
        _collider3.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player Bounds") && !hasEnteredCamera)
        {
            hasEnteredCamera = true;
            _collider1.enabled = true;
            _collider2.enabled = true;
            _collider3.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player Bounds"))
        {
            gameObject.SetActive(false);
        }
    }
}
