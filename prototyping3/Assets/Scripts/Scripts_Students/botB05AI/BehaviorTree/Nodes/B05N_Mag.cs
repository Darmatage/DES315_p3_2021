using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_Mag : B05_UNode
{
    private bool b_running;
    private bool b_attracting;

    private float timer;
    private const float MIN_TIME = 2.0f;
    private const float MAX_TIME = 3.5f;
    private float goal_time;

    private const float RANGE = 4.5f;
    private const float ANGLE = 30.0f;

    private Transform center;
    private Transform enemy_trans;
    private B05_MagneticForce mag;

    public B05N_Mag (B05_MagneticForce mag, Transform center, Transform enemy_trans)
    {
        this.mag = mag;
        this.center = center;
        this.enemy_trans = enemy_trans;
        timer = 0.0f;
        b_running = false;
        b_attracting = true;
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

            if (b_attracting)
            {
                mag.EndAttract();
            }
            else
            {
                mag.EndRepel();
            }

            return NodeState.SUCCESS;
        }

        if (!b_running)
        {
            b_running = true;

            if (b_attracting)
            {
                mag.BeginAttract();
            }
            else
            {
                mag.BeginRepel();
            }
        }

        return NodeState.RUNNING;
    }

    public override float CalcScore()
    {
        List<B05_MiniTop> tops = new List<B05_MiniTop>();
        mag.GetTops(tops);

        if (tops.Count == 0)
            return 0.0f;

        int attract_incentive = 0;
        int repel_incentive = 0;

        Vector2 bot_pos = new Vector2(center.position.x, center.position.z);
        Vector2 enemy_pos = new Vector2(enemy_trans.position.x, enemy_trans.position.z);

        foreach (var t in tops)
        {
            // check if top is in good range from opponent
            Vector2 top_pos = new Vector2(t.transform.position.x, t.transform.position.z);
            if (Vector2.Distance(enemy_pos, top_pos) <= RANGE)
            {
                // check objects align well
                if (Vector2.Angle(top_pos - bot_pos, enemy_pos - bot_pos) <= ANGLE)
                {
                    // check if attracting or repelling is better
                    if (Vector2.Distance(top_pos, bot_pos) > Vector2.Distance(enemy_pos, bot_pos))
                    {
                        ++attract_incentive;
                    }
                    else
                    {
                        ++repel_incentive;
                    }
                }
            }
        }


        b_attracting = (attract_incentive >= repel_incentive);
        int largest_incentive = (b_attracting ? attract_incentive : repel_incentive);

        return Mathf.Clamp(largest_incentive * 0.6f, 0.0f, 1.8f);
    }
}
