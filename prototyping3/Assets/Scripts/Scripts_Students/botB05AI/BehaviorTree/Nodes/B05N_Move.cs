using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B05N_Move : B05_UNode
{
    private Transform enemy;
    private NavMeshAgent agent;
    private B05_AI ai;

    private Vector3 goal;
    private bool b_have_goal;
        
    public B05N_Move (Transform enemy, NavMeshAgent agent, B05_AI ai)
    {
        this.enemy = enemy;
        this.agent = agent;
        this.ai = ai;
        goal = Vector3.zero;
        b_have_goal = false;
    }

    public override NodeState Evaluate()
    {
        ai.mesh.material = ai.matmove;
        if (!b_have_goal)
        {
            FindGoal();
        }

        if (Vector3.Distance(ai.transform.position, goal) < 0.3f)
        {
            agent.isStopped = true;
            b_have_goal = false;
            return NodeState.SUCCESS;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(goal);
            return NodeState.RUNNING;
        }
    }

    // find a valid location to move to that matched our perfered distance
    private void FindGoal()
    {
        // closest goal
        Vector3 dir = ai.transform.position - enemy.position;
        dir.z = 0.0f;
        dir.Normalize();
        goal = enemy.position + (dir * ai.distance_goal);

        // check if goal is valid otherwise find a goal to the left or right of enemy

        //if (Vector3.Distance(enemy.position, ai.transform.position) >= ai.distance_goal);

        b_have_goal = true;
    }

    public override float CalcScore()
    {
        float offset = Mathf.Abs(ai.distance_goal - Vector3.Distance(enemy.position, ai.transform.position));
        float factor = offset / 10.0f;
        return factor;// * factor;
    }
}
