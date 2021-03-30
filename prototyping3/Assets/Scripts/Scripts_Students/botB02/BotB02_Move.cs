﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB02_Move : MonoBehaviour
{
	public float moveSpeed = 10;
	public float rotateSpeed = 100;
    public float rightingSpeed = 3f;
	public float jumpSpeed = 7f;
	private float flipSpeed = 150f;
	public float boostSpeed = 10f;
    
    private Quaternion desired;

    private Rigidbody rb;
	public Transform groundCheck;
	public Transform turtleCheck;
	public LayerMask groundLayer;
	//public Collider[] isGrounded;
	public bool isGrounded;
	public bool isTurtled;

	// flip cooldown logic
	public bool canFlip = true;
	// private bool canFlipGate = true;
	// private float flipTimer = 0;
	// public float flipTime = 1f;

	public bool isGrabbed = false;

	//grab axis from parent object
	public string parentName;
	public string pVertical;
	public string pHorizontal;
	public string pJump;
	public string button4; // right bumper or [y] or [/] keys, to test on boost
	
    void Start(){
		if (gameObject.GetComponent<Rigidbody>() != null){
			rb = gameObject.GetComponent<Rigidbody>();
		}

		parentName = this.transform.parent.gameObject.name;
		pVertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
		pHorizontal = gameObject.transform.parent.GetComponent<playerParent>().rotateAxis;
		pJump = gameObject.transform.parent.GetComponent<playerParent>().jumpInput;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
		
		// pVertical = "p1Vertical";
		// pHorizontal = "p1Horizontal";
		// pJump = "p1Jump";
		// button4 = "p1Fire4";
    }

    void Update(){
		float botMove = Input.GetAxisRaw(pVertical) * moveSpeed * Time.deltaTime;
		float botRotate = Input.GetAxisRaw(pHorizontal) * rotateSpeed * Time.deltaTime;
		
		if (isGrabbed == false){
			transform.Translate(0,0, botMove);
			transform.Rotate(0, botRotate, 0);
		}
        
		// JUMP
		isGrounded = Physics.CheckSphere(groundCheck.position, 1.7f, groundLayer);
		isTurtled = Physics.CheckSphere(turtleCheck.position, 0.4f, groundLayer);

        desired = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

        if(isGrounded)
        {
            gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, desired, Time.deltaTime * rightingSpeed);
        }


        if (Input.GetButtonDown(pJump)){
			if (isGrounded == true){
				rb.AddForce(rb.centerOfMass + new Vector3(0f, jumpSpeed*10, 0f) + (transform.forward * jumpSpeed * 10) , ForceMode.Impulse);
			}
			
			//flip cooldown logic
			// if ((isTurtled == true) && (canFlip == false)){
				// canFlipGate = false;	
			// }
			
			if ((isTurtled == true) && (canFlip == true)){
				rb.AddForce(rb.centerOfMass + new Vector3(jumpSpeed / 2, 0, jumpSpeed / 2), ForceMode.Impulse);
				transform.Rotate(flipSpeed, 0, 0);
				// canFlip = false;
				// canFlipGate = true;
			}

			else if (canFlip == true){
				Vector3 betterEulerAngles = new Vector3(gameObject.transform.parent.eulerAngles.x, transform.eulerAngles.y, gameObject.transform.parent.eulerAngles.z); 
				transform.eulerAngles = betterEulerAngles;
			}
		}
        if(rb.velocity.y > 7)
        {
            rb.velocity.Set(rb.velocity.x, 10, rb.velocity.z);
        }
		// BOOST
		// if (Input.GetButtonDown(button4)){
			// rb.AddForce(transform.forward * boostSpeed, ForceMode.Impulse);
		// }
    }

    private void LateUpdate()
    {
        
    }

    //Flip cooldown timer
    // void FixedUpdate(){
    // if (canFlipGate == false){
    // flipTimer += 1f;
    // if (flipTimer >= flipTime){
    // canFlip = true;
    // flipTimer = 0f;
    // }
    // }
    // }

}



	//isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);
	//rb.transform.Rotate(Vector3(flipSpeed,0,0));
	//rb.AddTorque.transform.Rotate(Vector3(flipSpeed,0,0), ForceMode.Impulse);
	//else gameObject.transform.rotation = gameObject.transform.parent.rotation;
	//else transform.rotation = Quaternion.Euler(90, transform.eulerAngle.y, 90);
	//else  transform.Rotate (new Vector3 (90, gameObject.eulerAngle.y, 90));