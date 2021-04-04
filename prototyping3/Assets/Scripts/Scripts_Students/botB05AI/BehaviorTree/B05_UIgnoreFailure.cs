using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_UIgnoreFailure : B05_UNode
{
    private B05_UNode node;

    public B05_UIgnoreFailure(B05_UNode node)
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {
        _nodeState = node.Evaluate();

        if (_nodeState == NodeState.FAILURE)
        {
            _nodeState = NodeState.SUCCESS;
        }

        return _nodeState;
    }

    public override float CalcScore()
    {
        return node.CalcScore();
    }
}
