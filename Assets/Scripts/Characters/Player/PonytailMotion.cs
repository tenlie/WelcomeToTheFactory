using UnityEngine;

public class PonytailMotion : MonoBehaviour
{
    public PlayerController2D _controller;
    public int rotationSpeed;

    void Update()
    {
        //Debug.Log("player's _velocity.y: " + _controller._velocity.y + " isGrounded: " + _controller.State.IsGrounded);

        if (_controller.State.IsGrounded != true)
        {
            //Debug.Log("transform.localRotation.z: " + transform.localRotation.z);

            // 상승
            if (_controller._velocity.y > 0)
            {
                //머리가 아래로 휨
                if (_controller._velocity.y > 5 && (transform.localRotation.z < 0.30f))
                {
                    transform.Rotate(new Vector3(0, 0, _controller._velocity.y * Time.deltaTime * 35), Space.Self);
                }
                //머리가 수평으로 원상복귀
                else
                {
                    //서서히 수평으로 복귀 
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
                }
            }
            // 하강
            else
            {
                //머리가 위로 휨
                if (_controller._velocity.y > -9)
                {
                    transform.Rotate(new Vector3(0, 0, _controller._velocity.y * Time.deltaTime * 35), Space.Self);
                }
                //머리가 수평으로 원상복귀
                else
                {
                    //서서히 수평으로 복귀
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
                }
            }
        }
        else
        {
            if (transform.rotation.z != 0)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
