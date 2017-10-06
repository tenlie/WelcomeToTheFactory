using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathDefinition : MonoBehaviour
{
    public Transform[] Points;
    public FollowPath followPath;
    public GameObject missile;
    public SpannerEnemyAI spannerEnemyAI;
    public MissileAfterImageEffect missileAfterImageEffect;

    public IEnumerator<Transform> GetPathsEnumerator()
    {
        if (Points == null || Points.Length < 1)
            yield break;
         
        var direction = 1;
        var index = 0;
        var count = 0;

        while (true)
        {
            yield return Points[index];
        
            if (Points.Length == 1)
                continue;

            if (index <= 0) 
            {
                direction = 1;
            }
            else if (index >= Points.Length - 1)
            {
                direction = -1;
            }

            index = index + direction;
            count++;

            if (count > 2)
            {
                missileAfterImageEffect.afterImageEnabled = false;
                missile.SetActive(false);
                spannerEnemyAI.Animator.SetBool("IsAttacking", false);
                break;
            }
                
        }
    }

    public void OnDrawGizmos()
    {

        if (Points == null || Points.Length < 2)
            return;

        var points = Points.Where(t => t != null).ToList();
        if (points.Count < 2)
            return;

        for (var i = 1; i < points.Count; i++)
        {
            Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
        }
    }
}
