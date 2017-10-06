using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Player _player;
    private Vector2 touchPos;

    void OnPress(bool isDown)
    {
        if (isDown)
        {
            touchPos = UICamera.currentTouch.pos;
            _player.HandleTouchInput(touchPos);
        }
    }
}
