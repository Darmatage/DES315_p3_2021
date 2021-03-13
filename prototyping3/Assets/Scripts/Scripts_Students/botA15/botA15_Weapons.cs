using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botA15_Weapons : MonoBehaviour
{
	public GameObject weaponThrust;
	public GameObject wedge;
	public GameObject hammer;
	public AudioClip hammerHitSound;

	private bool swingingHammer = false;
	private bool shootingQuills = false;
	private AudioSource audioSource;

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
			StartCoroutine(shootingDisabled());
        }
    }

	IEnumerator swingingDisabled()
	{
		yield return new WaitForSeconds(1.5f); // wait for animation length until it can be used again

		swingingHammer = false;
	}

	IEnumerator shootingDisabled()
	{
		yield return new WaitForSeconds(3.5f); // wait for animation length until it can be used again

		shootingQuills = false;
	}

	IEnumerator playHammerSound()
    {
		yield return new WaitForSeconds(.10f);
		audioSource.PlayOneShot(hammerHitSound);
    }
}
