using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorenzoDeMaine_FlingTrap : MonoBehaviour
{
    public float startRotation = 0f;
    public float triggeredRotation = 90f;
    public float torque = 10000f;
    public float damper = 120f;
    HingeJoint hinge;

    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useSpring = true;
    }

    // Update is called once per frame
    void Update()
    {
        JointSpring spring = new JointSpring();
        spring.spring = torque;
        spring.damper = damper;

        if (triggered)
        {
            spring.targetPosition = triggeredRotation;
        }
        else
        {
            spring.targetPosition = startRotation;
        }

        hinge.spring = spring;
        hinge.useLimits = true;

    }

    public void triggerOn()
    {
        triggered = true;
    }

    public void triggerOff()
    {
        triggered = false;
    }
}
