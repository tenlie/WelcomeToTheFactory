using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum FollowType
    {
        MoveTowards,
        Lerp
    }

    public FollowType Type = FollowType.MoveTowards;
    public PathDefinition Path;
    public float Speed;
    public float MaxDistanceToGoal = 0.1f;
    public SpannerEnemyAI spannerEnemyAI;

    private IEnumerator<Transform> _currentPoint;

    public void Start()
    {
        Path = transform.parent.FindChild("Path").GetComponent<PathDefinition>();
        spannerEnemyAI = transform.parent.gameObject.GetComponent<SpannerEnemyAI>();

        if (Path == null)
        {
            //Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        _currentPoint = Path.GetPathsEnumerator();
        _currentPoint.MoveNext();

        if (_currentPoint.Current == null)
            return;

        transform.position = _currentPoint.Current.position;
    }

    public void Update()
    {
        if (_currentPoint == null || _currentPoint.Current == null)
            return;

        if (transform.position.y < _currentPoint.Current.position.y)
            Speed -= 4 * Time.deltaTime;
        else
            Speed += 4 * Time.deltaTime;

        if (Type == FollowType.MoveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
        }
        else if (Type == FollowType.Lerp)
            transform.position = Vector3.Lerp(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);


        var distanceSquared = (transform.position - _currentPoint.Current.position).sqrMagnitude;

        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
        {
            _currentPoint.MoveNext();
        }
    }

    public void Destroy()
    {
        spannerEnemyAI.Animator.SetBool("Attack", false); 
    }
}
