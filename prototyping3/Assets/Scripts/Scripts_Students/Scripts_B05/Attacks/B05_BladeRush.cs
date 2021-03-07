using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_BladeRush : MonoBehaviour
{
    private bool b_active = false;    // false when attack can be activated
    private bool b_attacking = false; // true when attacking

    private float timer = 0.0f;     // keeps track of attack start time
    public float t_startup = 0.0f;  // holds no revalance right now
    public float t_length = 0.0f;   // length of attack
    public float t_recovery = 0.0f; // length after attack before being able to move again
    public float t_cooldown = 0.0f; // time until attack can be used again
    public int damage = 0;

    public MeshRenderer vent;
    public Material mat_able;
    public Material mat_used;

    public Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        b_active = false;
        b_attacking = false;
        timer = 0.0f;
        vent.material = mat_able;
    }

    // Update is called once per frame
    void Update()
    {
        if (!b_active)
            return;

        timer += Time.deltaTime;
        if (timer > (t_startup + t_length) && b_attacking)
        {
            EndAttack();
        }
        if (timer > (t_startup + t_length + t_recovery))
        {
            // give back control to player
        }
        if (timer > t_cooldown)
        {
            Ready();
        }
    }

    public void Attack()
    {
        // begin attack if avaliable
        if (!b_active)
        {
            BeginAttack();
        }
    }

    private void BeginAttack()
    {
        b_active = true;
        b_attacking = true;
        vent.material = mat_used;
        timer = 0.0f;
        ani.SetBool("b_attacking", true);
    }

    private void EndAttack()
    {
        b_attacking = false;
        ani.SetBool("b_attacking", false);
    }

    private void Ready()
    {
        b_active = false;
        vent.material = mat_able;
        timer = 0.0f;
    }
}
