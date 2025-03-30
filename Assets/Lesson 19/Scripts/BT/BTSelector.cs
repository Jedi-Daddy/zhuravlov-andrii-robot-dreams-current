using System.Collections.Generic;
using UnityEngine;

[NodeTint("#6A5ACD")] // Фиолетовый цвет в редакторе
public class BTSelector : BTNode
{
    [Input] public List<BTNode> children;

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            if (child.Evaluate() == NodeState.Success)
                return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
