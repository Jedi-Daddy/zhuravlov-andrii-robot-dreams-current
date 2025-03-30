using UnityEngine;
using XNode;

public class BTPatrol : BTNode
{
    public Transform[] patrolPoints;
    private int currentPoint = 0;

    public override bool Execute()
    {
        if (patrolPoints.Length == 0) return false;

        Transform target = patrolPoints[currentPoint];
        float distance = Vector3.Distance(target.position, AIController.instance.transform.position);

        if (distance < 1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }

        AIController.instance.MoveTo(target.position);
        return true;
    }
}
