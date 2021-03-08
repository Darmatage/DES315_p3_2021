using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot05_Move : MonoBehaviour
{
    protected enum STATE
    {
        S_RECOVERING, // bot cannot move, is in recovery phase of attack
        S_AIMING,     // bot can only turn in place in order to aim
        S_ATTACKING,  // bot cannot be player controlled
        S_NORMAL      // bot can be player controlled
    }
    protected STATE cur_state;

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

    //grab axis from parent object
    public string parentName;
    public string pVertical;
    public string pHorizontal;
    public string pJump;
    public string button4; // right bumper or [y] or [/] keys, to test on boost



    void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        parentName = this.transform.parent.gameObject.name;
        pVertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
        pHorizontal = gameObject.transform.parent.GetComponent<playerParent>().rotateAxis;
        pJump = gameObject.transform.parent.GetComponent<playerParent>().jumpInput;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        cur_state = STATE.S_NORMAL;

        // pVertical = "p1Vertical";
        // pHorizontal = "p1Horizontal";
        // pJump = "p1Jump";
        // button4 = "p1Fire4";
    }

    void Update()
    {
        float botMove = Input.GetAxisRaw(pVertical) * moveSpeed * Time.deltaTime;
        float botRotate = Input.GetAxisRaw(pHorizontal) * rotateSpeed * Time.deltaTime;

        if (isGrabbed == false)
        {
            if (cur_state == STATE.S_NORMAL)
                transform.Translate(0, 0, botMove);

            if (cur_state == STATE.S_NORMAL || cur_state == STATE.S_AIMING)
            transform.Rotate(0, botRotate, 0);

            // attacking blade rush
            if (cur_state == STATE.S_ATTACKING)
                transform.Translate(0, 0, 2.0f * moveSpeed * Time.deltaTime);
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
                // canFlip = false;
                // canFlipGate = true;
            }

            else if (canFlip == true)
            {
                Vector3 betterEulerAngles = new Vector3(gameObject.transform.parent.eulerAngles.x, transform.eulerAngles.y, gameObject.transform.parent.eulerAngles.z);
                transform.eulerAngles = betterEulerAngles;
            }
        }

        // BOOST
        // if (Input.GetButtonDown(button4)){
        // rb.AddForce(transform.forward * boostSpeed, ForceMode.Impulse);
        // }
    }

    public void SetAimingState()
    {
        cur_state = STATE.S_AIMING;
    }

    public void SetRecoveringState()
    {
        cur_state = STATE.S_RECOVERING;
    }

    public void SetAttackState()
    {
        cur_state = STATE.S_ATTACKING;
    }

    public void SetNormalState()
    {
        cur_state = STATE.S_NORMAL;
    }
}
