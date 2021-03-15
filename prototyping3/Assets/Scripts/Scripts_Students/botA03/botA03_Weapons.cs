using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botA03_Weapons : MonoBehaviour
{
    //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

    public GameObject weaponThrust;
    public GameObject turtleShip;
    public GameObject smokeShell;

    private float rushAmount = 2f;
    private float thrustAmount = 3f;

    private float currentDashTime = 0.0f;
    private float dashCoolTime = 3.0f;

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
        //if (Input.GetKeyDown(KeyCode.T)){
        if ((Input.GetButtonDown(button1)) && (tankOut == false) && (dashCoolOn == false))
        {
            currentDashTime = 0.5f;
            dashCoolTime = 3.0f;
            dashCoolOn = true;
            //turtleShip.transform.Translate(0,0, rushAmount);
            //tankOut = true;
            //StartCoroutine(horizontalWDShip());
            
            source.Play();
        }
        /*else if ((Input.GetButtonDown(button2)) && (tankOut == false))
        {
            turtleShip.transform.Translate(0,rushAmount, 0);
            tankOut = true;
            StartCoroutine(horizontalWDShip());
        }*/
        else if ((Input.GetButtonDown(button2))&&(weaponOut==false)){
            weaponThrust.transform.Translate(0,thrustAmount, 0);
            weaponOut = true;
            StartCoroutine(WithdrawWeapon());
        }
        else if ((Input.GetButtonDown(button3)) && shellActive == false)
        {
            shellActive = true;
            smokeShell.SetActive(true);
        }
        else if ((Input.GetButtonDown(button3)) && shellActive == true)
        {
            shellActive = false;
            smokeShell.SetActive(false);
        }

        TimeManagement();
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
    }
}
