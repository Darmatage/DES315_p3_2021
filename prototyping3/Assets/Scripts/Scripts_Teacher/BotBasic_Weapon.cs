﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBasic_Weapon : MonoBehaviour{
    //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

	public GameObject weaponThrust;
	private float thrustAmount = 3f;
	
	private bool weaponOut = false;

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
    }

    void Update(){
		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button1))&&(weaponOut==false)){
			weaponThrust.transform.Translate(0,thrustAmount, 0);
			weaponOut = true;
			StartCoroutine(WithdrawWeapon());
		}
    }

	IEnumerator WithdrawWeapon(){
		yield return new WaitForSeconds(0.6f);
		weaponThrust.transform.Translate(0,-thrustAmount, 0);
		weaponOut = false;
	}

}
