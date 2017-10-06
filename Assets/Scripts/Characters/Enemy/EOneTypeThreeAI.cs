using UnityEngine;
using System.Collections;

public class EOneTypeThreeAI : MonoBehaviour {

    public Collider2D _collider { get; private set; }
    private bool hasEnteredCamera;
    private bool isRendered;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;
        hasEnteredCamera = false;
        isRendered = false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("LOAD") && !hasEnteredCamera)
        {
            hasEnteredCamera = true;
            _collider.enabled = true;
            iTween.MoveBy(transform.parent.gameObject, iTween.Hash("x", -12.8f, "easeType", iTween.EaseType.linear, "time", 2f));
        }

        return;

        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<Player>();
            if (player == null)
                return;

            if (!player.IsDead)
            {
                LevelManager.Instance.KillPlayer();
                player._controller.SetForce(new Vector2(7, 17));
                player.DeadJumpCount++;
                Debug.Log("DeadJumpCount : 0");
            }
            else
            {
                if (player.DeadJumpCount == 1)
                {
                    Debug.Log("DeadJumpCount : 1 " + player._controller.Velocity.x);

                    if (player._controller.Velocity.x >= 0)
                    {
                        player._controller.SetHorizontalForce(4);
                    }
                    else
                    {
                        player._controller.SetHorizontalForce(-4);
                    }

                    player._controller.SetVerticalForce(9);
                    player.DeadJumpCount++;
                }
                else if (player.DeadJumpCount == 2)
                {
                    Debug.Log("DeadJumpCount : 2 " + player._controller.Velocity.x);

                    if (player._controller.Velocity.x >= 0)
                    {
                        player._controller.SetHorizontalForce(2);
                    }
                    else
                    {
                        player._controller.SetHorizontalForce(-2);
                    }

                    player._controller.SetVerticalForce(5);
                    player.DeadJumpCount++;
                }
            }
        }
        else if (col.CompareTag("Player Bounds") && !hasEnteredCamera)
        {
            hasEnteredCamera = true;
            _collider.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("UNLOAD") && hasEnteredCamera)
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
