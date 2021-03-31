using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons_ChaseG : MonoBehaviour
{
	//NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot
	// copied BotBasic_Weapon.cs as base for this script

	public GameObject weaponStomp;
	private float thrustAmount = 1.0f;

	public Material ReadyMaterial;
	public Material NotReadyMaterial;

	public GameObject Booster1;
	public GameObject Booster2;
	public GameObject Booster3;
	public GameObject Booster4;

	public float CoolDown;

	private bool weaponOut = false;
	private bool attack = false;
	private float lerpItr = 0;
	private float timer = 0;
	private float CDtimer;

	//grab axis from parent object
	public string button1;
	public string button2;
	public string button3;
	public string button4; // currently boost in player move script

	public ParticleSystem BoosterFL_up;
	public ParticleSystem BoosterBL_up;
	public ParticleSystem BoosterFR_up;
	public ParticleSystem BoosterBR_up;

	public ParticleSystem BoosterFL_down;
	public ParticleSystem BoosterBL_down;
	public ParticleSystem BoosterFR_down;
	public ParticleSystem BoosterBR_down;

	public GameObject AudioSourceUp;
	public GameObject AudioSourceDown;

	void Start()
	{
		button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

		CDtimer = CoolDown;
	}

	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button1)))
		{
			StartAttack();
		}


		Vector3 startPos = Vector3.zero;
		Vector3 endPos = new Vector3(0, -thrustAmount, 0);

		if (attack)
        {
			if (timer < 0.75f)
			{
				timer += Time.deltaTime;
				BoosterFL_down.Stop();
				BoosterBL_down.Stop();
				BoosterFR_down.Stop();
				BoosterBR_down.Stop();
			}
			else
			{
				weaponStomp.transform.localPosition = (Vector3.Lerp(startPos, endPos, lerpItr));
				lerpItr += Time.deltaTime * 10;
				if (lerpItr > 1.0f)
				{
					attack = false;

					Vector3 thrust = gameObject.GetComponentInParent<Transform>().up * -17;
					gameObject.GetComponentInParent<Rigidbody>().velocity = thrust;

					BoosterFL_up.Play();
					BoosterBL_up.Play();
					BoosterFR_up.Play();
					BoosterBR_up.Play();

					AudioSourceDown.GetComponent<AudioSource>().Play();

					Booster1.GetComponent<MeshRenderer>().material = NotReadyMaterial;
					Booster2.GetComponent<MeshRenderer>().material = NotReadyMaterial;
					Booster3.GetComponent<MeshRenderer>().material = NotReadyMaterial;
					Booster4.GetComponent<MeshRenderer>().material = NotReadyMaterial;

					timer = 0;
				}
			}
        }
		else if (lerpItr > 0.0)
        {
			if (timer < 0.5f)
			{
				timer += Time.deltaTime;
			}
			else
			{
				weaponStomp.transform.localPosition = (Vector3.Lerp(startPos, endPos, lerpItr));
				lerpItr -= Time.deltaTime * 10;
				
				weaponStomp.tag = "Untagged";
				BoosterFL_up.Stop();
				BoosterBL_up.Stop();
				BoosterFR_up.Stop();
				BoosterBR_up.Stop();
			}
		}
		else
        {
			timer = 0.0f;
			weaponStomp.transform.localPosition = Vector3.zero;
			weaponOut = false;
		}

		CDtimer += Time.deltaTime;

		if(CDtimer >= CoolDown)
        {
			Booster1.GetComponent<MeshRenderer>().material = ReadyMaterial;
			Booster2.GetComponent<MeshRenderer>().material = ReadyMaterial;
			Booster3.GetComponent<MeshRenderer>().material = ReadyMaterial;
			Booster4.GetComponent<MeshRenderer>().material = ReadyMaterial;
		}
	}

	public void StartAttack()
    {
		if ((weaponOut == false) && CDtimer >= CoolDown)
		{
			CDtimer = 0;

			Vector3 thrust = gameObject.GetComponentInParent<Transform>().up * 15;
			gameObject.GetComponentInParent<Rigidbody>().velocity = thrust;

			//weaponStomp.transform.Translate(0, -thrustAmount, 0);
			weaponOut = true;
			attack = true;
			weaponStomp.tag = "Hazard";

			BoosterFL_down.Play();
			BoosterBL_down.Play();
			BoosterFR_down.Play();
			BoosterBR_down.Play();

			AudioSourceUp.GetComponent<AudioSource>().Play();
		}
	}

	IEnumerator WithdrawWeapon()
	{
		yield return new WaitForSeconds(0.6f);
		weaponStomp.transform.Translate(0, thrustAmount, 0);
		weaponOut = false;
		weaponStomp.tag = "Untagged";
	}
}
