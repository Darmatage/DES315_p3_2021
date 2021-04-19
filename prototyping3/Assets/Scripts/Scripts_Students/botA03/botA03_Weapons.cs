using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botA03_Weapons : MonoBehaviour
{
    //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

    public GameObject weaponThrust;
    
    private GameObject weaponThrustForward;
    private GameObject weaponThrustLeft;
    private GameObject weaponThrustRight;
    private GameObject weaponThrustBack;
    private GameObject weaponThrustNE;
    private GameObject weaponThrustNW;
    private GameObject weaponThrustSE;
    private GameObject weaponThrustSW;

    public GameObject turtleShip;
    public GameObject smokeShell;

    private float rushAmount = 2f;
    private float thrustAmount = 3f;

    private float currentDashTime = 0.0f;
    private float dashCoolTime = 3.0f;
    private float barrierCoolTime = 0.0f;
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
        //if (Input.GetKeyDown(KeyCode.T)){
        if ((Input.GetButtonDown(button1)) && (tankOut == false) && (dashCoolOn == false))
        {
            currentDashTime = 0.5f;
            dashCoolTime = 3.0f;
            dashCoolOn = true;
            
            source.Play();
        }
        else if ((Input.GetButtonDown(button2)) && (weaponOut==false))
        {
            Vector3 tempTransform = turtleShip.transform.position;
            tempTransform.z += 3;
            weaponThrustForward = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, 90f, 90f), weaponThrust.transform);
            tempTransform.z -= 6;
            weaponThrustBack = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, -90f, 90f), weaponThrust.transform);
            tempTransform.z += 3;
            tempTransform.x += 3;
            weaponThrustRight = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, 180f, 90f), weaponThrust.transform);
            tempTransform.x -= 6;
            weaponThrustLeft = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, 0f, 90f), weaponThrust.transform);

            tempTransform.x += 5;
            tempTransform.z += 2;
            weaponThrustNE = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, 135f, 90f), weaponThrust.transform);
            tempTransform.x -= 4;
            weaponThrustNW = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, 45f, 90f), weaponThrust.transform);
            tempTransform.z -= 4;
            weaponThrustSW = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, -45f, 90f), weaponThrust.transform);
            tempTransform.x += 4;
            weaponThrustSE = Instantiate(weaponThrust, tempTransform, Quaternion.Euler(0f, -135f, 90f), weaponThrust.transform);
            
            barrierCoolTime = 0.5f;
            weaponOut = true;
        }
        else if ((Input.GetButtonDown(button3)) && shellActive == false)
        {
            shellActive = true;
            smokeShell.SetActive(true);
            smokeCoolTime = 10.0f;
        }
        else if ((Input.GetButtonDown(button3)) && shellActive == true)
        {
            shellActive = false;
            smokeShell.SetActive(false);
            smokeCoolTime = 0.0f;
        }

        TimeManagement();
    }

    IEnumerator horizontalWDShip()
    {
        yield return new WaitForSeconds(0.6f);
        turtleShip.transform.Translate(0,-rushAmount, 0);
        tankOut = false;
    }
    
    //IEnumerator WithdrawWeapon(){
    //    yield return new WaitForSeconds(0.6f);
        //weaponThrust.transform.Translate(0,-thrustAmount, 0);
    //    Destroy(weaponThrustForward);
    //    Destroy(weaponThrustBack);
    //    Destroy(weaponThrustRight);
    //    Destroy(weaponThrustLeft);
    //    weaponOut = false;
    //}

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

        if (barrierCoolTime > 0.0f)
        {
            barrierCoolTime -= Time.deltaTime;
            weaponThrustForward.transform.Translate(0, 12 * Time.deltaTime, 0);
            weaponThrustBack.transform.Translate(0, 12 * Time.deltaTime, 0);
            weaponThrustRight.transform.Translate(0, 12 * Time.deltaTime, 0);
            weaponThrustLeft.transform.Translate(0, 12 * Time.deltaTime, 0);
            weaponThrustNE.transform.Translate(0, 12 * Time.deltaTime, 0);
            weaponThrustNW.transform.Translate(0, 12 * Time.deltaTime, 0);
            weaponThrustSE.transform.Translate(0, 12 * Time.deltaTime, 0);
            weaponThrustSW.transform.Translate(0, 12 * Time.deltaTime, 0);
        }
        else
        {
            barrierCoolTime = 0.0f;
            Destroy(weaponThrustForward);
            Destroy(weaponThrustBack);
            Destroy(weaponThrustRight);
            Destroy(weaponThrustLeft);
            Destroy(weaponThrustNE);
            Destroy(weaponThrustNW);
            Destroy(weaponThrustSE);
            Destroy(weaponThrustSW);
            weaponOut = false;
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
}
