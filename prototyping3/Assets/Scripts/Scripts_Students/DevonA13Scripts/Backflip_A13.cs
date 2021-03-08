using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backflip_A13 : MonoBehaviour
{
	public float distanceX;
	public float distanceY;
	public float cooldown;
	public bool flipping = false;
	bool cddone = true;
	float timer = 0.0f;

	//grab axis from parent object
	public string button1;
	public string button2;
	public string button3;
	public string button4; // currently boost in player move script
	public string pVertical;

	void Start()
	{
		button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
		pVertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
		timer = cooldown;
		cooldown = 0.0f;
	}

	// Update is called once per frame
	void Update()
    {
		if (cooldown > 0.0f) 
		{
			cooldown -= Time.deltaTime;
		}

		if ((Input.GetButtonDown(button1)) && cooldown <= 0.0f && gameObject.GetComponent<BotBasic_Move>().isGrounded && !gameObject.GetComponent<BotBasic_Move>().isGrabbed) 
		{
			gameObject.GetComponent<Rigidbody>().AddForce(-Input.GetAxisRaw(pVertical) * new Vector3(distanceX, distanceY, 0));
			cooldown = timer;
			//flipping = true;
		}
    }
}
