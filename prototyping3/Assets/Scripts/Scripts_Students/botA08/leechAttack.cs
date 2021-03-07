using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leechAttack : MonoBehaviour
{
    public GameObject weaponThrust;
    public float cooldown = 5.0f;

    private float cooldownTimer = 0;
    private float thrustAmount = 1f;

    private bool weaponOut = false;

    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script


    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
    }


    void Update()
    {
        if ((Input.GetButtonDown(button1)) && (weaponOut == false) && cooldownTimer <= 0)
        {
            weaponThrust.transform.Translate(0, thrustAmount, 0);
            weaponOut = true;
            StartCoroutine(WithdrawWeapon());

            cooldownTimer = cooldown;
        }

        cooldownTimer -= Time.deltaTime;
    }


    IEnumerator WithdrawWeapon()
    {
        yield return new WaitForSeconds(0.2f);
        weaponThrust.transform.Translate(0, -thrustAmount, 0);
        weaponOut = false;
    }
}
