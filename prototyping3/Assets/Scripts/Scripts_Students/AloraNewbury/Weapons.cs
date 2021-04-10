using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
	//NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

	public GameObject weaponThrust;
	public GameObject weaponThrust_l;
	public GameObject weaponThrust_r;

	private float thrustAmount = 3f;

	private bool weaponOut = false;
	private bool weaponOut_l = false;
	private bool weaponOut_r = false;


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
			//weaponThrust.GetComponent<ParticleSystem>().Play();
			weaponThrust.transform.Translate(0, thrustAmount, 0);
			weaponOut = true;
			StartCoroutine(WithdrawWeapon());
		}
		if ((Input.GetButtonDown(button2)) && (weaponOut_l == false))
		{
			weaponThrust_l.transform.Translate(0, -1.0f, 0);
			weaponOut_l = true;
			StartCoroutine(WithdrawWeapon());
		}
		if ((Input.GetButtonDown(button3)) && (weaponOut_r == false))
		{
			weaponThrust_r.transform.Translate(0, 1.0f, 0);
			weaponOut_r = true;
			StartCoroutine(WithdrawWeapon());
		}


	}

	IEnumerator WithdrawWeapon()
	{
		if (weaponOut)
		{ 
			yield return new WaitForSeconds(0.6f);
			weaponThrust.transform.Translate(0, -thrustAmount, 0);
			weaponOut = false;
		}
		if (weaponOut_l)
		{
			yield return new WaitForSeconds(0.6f);
			weaponThrust_l.transform.Translate(0, 1.0f, 0);
			weaponOut_l = false;
		}
		if (weaponOut_r)
		{
			yield return new WaitForSeconds(0.6f);
			weaponThrust_r.transform.Translate(0, -1.0f, 0);
			weaponOut_r = false;
		}
	}

}
