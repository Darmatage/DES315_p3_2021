using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backflip_A13 : MonoBehaviour
{
	//flip
	public float flipdistanceX;
	public float flipdistanceY;
	public float flipcooldown;
	public bool flipping = false;
	//bool cddone = true;
	float timer = 0.0f;

	//dash
	public float dashdistance;
	public float dashcooldown;
	float dashtimer = 0.0f;
	float airdashangle = 30.0f;
	public GameObject lance;

	//bullet
	public GameObject bullet;
	public float bulletcooldown = 1.0f;
	float bullettimer = 0.0f;

	//grab axis from parent object
	public string button1;
	public string button2;
	public string button3;
	public string button4; // currently boost in player move script
	public string pVertical;

	Rigidbody rb;
	void Start()
	{
		button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
		pVertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
		timer = flipcooldown;
		flipcooldown = 0.0f;
		rb = gameObject.GetComponent<Rigidbody>();

		dashtimer = dashcooldown;
        dashcooldown = 0.0f;

		bullettimer = bulletcooldown;
		bulletcooldown = 0.0f;

	}

	// Update is called once per frame
	void Update()
    {


		if (flipcooldown > 0.0f)
		{
			flipcooldown -= Time.deltaTime;
		}

		if (dashcooldown > 0.0f)
		{
			dashcooldown -= Time.deltaTime;
		}

		if (bulletcooldown > 0.0f) 
		{
			bulletcooldown -= Time.deltaTime;
		}

		if ((Input.GetButtonDown(button1)) && bulletcooldown <= 0.0f && !gameObject.GetComponent<BotBasic_Move>().isGrabbed) 
		{
			GameObject bull = Instantiate(bullet, transform.position, transform.rotation);
			bull.GetComponent<A13_Bullet>().playertag = gameObject.tag;
			bulletcooldown = bullettimer;
		}


		if ((Input.GetButtonDown(button2)) && dashcooldown <= 0.0f && !gameObject.GetComponent<BotBasic_Move>().isGrabbed) 
		{
			if (!gameObject.GetComponent<BotBasic_Move>().isGrounded)
			{
				
				transform.Rotate(new Vector3(transform.rotation.x + airdashangle, transform.rotation.y, transform.rotation.z), Space.Self);
			}
			rb.velocity = Vector3.zero;
			rb.AddForce(rb.centerOfMass + gameObject.transform.forward * dashdistance, ForceMode.Impulse);
			dashcooldown = dashtimer;

			//attack
			lance.GetComponent<MeshCollider>().enabled = true;
			StartCoroutine(activatelance());
			gameObject.GetComponent<AudioSource>().Play();
		}

		if ((Input.GetButtonDown(button3)) && flipcooldown <= 0.0f && gameObject.GetComponent<BotBasic_Move>().isGrounded && !gameObject.GetComponent<BotBasic_Move>().isGrabbed) 
		{
			rb.AddForce(rb.centerOfMass + (new Vector3(gameObject.transform.forward.x * -flipdistanceX, flipdistanceY, gameObject.transform.forward.z * -flipdistanceX)), ForceMode.Impulse);
			flipcooldown = timer;
		}
    }

	IEnumerator activatelance()
	{
		yield return new WaitForSeconds(0.3f);
		lance.GetComponent<MeshCollider>().enabled = false;
	}

	IEnumerator backflipanim() 
	{
		yield return new WaitForSeconds(1.0f);

	}
}
