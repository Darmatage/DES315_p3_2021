using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B05N_Move : B05_UNode
{
    private float MIN_DIST = 0.2f;
    private float OFFSET_VALUE = 10.0f;
    private float MAX_TIME = 2.0f;

    private Transform enemy;
    private NavMeshAgent agent;
    private B05_AI ai;

    private Vector3 goal;
    private bool b_have_goal;

    private float timer;
        
    public B05N_Move (Transform enemy, NavMeshAgent agent, B05_AI ai)
    {
        this.enemy = enemy;
        this.agent = agent;
        this.ai = ai;
        goal = Vector3.zero;
        b_have_goal = false;
        timer = 0.0f;
    }

    public override NodeState Evaluate()
    {
        ai.mesh.material = ai.matmove;
        if (!b_have_goal)
        {
            FindGoal();
            timer = 0.0f;
        }

        timer += Time.deltaTime;

        if (Vector3.Distance(ai.transform.position, agent.destination) < MIN_DIST)
        {
            agent.isStopped = true;
            b_have_goal = false;
            return NodeState.SUCCESS;
        }
        else if (timer > MAX_TIME)
        {
            agent.isStopped = true;
            b_have_goal = false;
            return NodeState.FAILURE;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(goal);
            ai.goalsphere.position = agent.destination;
            return NodeState.RUNNING;
        }
    }

    // find a valid location to move to that matched our perfered distance
    private void FindGoal()
    {
        // closest goal
        Vector3 dir = ai.transform.position - enemy.position;
        dir.y = 0.0f;
        dir.Normalize();
        goal = enemy.position + (dir * ai.distance_goal);
        //goal.y += 2.0f;

        // check if goal is valid otherwise find a goal to the left or right of enemy

        //if (Vector3.Distance(enemy.position, ai.transform.position) >= ai.distance_goal);

        b_have_goal = true;

        Debug.Log("Found a new goal.");
    }

    public override float CalcScore()
    {
        float offset = Mathf.Abs(ai.distance_goal - Vector3.Distance(enemy.position, ai.transform.position));
        float factor = offset / OFFSET_VALUE;
        return factor;// * factor;
    }
}
