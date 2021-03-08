using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BotB04.Controller
{
	public class BotB04_WeaponHandler : MonoBehaviour
	{
		//NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

		public GameObject weaponThrust;
		private float thrustAmount = 3f;

		private bool weaponOut = false;

		//grab axis from parent object
		string button1;
		string button2;
		string button3;
		string button4; // currently boost in player move script


		public GameObject leftWeapon;
		public GameObject rightWeapon;


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
				weaponThrust.transform.Translate(0, thrustAmount, 0);
				weaponOut = true;
				StartCoroutine(WithdrawWeapon());
			}
		}

		IEnumerator WithdrawWeapon()
		{
			yield return new WaitForSeconds(0.6f);
			weaponThrust.transform.Translate(0, -thrustAmount, 0);
			weaponOut = false;
		}

	}
}