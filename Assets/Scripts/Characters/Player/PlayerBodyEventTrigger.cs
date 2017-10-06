using UnityEngine;
using System;

public class PlayerBodyEventTrigger : MonoBehaviour
{
    public Transform AttackPoint;
    public LayerMask PlatformMask;
    private PlayerController2D _controller;

    void Start() {
        _controller = GetComponentInParent<PlayerController2D>();
    }

    public void InitInputString()
    {
        //Debug.Log("PlayerBodyEventTrigger >>> InitializeInputTypeString()");
        _controller.InputType = "";
    }

    public void DisableAttackCollider()
    {
        _controller.AttackCol.enabled = false;
    }

    public void CheckForSavedJumpInput()
    {
        //Debug.Log("PlayerBodyEventTrigger >>> CheckForSavedJumpInput()");
        if (_controller.JumpInputSaved)
        {
            TimeSpan timeDifference = (DateTime.UtcNow - _controller.JumpInputSavedTime);

            //Debug.Log("timeDifference.Milliseconds:" + timeDifference.Milliseconds);
            if (timeDifference.Milliseconds < 100)
            {
                _controller.Jump();
            }
            _controller.JumpInputSaved = false;
        }
    }
}
