using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_Shield : B05_UNode
{
    private bool b_running;

    private float timer;
    private const float MIN_TIME = 2.8f;
    private const float MAX_TIME = 4.2f;
    private float goal_time;

    private B05_MagneticForce mag;
    private B05_AI ai;

    public B05N_Shield(B05_MagneticForce mag, B05_AI ai)
    {
        this.mag = mag;
        this.ai = ai;
        timer = 0.0f;
        b_running = false;
        goal_time = Random.Range(MIN_TIME, MAX_TIME);
    }

    public override NodeState Evaluate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > goal_time)
        {
            timer = 0.0f;
            goal_time = Random.Range(MIN_TIME, MAX_TIME);

            b_running = false;

            mag.EndAttract();

            return NodeState.SUCCESS;
        }

        if (!b_running)
        {
            b_running = true;

            mag.BeginAttract();
        }

        return NodeState.RUNNING;
    }

    public override float CalcScore()
    {
        List<B05_MiniTop> tops = new List<B05_MiniTop>();
        mag.GetTops(tops);

        if (tops.Count == 0)
            return 0.0f;

        if (ai.low_health)
        {
            return Mathf.Clamp(tops.Count * 0.25f, 0.0f, 1.0f);
        }
        else
        {
            return Mathf.Clamp(tops.Count * 0.05f, 0.0f, 0.2f);
        }
    }
}
