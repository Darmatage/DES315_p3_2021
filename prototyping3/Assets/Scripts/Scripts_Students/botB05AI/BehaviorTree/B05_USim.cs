using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_USim : B05_UNode
{
    protected List<B05_UNode> nodes = new List<B05_UNode>();

    public B05_USim(List<B05_UNode> n)
    {
        nodes = n;
    }

    public override NodeState Evaluate()
    {
        _nodeState = NodeState.SUCCESS;

        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    break;
            }
        }

        return _nodeState;
    }

    public override float CalcScore()
    {
        float total_score = 0.0f;

        foreach (var node in nodes)
        {
            total_score += node.CalcScore();
        }

        return total_score;
    }
}
