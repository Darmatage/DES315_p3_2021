using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB02_Weapon : MonoBehaviour{
    //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

    public GameObject FrontWeapon;
    public GameObject BackWeapon;
    public AudioSource SoundEffect;
	//private float thrustAmount = 3f;
	
	private bool frontWeaponOut = false;
	private bool backWeaponOut = false;
    private bool emergencyEject = true;
    private Rigidbody rb;

	//grab axis from parent object
	public string button1;
	public string button2;
	public string button3;
	public string button4; // currently boost in player move script

    void Start(){
        rb = GetComponent<Rigidbody>();
		button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
    }

    void Update(){
		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button1))&&(frontWeaponOut==false)){
            FrontWeapon.SetActive(true);
            SoundEffect.PlayOneShot(SoundEffect.clip);
            frontWeaponOut = true;
			StartCoroutine(WithdrawFrontWeapon());
        }
        if ((Input.GetButtonDown(button2)) && (backWeaponOut == false))
        {
            BackWeapon.SetActive(true);
            SoundEffect.PlayOneShot(SoundEffect.clip);
            backWeaponOut = true;
            StartCoroutine(WithdrawBackWeapon());
        }
        if(Input.GetButtonDown(button3) && (emergencyEject == true))
        {
            rb.AddForce(rb.centerOfMass + new Vector3(Random.Range(0, 200), Random.Range(140, 200), Random.Range(0, 200)), ForceMode.Impulse);
            StartCoroutine(EmergencyCooldown());
            emergencyEject = false;
        }
    }

	IEnumerator WithdrawFrontWeapon(){
		yield return new WaitForSeconds(0.6f);
        FrontWeapon.SetActive(false);
        frontWeaponOut = false;
	}

    IEnumerator WithdrawBackWeapon()
    {
        yield return new WaitForSeconds(0.6f);
        BackWeapon.SetActive(false);
        backWeaponOut = false;
    }

    IEnumerator EmergencyCooldown()
    {
        yield return new WaitForSeconds(3f);
        emergencyEject = true;
    }
}
