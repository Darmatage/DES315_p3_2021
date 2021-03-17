using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botA15_Weapons : MonoBehaviour
{
	public GameObject weaponThrust;
	public GameObject wedge;
	public GameObject hammer;
	public AudioClip hammerHitSound;
	public AudioClip quillReleaseSound;
	public GameObject Piston;

	private bool swingingHammer = false;
	private bool shootingQuills = false;
	private AudioSource audioSource;
	private MeshRenderer meshRend;
	private Color originalColor;

	public static float strobeDelay = .15f;
	float strobeDelayTimer = strobeDelay;
	bool toggle = false;
	float detonateTimer = 2.5f; // in seconds




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

		audioSource = GetComponent<AudioSource>();
		meshRend = Piston.GetComponent<MeshRenderer>();
		originalColor = meshRend.material.color;
	}

	void Update()
	{
		 // Hammer hit attack
		if (Input.GetButtonDown(button1) && swingingHammer == false)
        {
			 // Plays the animation and resets immediately so that it can be played multiple times
			hammer.GetComponent<Animator>().Play("hammerHitTest2");
			hammer.GetComponent<Animator>().Play("hammerHitTest2", -1, 0f);

			swingingHammer = true;
			StartCoroutine(playHammerSound());
			StartCoroutine(swingingDisabled()); // cooldown / wait for ani to finish
        }

        if (Input.GetButtonDown(button2) && shootingQuills == false)
        {
            GetComponent<QuillShoot>().ShootQuills();
            shootingQuills = true;
			
			audioSource.PlayOneShot(quillReleaseSound);
			StartCoroutine(shootingDisabled());

			
        }

		if(shootingQuills)
        {
			if (detonateTimer >= 0)
			{
				Debug.Log("I am in the first if statement");
				Strobe();
				detonateTimer -= Time.deltaTime;
			}
			else
				detonateTimer = 2.5f;
		}
    }

	IEnumerator swingingDisabled()
	{
		yield return new WaitForSeconds(1.5f); // wait for animation length until it can be used again

		swingingHammer = false;
	}

	IEnumerator shootingDisabled()
	{
		yield return new WaitForSeconds(2.5f); 

		shootingQuills = false;
		meshRend.material.SetColor("_Color", originalColor);
	}

	IEnumerator playHammerSound()
    {
		yield return new WaitForSeconds(.10f);
		audioSource.PlayOneShot(hammerHitSound);
    }

	private void Strobe()
    {
		Debug.Log("I get in here");
		if (strobeDelayTimer <= 0f)
		{
			Debug.Log("I should be flashing right now");
			strobeDelayTimer = strobeDelay;

			toggle = !toggle;

			if (toggle)
				meshRend.material.SetColor("_Color", Color.red);
			else
				meshRend.material.SetColor("_Color", originalColor);
		}
		else
        {
			Debug.Log("Loop loop");
			strobeDelayTimer -= Time.deltaTime;
        }
	}
}
