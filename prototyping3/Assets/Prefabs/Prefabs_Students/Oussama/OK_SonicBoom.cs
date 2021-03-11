using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK_SonicBoom : MonoBehaviour
{
	public GameObject projectilePrefab;
	//public AudioClip sonicBoomSound;
	public float Cooldown = 0.5f;
	public float projectileSpeed;
	private float timer;

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
		timer -= Time.deltaTime;
		//if (Input.GetKeyDown(KeyCode.T)){
		if ((Input.GetButtonDown(button1)) && (timer <= 0))
		{
			if (GameObject.FindGameObjectWithTag("camP1") != null)
				GetComponent<AudioSource>().Play(); 
				//AudioSource.PlayClipAtPoint(sonicBoomSound, GameObject.FindGameObjectWithTag("camP1").transform.position);

			timer = Cooldown;

			GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward *3.0f, transform.rotation);
			projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;

		}
	}

}
