using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class ControllerParameters2D
{
    public enum JumpBehavior
    {
        CanJumpOnGround, // 0
        CanJumpAnywhere, // 1
        CantJump         // 2
    }
    //최대 속도
    public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);
    //최대 경사각
    [Range(0, 90)]
    public float SlopeLimit = 30;
    //중력가속도
    public float Gravity = -25f;
    //점프력
    public float JumpMagnitude = 12;
    //점프 옵션
    public JumpBehavior JumpRestrictions;
    //점프 쿨타임
    public float JumpFrequency = 0.01f;
    //공격 쿨타임
    public float AttackFrequency = 0.01f;
    //감속 속도
    public float SpeedForSlowingDown = 3.0f;
}
