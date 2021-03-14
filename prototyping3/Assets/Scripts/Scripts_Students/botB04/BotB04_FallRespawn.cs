using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BotB04.Controller
{
	public class BotB04_FallRespawn : MonoBehaviour
	{
		private Rigidbody rb;
		private GameObject fallRespawn;
		private Transform playerParent;


		// Start is called before the first frame update
		void Start()
		{
			fallRespawn = GameObject.FindWithTag("FallRespawn");
			playerParent = gameObject.transform.parent;

			if (gameObject.GetComponent<Rigidbody>() != null)
			{
				rb = gameObject.GetComponent<Rigidbody>();
			}
		}

		// Update is called once per frame
		void Update()
		{
			if (gameObject.transform.position.y <= fallRespawn.transform.position.y)
			{
				gameObject.transform.position = playerParent.position;
				gameObject.transform.rotation = gameObject.transform.parent.rotation;
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
			}
		}
	}
}