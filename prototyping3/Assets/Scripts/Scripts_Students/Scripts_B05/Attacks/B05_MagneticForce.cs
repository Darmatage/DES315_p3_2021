using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_MagneticForce : MonoBehaviour
{
    private B05_MiniTop[] tops;

    public Bot05_Move b05;

    public Animator ani;

    public GameObject circles;

    // Start is called before the first frame update
    void Start()
    {
        tops = new B05_MiniTop[0];
        circles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (b05.IsState(Bot05_Move.STATE.ATTRACTING))
        {
            for (int i = 0; i < tops.Length; ++i)
                tops[i].MoveToward(b05.GetCenter().position);
        }
        else if (b05.IsState(Bot05_Move.STATE.REPELING))
        {
            for (int i = 0; i < tops.Length; ++i)
                tops[i].MoveAway(b05.GetCenter().position);
        }
        else
        {
            circles.SetActive(false);
        }
    }

    public void BeginRepel()
    {
        if (b05.IsState(Bot05_Move.STATE.ATTRACTING))
        {
            EndAttract();
        }

        if (b05.IsState(Bot05_Move.STATE.NORMAL))
        {
            tops = FindObjectsOfType<B05_MiniTop>();
            b05.SetState(Bot05_Move.STATE.REPELING);
            circles.SetActive(true);
            ani.SetBool("b_repel", true);
        }
    }

    public void EndRepel()
    {
        if (b05.IsState(Bot05_Move.STATE.REPELING))
        {
            b05.SetState(Bot05_Move.STATE.NORMAL);
            tops = new B05_MiniTop[0];
            circles.SetActive(false);
            ani.SetBool("b_repel", false);
        }
    }

    public void BeginAttract()
    {
        if (b05.IsState(Bot05_Move.STATE.REPELING))
        {
            EndRepel();
        }

        if (b05.IsState(Bot05_Move.STATE.NORMAL))
        {
            tops = FindObjectsOfType<B05_MiniTop>();
            b05.SetState(Bot05_Move.STATE.ATTRACTING);
            circles.SetActive(true);
            ani.SetBool("b_attract", true);
        }
    }

    public void EndAttract()
    {
        if (b05.IsState(Bot05_Move.STATE.ATTRACTING))
        {
            b05.SetState(Bot05_Move.STATE.NORMAL);
            tops = new B05_MiniTop[0];
            circles.SetActive(false);
            ani.SetBool("b_attract", false);
        }
    }
}
