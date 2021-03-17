using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BotB04.Controller
{
	public class BotB04_WeaponHandler : MonoBehaviour
	{
		//NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot


		//grab axis from parent object
		string button1;
		string button2;
		string button3;
		string button4; // currently boost in player move script


		public BotB04_ShieldWeapon LeftWeapon;
		public BotB04_ShieldWeapon RightWeapon;

		private BotB04_ShieldWeapon currWeapon;

		public float CooldownTime = .5f;
		private float cooldown;

		public float recoilStrength = 10;
		public float recoilDuration = 1;
		private float recoilClock;

		void Start()
		{
			LeftWeapon.Initialize();
			RightWeapon.Initialize();

			button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
			button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
			button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
			button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

			BotB04_DamageHandler damageHandler = gameObject.GetComponent<BotB04_DamageHandler>();
			LeftWeapon.shieldStats = damageHandler.shieldRuntime.left;
			RightWeapon.shieldStats = damageHandler.shieldRuntime.right;
			currWeapon = LeftWeapon;

			cooldown = 0;
			recoilClock = 0;
		}

		void Update()
		{
			//if ((Input.GetButtonDown(button1)) && (weaponOut == false))
			//{
			//    weaponThrust.transform.Translate(0, thrustAmount, 0);
			//    weaponOut = true;
			//    StartCoroutine(WithdrawWeapon());
			//}
			cooldown -= Time.deltaTime;
			recoilClock -= Time.deltaTime;

            if (Input.GetButton(button1) && cooldown <= 0)
            {
				if (currWeapon.RequestFire())
                {
					recoilClock = recoilDuration;
                }
					
				cooldown = CooldownTime;
				SwitchWeapon();
			}

			if(recoilClock > 0)
            {
				transform.Translate(0, 0, -1 * recoilStrength * (recoilClock / recoilDuration) * Time.deltaTime);
			}

		}

        private void SwitchWeapon()
        {
			if (currWeapon == LeftWeapon)
				currWeapon = RightWeapon;
			else
				currWeapon = LeftWeapon;
        }
	}
}