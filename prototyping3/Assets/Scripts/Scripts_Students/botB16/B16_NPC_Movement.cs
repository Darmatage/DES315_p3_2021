﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B16_NPC_Movement : MonoBehaviour
{
	public float moveSpeed = 10;
	public float rotateSpeed = 100;
	public float jumpSpeed = 7f;
	private float flipSpeed = 150f;
	public float boostSpeed = 10f;

	private Rigidbody rb;
	public Transform groundCheck;
	public Transform turtleCheck;
	public LayerMask groundLayer;
	public bool isGrounded;
	public bool isTurtled;

	// flip cooldown logic
	public bool canFlip = true;

	public bool isGrabbed = false;

	//grab axis from parent object
	public string parentName;
	public string pVertical;
	public string pHorizontal;
	public string pJump;

	private GameObject enemy;
	private NavMeshAgent agent;
	private Rigidbody rigidbody;

	private Weapons_ChaseG weapon;

	void Start()
	{
		if (gameObject.GetComponent<Rigidbody>() != null)
		{
			rb = gameObject.GetComponent<Rigidbody>();
		}

		weapon = GetComponent<Weapons_ChaseG>();

		parentName = this.transform.parent.gameObject.name;
		pVertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
		pHorizontal = gameObject.transform.parent.GetComponent<playerParent>().rotateAxis;
		pJump = gameObject.transform.parent.GetComponent<playerParent>().jumpInput;

		enemy = GetEnemy();
		agent = GetComponent<NavMeshAgent>();
		rigidbody = GetComponentInParent<Rigidbody>();
	}

	void Update()
	{
		float botMove = Input.GetAxisRaw(pVertical) * moveSpeed * Time.deltaTime;
		float botRotate = Input.GetAxisRaw(pHorizontal) * rotateSpeed * Time.deltaTime;
		
		if (enemy)
        {
			Vector3 destination = enemy.transform.position;
			destination.y = transform.position.y;

			if (Vector3.Distance(destination, transform.position) < 10 && weapon.Ready())
            {
				isGrounded = false;
				agent.enabled = false;
				weapon.StartAttack();
            }
            else if(weapon.Done())
            {
				agent.enabled = true;
				agent.destination = destination;
			}
			else
            {
				transform.LookAt(destination);
				Vector3 velocity = transform.forward * moveSpeed;
				velocity.y = rigidbody.velocity.y;
				
				rigidbody.velocity = velocity;
			}
        }


		if (isGrabbed == false)
		{
			transform.Translate(0, 0, botMove);
			transform.Rotate(0, botRotate, 0);
		}

		// JUMP
		isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
		isTurtled = Physics.CheckSphere(turtleCheck.position, 0.4f, groundLayer);
		if (Input.GetButtonDown(pJump))
		{
			if (isGrounded == true)
			{
				rb.AddForce(rb.centerOfMass + new Vector3(0f, jumpSpeed * 10, 0f), ForceMode.Impulse);
			}

			//flip cooldown logic
			// if ((isTurtled == true) && (canFlip == false)){
			// canFlipGate = false;	
			// }

			if ((isTurtled == true) && (canFlip == true))
			{
				rb.AddForce(rb.centerOfMass + new Vector3(jumpSpeed / 2, 0, jumpSpeed / 2), ForceMode.Impulse);
				transform.Rotate(flipSpeed, 0, 0);
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				// canFlip = false;
				// canFlipGate = true;
			}

			else if (canFlip == true)
			{
				Vector3 betterEulerAngles = new Vector3(gameObject.transform.parent.eulerAngles.x, transform.eulerAngles.y, gameObject.transform.parent.eulerAngles.z);
				transform.eulerAngles = betterEulerAngles;
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			}
		}
	}

	private GameObject GetEnemy()
    {
		GameHandler handler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		string playerTag = transform.root.tag;
		if (playerTag == "Player1")
        {
			return handler.Player2Holder.transform.GetChild(0).gameObject;
		}
		else if(playerTag == "Player2")
        {
			return handler.Player1Holder.transform.GetChild(0).gameObject;
		}
		else
        {
			return null;
        }
	}
}
