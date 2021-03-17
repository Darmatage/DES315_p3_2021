using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBasic_Weapon_DRC : MonoBehaviour{
	//NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

	//public GameObject weaponThrust;
	public GameObject missile;

	public GameObject signifier_on;
	public GameObject signifier_off;

	private float thrustAmount = 3f;
	
	private bool weaponOut = false;
	private float timeOut;

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
			//weaponThrust.transform.Translate(0,thrustAmount, 0);
			weaponOut = true;
			GameObject thisone = Instantiate(missile);
			float origin_dir = GetComponent<Transform>().rotation.eulerAngles.y;

			if (origin_dir < 0.0f)
				origin_dir += 180.0f;

			origin_dir *= Mathf.PI / 180.0f;

			Vector3 place = new Vector3(Mathf.Sin(origin_dir), 0.0f, Mathf.Cos(origin_dir));

			place *= 4.0f;

			GetComponent<AudioSource>().Play();

			thisone.transform.position = transform.position + place; // new Vector3(3.0f, 0.0f, 3.0f);
			thisone.GetComponent<Rigidbody>().velocity = place * 15.0f;

			thisone.transform.rotation = transform.rotation;
			//StartCoroutine(WithdrawWeapon());

			signifier_on.SetActive(false);
			signifier_off.SetActive(true);
		}
		else if(weaponOut == true)
        {
			timeOut += Time.deltaTime;
			if (timeOut > 0.5f && GameObject.Find("Rocket_DRC(Clone)") != null)
			{
				Vector3 test = GameObject.Find("Rocket_DRC(Clone)").transform.position - transform.position;
				if (test.magnitude < 5.0f)
				{
					Destroy(GameObject.Find("Rocket_DRC(Clone)"));
					StartCoroutine(WithdrawWeapon());
				}
			}
		}
    }

    IEnumerator WithdrawWeapon(){
		yield return new WaitForSeconds(0.0f);
		//weaponThrust.transform.Translate(0,-thrustAmount, 0);
		weaponOut = false;
		signifier_on.SetActive(true);
		signifier_off.SetActive(false);
		timeOut = 0.0f;
	}

}
