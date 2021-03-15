using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot05_Weapons : MonoBehaviour
{
    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script

    public B05_BladeRush a_bladerush;
    public B05_ShootTop a_shoottop;
    public B05_MagneticForce a_magforce;

    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
    }

    void Update()
    {
        if (Input.GetButtonDown(button1))
        {
            a_bladerush.Attack();
        }

        if (Input.GetButtonDown(button2))
        {
            a_shoottop.BeginAttack();
        }
        if (Input.GetButtonUp(button2))
        {
            a_shoottop.EndAttack();
        }

        if (Input.GetButtonDown(button3))
        {
            a_magforce.BeginAttract();
        }
        if (Input.GetButtonUp(button3))
        {
            a_magforce.EndAttract();
        }

        if (Input.GetButtonDown(button4))
        {
            a_magforce.BeginRepel();
        }
        if (Input.GetButtonUp(button4))
        {
            a_magforce.EndRepel();
        }
    }
}