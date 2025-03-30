using System.Collections.Generic;
using XNode;

[NodeWidth(200)]
public class BTSequence : BTNode
{
    [Output(dynamicPortList = true)] public List<BTNode> children;

    public override bool Execute()
    {
        foreach (BTNode child in children)
        {
            if (!child.Execute())
            {
                return false;
            }
        }
        return true;
    }
}
