using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK_SonicBoom : MonoBehaviour
{
	public AudioClip SonicBoomSound;
	public AudioClip FlashKickSound;


	public GameObject projectilePrefab;
	public GameObject flashKick;
	//public AudioClip sonicBoomSound;
	public float SonicCooldown = 0.5f;
	public float projectileSpeed;
	public float thrustAmount = 2.5f;
	private float Sonictimer;
	private float flashTimer;
	public float flashCooldown = 0.5f;

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
		Sonictimer -= Time.deltaTime;
		flashTimer -= Time.deltaTime;

		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button1)) && (Sonictimer <= 0))
		{
			if (GameObject.FindGameObjectWithTag("camP1") != null)
			{
				GetComponent<AudioSource>().clip = SonicBoomSound;
				GetComponent<AudioSource>().Play();
			}
			//AudioSource.PlayClipAtPoint(sonicBoomSound, GameObject.FindGameObjectWithTag("camP1").transform.position);

			Sonictimer = SonicCooldown;

			GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward * 3.5f, transform.rotation);
			projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;

		}
		if ((Input.GetButtonDown(button2)) && (flashTimer <= 0))
		{

			if (GameObject.FindGameObjectWithTag("camP1") != null)
			{
				GetComponent<AudioSource>().clip = FlashKickSound;
				GetComponent<AudioSource>().Play();
			}

			flashTimer = flashCooldown;
			flashKick.transform.Translate(0, 0, thrustAmount);
			flashKick.GetComponent<OK_FlashKick>().weaponOut = true;
			StartCoroutine(WithdrawWeapon());
		}

	}

	IEnumerator WithdrawWeapon()
	{
		yield return new WaitForSeconds(flashCooldown);
		flashKick.transform.Translate(0, 0, -thrustAmount);
		flashKick.GetComponent<OK_FlashKick>().weaponOut = false;
	}
}
