using UnityEngine;
using System.Collections;

public class DragController : MonoBehaviour {

    private Vector2 _dragAmount;
    public MainInput _mainInput;

    void OnDrag()
    {
        _dragAmount.x = UICamera.currentTouch.totalDelta.x;
        Debug.Log("totalDelta : " + _dragAmount.x);
        if (_dragAmount.x < -50f)
        {
            _mainInput.ScrollRight();
        }
        else if (_dragAmount.x > 50f)
        {
            _mainInput.ScrollLeft();
        }
    }
}
