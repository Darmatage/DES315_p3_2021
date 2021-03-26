using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB02_Weapon : MonoBehaviour{
    //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

    public GameObject FrontWeapon;
    public GameObject BackWeapon;
    public GameObject BottomSmoke;
    public AudioSource SoundEffect;
    public AudioSource WoopSoundEffect;
    private float thrustAmount = 2.0f;

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

        BottomSmoke.SetActive(false);
    }

    void Update(){
		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button1))&&(frontWeaponOut==false))
        {
            FrontWeapon.transform.localScale = new Vector3(1, 1, 1);
            FrontWeapon.transform.Translate(0, thrustAmount, 0);
            SoundEffect.PlayOneShot(SoundEffect.clip);
            frontWeaponOut = true;
			StartCoroutine(WithdrawFrontWeapon());
        }
        if ((Input.GetButtonDown(button2)) && (backWeaponOut == false))
        {
            BackWeapon.transform.localScale = new Vector3(1, 1, 1);
            BackWeapon.transform.Translate(0, thrustAmount, 0);
            SoundEffect.PlayOneShot(SoundEffect.clip);
            backWeaponOut = true;
            StartCoroutine(WithdrawBackWeapon());
        }
        if(Input.GetButtonDown(button3) && (emergencyEject == true))
        {
            rb.AddForce(rb.centerOfMass + new Vector3(Random.Range(0, 200), 100, Random.Range(0, 200)), ForceMode.Impulse);
            WoopSoundEffect.Play();
            StartCoroutine(EmergencyCooldown());
            emergencyEject = false;
        }
    }

	IEnumerator WithdrawFrontWeapon(){
		yield return new WaitForSeconds(0.6f);
        FrontWeapon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        FrontWeapon.transform.Translate(0, -thrustAmount, 0);
        frontWeaponOut = false;
	}

    IEnumerator WithdrawBackWeapon()
    {
        yield return new WaitForSeconds(0.6f);
        BackWeapon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        BackWeapon.transform.Translate(0, -thrustAmount, 0);
        backWeaponOut = false;
    }

    IEnumerator EmergencyCooldown()
    {
        BottomSmoke.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        BottomSmoke.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        emergencyEject = true;
    }
}
