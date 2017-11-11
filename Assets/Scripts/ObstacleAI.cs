using UnityEngine;

public class ObstacleAI : MonoBehaviour
{
    //public GameObject parentObject;
    public Collider2D _collider { get; private set; }
    private bool hasEnteredCamera;
    private bool isRendered;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
        hasEnteredCamera = false;
        isRendered = false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
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
                    Debug.Log("DeadJumpCount : 1 "+player._controller.Velocity.x);

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
                    Debug.Log("DeadJumpCount : 2 "+player._controller.Velocity.x);

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
        /*
        else if (col.CompareTag("Enemy"))
        {
            var spannerEnemyAI = col.GetComponent<SpannerEnemyAI>();
            if (spannerEnemyAI == null)
                return;

            spannerEnemyAI.KillSelf();
        }
        */
        else if (col.CompareTag("Player Bounds") && !hasEnteredCamera)
        {
            hasEnteredCamera = true;
            _collider.enabled = true;
        }

    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player Bounds") && hasEnteredCamera)
        {
            gameObject.SetActive(false);
        }
    }

    void OnWillRenderObject()
    {
        if (isRendered)
            return; 

        isRendered = true;
        hasEnteredCamera = true;
        _collider.enabled = true;
    }

    void OnBecameInvisible()
    {
        /*
        #if UNITY_EDITOR
        Debug.Log("OnBecameInvisible >>> x:" + transform.position.x + ", y:" + transform.position.y);
        #endif
        */
        //parentObject.SetActive(false);
    }
}
