using UnityEngine;
using XNode;

public class BTRangedAttack : BTNode
{
    public override bool Execute()
    {
        if (AIController.instance.CanShootFireball())
        {
            AIController.instance.ShootFireball();
            return true;
        }
        return false;
    }
}
