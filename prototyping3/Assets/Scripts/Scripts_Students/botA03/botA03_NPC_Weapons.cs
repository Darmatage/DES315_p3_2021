using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botA03_NPC_Weapons : MonoBehaviour
{
    //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

    public GameObject weaponThrust;
    public GameObject turtleShip;
    public GameObject smokeShell;

    private float rushAmount = 2f;
    private float thrustAmount = 3f;

    private float currentDashTime = 0.0f;
    private float dashCoolTime = 3.0f;
    private float smokeCoolTime = 0.0f;

    private bool tankOut = false;
    private bool weaponOut = false;
    private bool dashCoolOn = false;
    private bool shellActive = false;

    private AudioSource source;

    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script

    void Start(){
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        source = GetComponent<AudioSource>();
        
        smokeShell.SetActive(false);
    }

    void Update(){
        
        //if ((Input.GetButtonDown(button1)) && (tankOut == false) && (dashCoolOn == false))
        //{
        //    DashAttack();
        //}
        //else if ((Input.GetButtonDown(button2))&&(weaponOut==false))
        //{
        //    RodAttack();
        //}
        //else if ((Input.GetButtonDown(button3)) && shellActive == false)
        //{
        //    SmokeOn();
        //}
        //else if ((Input.GetButtonDown(button3)) && shellActive == true)
        //{
        //    SmokeOff();
        //}

        TimeManagement();
    }

    public void DashAttack()
    {
        currentDashTime = 0.5f;
        dashCoolTime = 3.0f;
        dashCoolOn = true;

        source.Play();
    }

    public void RodAttack()
    {
        weaponThrust.transform.Translate(0,thrustAmount, 0);
        weaponOut = true;
        StartCoroutine(WithdrawWeapon());
    }

    public void SmokeOn()
    {
        shellActive = true;
        smokeShell.SetActive(true);
        smokeCoolTime = 10.0f;
    }

    public void SmokeOff()
    {
        shellActive = false;
        smokeShell.SetActive(false);
        smokeCoolTime = 0.0f;
    }
    
    IEnumerator horizontalWDShip()
    {
        yield return new WaitForSeconds(0.6f);
        turtleShip.transform.Translate(0,-rushAmount, 0);
        tankOut = false;
    }
    
    IEnumerator WithdrawWeapon(){
        yield return new WaitForSeconds(0.6f);
        weaponThrust.transform.Translate(0,-thrustAmount, 0);
        weaponOut = false;
    }

    void TimeManagement()
    {
        if (currentDashTime > 0.0f)
        {
            currentDashTime -= Time.deltaTime;
            turtleShip.transform.Translate(0,0, 15 * rushAmount * Time.deltaTime);
        }
        else
        {
            currentDashTime = 0.0f;
        }

        if (dashCoolTime > 0.0f)
        {
            dashCoolTime -= Time.deltaTime;
        }
        else
        {
            dashCoolTime = 3.0f;
            dashCoolOn = false;
        }

        if (smokeCoolTime > 0.0f)
        {
            smokeCoolTime -= Time.deltaTime;
        }
        else
        {
            smokeShell.SetActive(false);
            shellActive = false;
            smokeCoolTime = 0.0f;
        }
    }

    public void DemoAttack()
    {
        weaponThrust.transform.Translate(0,thrustAmount, 0);
        weaponOut = true;
        StartCoroutine(WithdrawWeapon());
    }
}
