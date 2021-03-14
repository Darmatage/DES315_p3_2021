using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttack : MonoBehaviour
{
	public GameObject ShieldOverloadOBJ;
	Vector3 DefaultScale;
	Vector3 AttackScale = new Vector3(4.4f, 2.4f, 8.4f);
	private bool weaponOut = false;
	

	//grab axis from parent object
	public string button1;
	public string button2;
	public string button3;
	public string button4; // currently boost in player move script

	void Start()
	{
		DefaultScale = ShieldOverloadOBJ.transform.localScale;
		ShieldOverloadOBJ.SetActive(false);

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
			ShieldOverloadOBJ.SetActive(true);
			ShieldOverloadOBJ.transform.localScale = AttackScale;
			weaponOut = true;
			GetComponent<BotDamageMod>().ConsumeForAttack();
			StartCoroutine(WithdrawWeapon());
		}
	}

	IEnumerator WithdrawWeapon()
	{
		yield return new WaitForSeconds(0.6f);
		ShieldOverloadOBJ.transform.localScale = DefaultScale;
		weaponOut = false;
		ShieldOverloadOBJ.SetActive(false);
	}

	void PlaySoundTest()
    {
		GetComponent<AudioSource>().Play();//attach audio clip to the source
    }

}
