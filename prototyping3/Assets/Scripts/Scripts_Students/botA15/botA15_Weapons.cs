using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botA15_Weapons : MonoBehaviour
{
	public GameObject weaponThrust;
	public GameObject wedge;
	public GameObject hammer;

	private float thrustAmount = 3f;

	private bool weaponOut = false;
	private bool swingingHammer = false;

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
		//if ((Input.GetButtonDown(button1)) && (weaponOut == false))
		//{
		//	weaponThrust.transform.Translate(0, thrustAmount, 0);
		//	weaponOut = true;
		//	StartCoroutine(WithdrawWeapon());
		//}

		//hammer.GetComponent<Animator>().


		if (Input.GetButtonDown(button1) && swingingHammer == false)
        {
			Debug.Log("Playing Ani");
			hammer.GetComponent<Animator>().Play("hammerHitTest2");
			hammer.GetComponent<Animator>().Play("hammerHitTest2", -1, 0f);

			swingingHammer = true;

			StartCoroutine(swingingDisabled());
        }
	}

	IEnumerator WithdrawWeapon()
	{
		yield return new WaitForSeconds(0.6f);
		weaponThrust.transform.Translate(0, -thrustAmount, 0);
		weaponOut = false;
	}

	IEnumerator swingingDisabled()
	{
		yield return new WaitForSeconds(1.5f);
		//weaponThrust.transform.Translate(0, -thrustAmount, 0);
		swingingHammer = false;
	}
}
