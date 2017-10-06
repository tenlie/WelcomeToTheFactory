using UnityEngine;

public class MissileBounds : MonoBehaviour {

    //캐릭터가 경계에 도달할 시 적용할 액션을 정의할 BoundsBehavior 선언
    public enum BoundsBehavior
    {
        Nothing,
        Constrain,
        Kill
    }

    //경계값
    public BoxCollider2D Bounds;

    public BoundsBehavior Above;
    public BoundsBehavior Below;
    public BoundsBehavior Left;
    public BoundsBehavior Right;

    //private Player _player;
    private BoxCollider2D _boxCollider;

    public void Start()
    {
        //_player = GetComponent<Player>(); //캐릭터가 가진 Player라는 스크립트
        _boxCollider = GetComponent<BoxCollider2D>(); //캐릭터의 BoxCollider2D
    }

    public void Update()
    {
        //if (_player.IsDead)
        //    return;

        //캐릭터의 BoxCollider2D 크기의 반값
        var colliderSize = new Vector2(
            _boxCollider.size.x * Mathf.Abs(transform.localScale.x),
            _boxCollider.size.y * Mathf.Abs(transform.localScale.y)) / 2;

        //경계값 가장 위보다 높이 있을 시 경계값 아래로 캐릭터를 내려줌.
        if (Above != BoundsBehavior.Nothing && transform.position.y + colliderSize.y > Bounds.bounds.max.y)
            ApplyBoundsBehavior(Above, new Vector2(transform.position.x, Bounds.bounds.max.y - colliderSize.y));

        //경계값 가장 아래 이하로 떨어질 경우 캐릭터를 죽임.
        if (Below != BoundsBehavior.Nothing && transform.position.y - colliderSize.y < Bounds.bounds.min.y)
            ApplyBoundsBehavior(Below, new Vector2(transform.position.x, Bounds.bounds.min.y + colliderSize.y));

        //경계값 가장 오른쪽보다 더 오른쪽으로 넘어갈 시 경계값보다 왼쪽으로 캐릭터를 옮겨줌.
        if (Right != BoundsBehavior.Nothing && transform.position.x + colliderSize.x > Bounds.bounds.max.x)
            ApplyBoundsBehavior(Right, new Vector2(Bounds.bounds.max.x - colliderSize.x, transform.position.y));

        //경계값 가장 왼쪽보다 더 왼쪽으로 넘어갈 시 경계값보다 오른쪽으로 캐릭터를 옮겨줌.
        if (Left != BoundsBehavior.Nothing && transform.position.x - colliderSize.x < Bounds.bounds.min.x)
            ApplyBoundsBehavior(Left, new Vector2(Bounds.bounds.min.x + colliderSize.x, transform.position.y));
    }

    private void ApplyBoundsBehavior(BoundsBehavior behavior, Vector2 constrainedPosition)
    {
        if (behavior == BoundsBehavior.Kill)
        {
            Destroy(gameObject);
        }

        //transform.position = constrainedPosition;
    }
}
