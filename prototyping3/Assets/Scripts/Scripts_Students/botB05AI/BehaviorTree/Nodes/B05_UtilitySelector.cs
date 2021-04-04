using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B05_UtilitySelector : B05_Node
{
    protected List<B05_UNode> nodes = new List<B05_UNode>();
    protected int cur;

    public B05_UtilitySelector(List<B05_UNode> nodes)
    {
        this.nodes = nodes;
        cur = -1;
    }

    public override NodeState Evaluate()
    {
        if (cur == -1)
        {
            cur = SelectNextAction();
        }

        _nodeState = nodes[cur].Evaluate();

        if (_nodeState == NodeState.SUCCESS)
            cur = -1;

        return _nodeState;
    }

    // use utility scores to randomly select a weighted action
    private int SelectNextAction()
    {
        List<float> scores = new List<float>();
        float total_score = 0.0f;

        // calc score for each action in the list
        foreach(var node in nodes)
        {
            float score = node.CalcScore();
            scores.Add(score);
            total_score += score;
        }

        // randomly select an action
        int i = -1;
        float rand = Random.Range(0.0f, total_score);
        while (rand >= 0.0f)
        {
            ++i;
            rand -= scores[i];
        }

        return i;
    }
}
