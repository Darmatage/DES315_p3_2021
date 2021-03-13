using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkshatMadanWeapon : MonoBehaviour
{
	public GameObject weaponThrust;
	private float thrustAmount = 2f;
	private float rotateAmt = 0f;
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
		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button1)) && (weaponOut == false))
		{
			rotateAmt = 0.0f;
			weaponThrust.transform.Translate(0, thrustAmount, 0);
			weaponOut = true;
			StartCoroutine(WithdrawWeapon());
		}

		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button2)) && (weaponOut == false))
		{
			rotateAmt = 90.0f;
			weaponThrust.transform.Rotate(0, 0, rotateAmt);
			weaponThrust.transform.Translate(0, thrustAmount, 0);
			weaponOut = true;
			StartCoroutine(WithdrawWeapon());
		}

		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button3)) && (weaponOut == false))
		{
			rotateAmt = 180.0f;
			weaponThrust.transform.Rotate(0, 0, rotateAmt);
			weaponThrust.transform.Translate(0, thrustAmount, 0);
			weaponOut = true;
			StartCoroutine(WithdrawWeapon());
		}

		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button4)) && (weaponOut == false))
		{
			rotateAmt = -90.0f;
			weaponThrust.transform.Rotate(0, 0, rotateAmt);
			weaponThrust.transform.Translate(0, thrustAmount, 0);
			weaponOut = true;
			StartCoroutine(WithdrawWeapon());
		}
	}

	IEnumerator WithdrawWeapon()
	{
		yield return new WaitForSeconds(0.6f);
		weaponThrust.transform.Translate(0, -thrustAmount, 0);
		weaponThrust.transform.Rotate(0, 0, -rotateAmt);
		weaponOut = false;
	}
}
