using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_BladeRush : MonoBehaviour
{
    private bool b_active = false;    // false when attack can be activated
    private bool b_attacking = false; // true when attacking
    private bool b_setnormal = false;

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

    public Bot05_Move b05;

    // Start is called before the first frame update
    void Start()
    {
        b_active = false;
        b_attacking = false;
        b_setnormal = false;
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
        if (timer > (t_startup + t_length + t_recovery) && !b_setnormal)
        {
            b05.SetState(Bot05_Move.STATE.NORMAL); // give control back to player
            b_setnormal = true;
        }
        if (timer > t_cooldown)
        {
            Ready();
        }
    }

    public void Attack()
    {
        // begin attack if avaliable
        if (!b_active && b05.IsState(Bot05_Move.STATE.NORMAL))
        {
            BeginAttack();
        }
    }

    private void BeginAttack()
    {
        b_active = true;
        b_attacking = true;
        b_setnormal = false;
        vent.material = mat_used;
        timer = 0.0f;
        ani.SetBool("b_attacking", true);
        b05.SetState(Bot05_Move.STATE.ATTACKING);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void EndAttack()
    {
        b_attacking = false;
        ani.SetBool("b_attacking", false);
        b05.SetState(Bot05_Move.STATE.RECOVERING);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void Ready()
    {
        b_active = false;
        vent.material = mat_able;
        timer = 0.0f;
    }
}
