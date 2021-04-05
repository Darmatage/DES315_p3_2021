using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_USequence : B05_UNode
{
    protected List<B05_UNode> nodes = new List<B05_UNode>();
    protected int cur;

    public B05_USequence(List<B05_UNode> n)
    {
        nodes = n;
        cur = 0;
    }

    public override NodeState Evaluate()
    {
        if (cur == nodes.Count)
            cur = 0;

        switch (nodes[cur].Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                return _nodeState;
            case NodeState.SUCCESS:
                ++cur;
                break;
            case NodeState.FAILURE:
                cur = 0;
                _nodeState = NodeState.FAILURE;
                return _nodeState;
        }

        _nodeState = (cur == nodes.Count) ? NodeState.SUCCESS : NodeState.RUNNING;
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
