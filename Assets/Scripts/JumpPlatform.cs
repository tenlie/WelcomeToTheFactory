using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour {

    public float JumpMagnitude = 20f;

    public void ControllerEnter2D(PlayerController2D controller)
    {
        controller.SetVerticalForce(JumpMagnitude);
    }
}
