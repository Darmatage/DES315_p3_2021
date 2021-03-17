using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSplincher_Move : MonoBehaviour
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
	//public Collider[] isGrounded;
	public bool isGrounded;
	public bool isTurtled;

	// flip cooldown logic
	public bool canFlip = true;
	// private bool canFlipGate = true;
	// private float flipTimer = 0;
	// public float flipTime = 1f;

	public bool isGrabbed = false;
    public bool isCursorOut = false;

	//grab axis from parent object
	public string parentName;
	public string pVertical;
	public string pHorizontal;
	public string pJump;
	public string button4; // right bumper or [y] or [/] keys, to test on boost

    public MeshRenderer m_arrowF;
    public MeshRenderer m_arrowB;
    public MeshRenderer m_arrowL;
    public MeshRenderer m_arrowR;

    Vector3 m_arrowScale;
	
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

        m_arrowScale = m_arrowF.transform.localScale;
    }

    void Update(){
        isGrabbed = GetComponent<BotBasic_Move>().isGrabbed;

		float botMoveX = Input.GetAxisRaw(pHorizontal) * moveSpeed * Time.deltaTime;
		float botMoveY = Input.GetAxisRaw(pVertical) * moveSpeed * Time.deltaTime;
		
		if (isGrabbed == false && isCursorOut == false)
        {
            //GetComponent<Rigidbody>().velocity += new Vector3(botMoveX, 0, botMoveY);
            transform.Translate(new Vector3(botMoveX, 0, botMoveY), Space.World);
		}

        Color arrowOnColor = new Color(1, 1, 1);
        Color arrowCursorColor = new Color(1, 0.4f, 1);
        Color arrowOffColor = new Color(0.7f, 0.7f, 0);
        Vector3 activeScale = 1.5f * m_arrowScale;
        Vector3 inactiveScale = 1.0f * m_arrowScale;

        if (GetComponent<BotSplincher_Teleport>().m_cursorOut)
        {
            arrowOnColor = arrowCursorColor;
        }

        if (botMoveX > 0)
        {
            m_arrowR.material.color = arrowOnColor;
            m_arrowR.transform.localScale = Vector3.Lerp(m_arrowR.transform.localScale, activeScale, 6.0f * Time.deltaTime);
        }
        else
        {
            m_arrowR.material.color = arrowOffColor;
            m_arrowR.transform.localScale = Vector3.Lerp(m_arrowR.transform.localScale, inactiveScale, 6.0f * Time.deltaTime);
        }

        if (botMoveX < 0)
        {
            m_arrowL.material.color = arrowOnColor;
            m_arrowL.transform.localScale = Vector3.Lerp(m_arrowL.transform.localScale, activeScale, 6.0f * Time.deltaTime);
        }
        else
        {
            m_arrowL.material.color = arrowOffColor;
            m_arrowL.transform.localScale = Vector3.Lerp(m_arrowL.transform.localScale, inactiveScale, 6.0f * Time.deltaTime);
        }

        if (botMoveY > 0)
        {
            m_arrowF.material.color = arrowOnColor;
            m_arrowF.transform.localScale = Vector3.Lerp(m_arrowF.transform.localScale, activeScale, 6.0f * Time.deltaTime);
        }
        else
        {
            m_arrowF.material.color = arrowOffColor;
            m_arrowF.transform.localScale = Vector3.Lerp(m_arrowF.transform.localScale, inactiveScale, 6.0f * Time.deltaTime);
        }

        if (botMoveY < 0)
        {
            m_arrowB.material.color = arrowOnColor;
            m_arrowB.transform.localScale = Vector3.Lerp(m_arrowB.transform.localScale, activeScale, 6.0f * Time.deltaTime);
        }
        else
        {
            m_arrowB.material.color = arrowOffColor;
            m_arrowB.transform.localScale = Vector3.Lerp(m_arrowB.transform.localScale, inactiveScale, 6.0f * Time.deltaTime);
        }

        if (Mathf.Abs(transform.rotation.eulerAngles.x) < 0.01f && Mathf.Abs(transform.rotation.eulerAngles.z) <= 0.01f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        }

        // JUMP
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
		isTurtled = Physics.CheckSphere(turtleCheck.position, 0.4f, groundLayer);
		if (Input.GetButtonDown(pJump)){
			if (isGrounded == true){
				rb.AddForce(rb.centerOfMass + new Vector3(0f, jumpSpeed*10, 0f), ForceMode.Impulse);
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

		// BOOST
		// if (Input.GetButtonDown(button4)){
			// rb.AddForce(transform.forward * boostSpeed, ForceMode.Impulse);
		// }
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