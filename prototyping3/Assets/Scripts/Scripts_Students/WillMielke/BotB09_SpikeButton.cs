using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB09_SpikeButton : MonoBehaviour
{
	public GameObject theSpikes;
	public GameObject theButton;
	private float theButtonUpPos;
	public bool isMoving = true;
	public float spikeMoveTime = 1.0f;
	public Transform spikePathStart;
	public Transform spikePathEnd;
	public float SpikeDownDuration;
	public bool stopForever;
	public bool goingUp = false;
	public float MoveDuration;
	private float time;

	void Start()
	{
		time = 0.0f;
		isMoving = true;
		stopForever = false;
		if (theButton != null)
		{
			theButtonUpPos = theButton.transform.position.y;
		}
	}

	void FixedUpdate()
	{
		if (time > MoveDuration)
		{
			if (!goingUp)
			{
				isMoving = false;
				StartCoroutine(SpikeWait());
			}
			goingUp = !goingUp;
			time = 0.0f;

		}

		//move the blade
		if (isMoving && !stopForever)
		{	
			if (!goingUp)
				theSpikes.transform.position = Vector3.Lerp(spikePathStart.position, spikePathEnd.position, time / MoveDuration);
			else
				theSpikes.transform.position = Vector3.Lerp(spikePathEnd.position, spikePathStart.position, time / MoveDuration);

			time += Time.deltaTime;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if ((other.transform.root.gameObject.tag == "Player1") || (other.transform.root.gameObject.tag == "Player2"))
		{
			theButton.transform.position = new Vector3(theButton.transform.position.x, theButtonUpPos - 0.4f, theButton.transform.position.z);
			Renderer buttonRend = theButton.GetComponent<Renderer>();
			buttonRend.material.color = new Color(2.0f, 0.5f, 0.5f, 2.5f);
			theSpikes.transform.position = spikePathEnd.position;
			stopForever = true;

		}
	}

	//public void ButtonUp()
	//{
	//	isMoving = false;
	//	theButton.transform.position = new Vector3(theButton.transform.position.x, theButtonUpPos, theButton.transform.position.z);
	//	Renderer buttonRend = theButton.GetComponent<Renderer>();
	//	buttonRend.material.color = Color.white;
	//}


	IEnumerator SpikeWait()
	{
		yield return new WaitForSeconds(SpikeDownDuration);

		isMoving = true;
	}
}
