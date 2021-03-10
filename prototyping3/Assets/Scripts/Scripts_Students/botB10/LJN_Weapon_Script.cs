using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJN_Weapon_Script : MonoBehaviour
{
    public GameObject LeftGrabArm;
    public GameObject RightGrabArm;
    public GameObject SawArm;

    public string button1; //grab
    public string button2; //saw
    public string button3; //nothing
    public string button4; //nothing

    public float GrabSpeed = 5.0f;
    public float SawSpeed = 5.0f;

    private float ArmLerpAmountLeft = 0;
    private float ArmLerpAmountRight = 0;
    private float SawLerpAmount = 0;

    //euler angle for arms
    public Vector3 LeftGrabArmStart;
    public Vector3 RightGrabArmStart;
    public Vector3 SawArmStart;

    public Vector3 LeftGrabArmEnd;
    public Vector3 RightGrabArmEnd;
    public Vector3 SawArmEnd;

    public bool SawCanDown = true;
    public bool LeftCanMove = true;
    public bool RightCanMove = true;

    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        
    }

    // Update is called once per frame
    void Update()
    {


        float dt = Time.deltaTime;
        if(Input.GetButton(button1))
        {
            if(LeftCanMove)
            ArmLerpAmountLeft += dt * GrabSpeed;
            if (RightCanMove)
                ArmLerpAmountRight += dt * GrabSpeed;

            if (ArmLerpAmountLeft > 1.0f) ArmLerpAmountLeft = 1.0f;
            if (ArmLerpAmountRight > 1.0f) ArmLerpAmountRight = 1.0f;

        }
        else
        {
            LeftCanMove = true;
            RightCanMove = true;

            ArmLerpAmountLeft -= dt * GrabSpeed;
            ArmLerpAmountRight -= dt * GrabSpeed;

            if (ArmLerpAmountLeft < 0.0f) ArmLerpAmountLeft = 0.0f;
            if (ArmLerpAmountRight < 0.0f) ArmLerpAmountRight = 0.0f;
        }

        if (Input.GetButton(button2))
        {
            if(SawCanDown)
            SawLerpAmount += dt * SawSpeed;

            if (SawLerpAmount > 1.0f) SawLerpAmount = 1.0f;

        }
        else
        {
            SawCanDown = true;
            SawLerpAmount -= dt * SawSpeed;

            if (SawLerpAmount < 0.0f) SawLerpAmount = 0.0f;
        }

      
        LeftGrabArm.transform.localEulerAngles = Vector3.Lerp(LeftGrabArmStart, LeftGrabArmEnd, ArmLerpAmountLeft);
        RightGrabArm.transform.localEulerAngles = Vector3.Lerp(RightGrabArmStart, RightGrabArmEnd, ArmLerpAmountRight);
        SawArm.transform.localEulerAngles = Vector3.Lerp(SawArmStart, SawArmEnd, SawLerpAmount);

        
    }
}
