using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_Shoot : B05_UNode
{
    private float SCORE_MAX = 1.0f;
    private float SCORE_TIME_MAX = 12.0f;
    private float last_shot; // time last shot

    private float DIST_FACTOR = 25.0f;
    private float CHARGE_TIME_MAX = 2.0f;
    private float charge_timer;
    private float charge_goal;

    private float MIN_DEGREE = 0.2f;
    private Vector3 dir_goal;

    private B05_ShootTop shoot;
    private Bot05_Move bot;
    private Transform enemy_trans;
    private Rigidbody enemy_body;

    private bool b_running;

    public B05N_Shoot (Transform enemy_trans, Rigidbody enemy_body, Bot05_Move bot, B05_ShootTop shoot)
    {
        this.enemy_trans = enemy_trans;
        this.enemy_body = enemy_body;
        this.bot = bot;
        this.shoot = shoot;
        b_running = false;
        last_shot = Time.time;
    }

    public override NodeState Evaluate()
    {
        // validate rigidbody
        if (enemy_body == null)
        {
            _nodeState = NodeState.FAILURE;
            return _nodeState;
        }

        // begin attack
        if (!b_running)
        {
            charge_goal = CalcCharge();
            //dir_goal = CalcTrajectory();
            b_running = true;
            shoot.BeginAttack();
            charge_timer = 0.0f;
        }

        // turn towards goal
        Turn();

        // hold for charge amount
        charge_timer += Time.deltaTime;
        if (charge_timer > charge_goal)
        {
            shoot.EndAttack();
            b_running = false;
            last_shot = Time.time;
            bot.horizontal_movement = 0.0f;
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }

        // return working
        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }

    private float CalcCharge()
    {
        float dist = Vector2.Distance(new Vector2(enemy_trans.position.x, enemy_trans.position.z),
                                      new Vector2(bot.transform.position.x, bot.transform.position.z));
        float base_charge = dist / DIST_FACTOR;
        float variable = 0.1f;
        float charge =  Mathf.Clamp(Random.Range(base_charge - variable, base_charge + variable), 0.0f, 1.0f);
        return charge * CHARGE_TIME_MAX;
    }

    private Vector2 CalcTrajectory()
    {
        Vector2 enemy_pos = new Vector2(enemy_trans.position.x, enemy_trans.position.z);
        Vector2 cur_pos = new Vector2(bot.transform.position.x, bot.transform.position.z);

        Vector2 enemy_goal = enemy_pos + (new Vector2(enemy_body.velocity.x, enemy_body.velocity.z) * (charge_goal + 0.1f));
        Vector2 dir = enemy_goal - cur_pos;
        return dir.normalized;
    }

    private void Turn()
    {
        dir_goal = CalcTrajectory();
        Vector2 facing = new Vector2(bot.transform.forward.x, bot.transform.forward.z);

        float angle = Vector2.SignedAngle(dir_goal, facing);

        if (Mathf.Abs(angle) < MIN_DEGREE)
        {
            bot.horizontal_movement = 0.0f;
            return;
        }

        if (angle > 0.0f)
        {
            bot.horizontal_movement = 1.3f;
        }
        else
        {
            bot.horizontal_movement = -1.3f;
        }
    }

    // increase odds of using attack over time
    public override float CalcScore()
    {
        if (!shoot.IsAvaliable())
            return 0.0f;

        float diff = Time.time - last_shot;
        return Mathf.Clamp(diff / SCORE_TIME_MAX, 0.0f, SCORE_MAX);
    }
}
