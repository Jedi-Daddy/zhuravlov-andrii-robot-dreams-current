using UnityEngine;
using XNode;

public class BTMeleeAttack : BTNode
{
    public override bool Execute()
    {
        if (AIController.instance.CanMeleeAttack())
        {
            AIController.instance.PerformMeleeAttack();
            return true;
        }
        return false;
    }
}
