using UnityEngine;

public class PlayerBounds : MonoBehaviour {

    //캐릭터가 경계에 도달할 시 적용할 액션을 정의할 BoundsBehavior 선언
    public enum BoundsBehavior {
        Nothing,    //0
        Constrain,  //1
        Kill        //2
    }

    //경계값
    public BoxCollider2D Bounds;

    public BoundsBehavior Above;
    public BoundsBehavior Below;
    public BoundsBehavior Left;
    public BoundsBehavior Right;

    private Vector2 colliderSize;
    private Player _player;
    private BoxCollider2D _playerBoxCollider2D;

    public void Start()
    {
        Bounds = GameObject.FindWithTag("Player Bounds").GetComponent<BoxCollider2D>();
        _player = GetComponent<Player>();
        _playerBoxCollider2D = GetComponent<BoxCollider2D>();
        colliderSize = new Vector2(
            (_playerBoxCollider2D.size.x * Mathf.Abs(transform.localScale.x)/2),
            _playerBoxCollider2D.size.y * Mathf.Abs(transform.localScale.y));
    }

    public void Update() {
        if (_player.IsDead)
            return;

        //var pos = _player.transform.position;
        //pos.x = Mathf.Clamp(pos.x, -12.8f+colliderSize.x, 12.8f-colliderSize.x);
        //pos.y = Mathf.Clamp(pos.y, -7.2f, 7.2f-colliderSize.y);


        //경계값 가장 위보다 높이 있을 시 경계값 아래로 캐릭터를 내려줌.
        //if (Above != BoundsBehavior.Nothing && transform.position.y + colliderSize.y > Bounds.bounds.max.y)
        if (Above == BoundsBehavior.Constrain)
            ApplyBoundsBehavior(Above, new Vector2(transform.position.x, Bounds.bounds.max.y - colliderSize.y));

        //경계값 가장 아래 이하로 떨어질 경우 캐릭터를 죽임.
        if (Below != BoundsBehavior.Nothing && transform.position.y < Bounds.bounds.min.y)
            ApplyBoundsBehavior(Below, new Vector2(transform.position.x, Bounds.bounds.min.y + colliderSize.y));

        //경계값 가장 오른쪽보다 더 오른쪽으로 넘어갈 시 경계값보다 왼쪽으로 캐릭터를 옮겨줌.
        //if (Right != BoundsBehavior.Nothing && transform.position.x + colliderSize.x > Bounds.bounds.max.x)
        //    ApplyBoundsBehavior(Right, new Vector2(Bounds.bounds.max.x - colliderSize.x, transform.position.y));

        //경계값 가장 왼쪽보다 더 왼쪽으로 넘어갈 시 경계값보다 오른쪽으로 캐릭터를 옮겨줌.
        //if (Left != BoundsBehavior.Nothing && transform.position.x - colliderSize.x < Bounds.bounds.min.x)
        //    ApplyBoundsBehavior(Left, new Vector2(Bounds.bounds.min.x + colliderSize.x, transform.position.y));
    }

    private void ApplyBoundsBehavior(BoundsBehavior behavior, Vector2 constrainedPosition)
    {
        if (behavior == BoundsBehavior.Kill)
        {
            LevelManager.Instance.KillPlayer();
            return;
        }
    }
}
