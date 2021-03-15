﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot09_Weapon : MonoBehaviour
{
    public GameObject weaponRake;
    public BoxCollider spikes;
    public float weaponDownTime;
    public float weaponRotateTime;
    public AudioSource audioThing;
    public float rotation, rotatdown, rotatreset;
    public ParticleSystem wowy;
    private bool SwingDown, SwingUp;

    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
        SwingDown = SwingUp = false;
        rotation = 0f;
        spikes.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T)){
        if ((Input.GetButtonDown(button1)) && (SwingDown == false) && (SwingUp == false))
        {
            SwingDown = true;
            spikes.enabled = true;

        }

        if (SwingDown && rotation < rotatdown)
        {
            weaponRake.transform.Rotate(Vector3.right * weaponRotateTime);
            rotation += weaponRotateTime;
            if (rotation > rotatdown)
            {
                spikes.enabled = false;
                audioThing.Play();
                wowy.Play();
                StartCoroutine(WithdrawWeapon());
            }
        }

        if(SwingUp && rotation > rotatreset)
        {
            weaponRake.transform.Rotate(-(Vector3.right * weaponRotateTime));
            rotation -= weaponRotateTime; 
            if (rotation < rotatreset)
            {
                // rotation = 0f;
                // transform.rotation = Quaternion.identity;
                SwingUp = false;
            }
        }


    }

    IEnumerator WithdrawWeapon()
    {
        yield return new WaitForSeconds(weaponDownTime);
        SwingDown = false;
        SwingUp = true;
    }

}
