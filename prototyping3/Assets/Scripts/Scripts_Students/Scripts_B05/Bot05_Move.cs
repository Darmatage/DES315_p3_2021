using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot05_Move : MonoBehaviour
{
    public enum STATE
    {
        RECOVERING, // bot cannot move, is in recovery phase of attack
        AIMING,     // bot can only turn in place in order to aim
        ATTACKING,  // bot cannot be player controlled, rushes forward
        ATTRACTING, // bot cannot be player controlled, stays in place
        REPELING,   // bot cannot be player controlled, stays in place
        JUMPING,    // bot can only rotate
        NORMAL      // bot can be player controlled
    }
    protected STATE cur_state;

    public Transform center_pt;

    // normal values
    public float moveSpeed = 10;
    public float rotateSpeed = 100;
    public float jumpSpeed = 33.0f;//7f;
    private float flipSpeed = 150f;
    public float boostSpeed = 10f;

    // rush values
    private float rushSpeed = 20;
    private float rushRotate = 400;
    private Vector3 rushDir = Vector3.zero;

    // aim values
    private float aimRotateMod = 1.1f;
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

    public B05_GroundPound a_pound;

    public B05_Camera cam;

    public Material eye1;
    public Material eye2;
    public MeshRenderer eyeball;

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

        if (gameObject.transform.root.tag == "Player1")
        {
            gameObject.layer = 15;
            eyeball.material = eye1;
        }
        else if(gameObject.transform.root.tag == "Player2")
        {
            gameObject.layer = 18;
            eyeball.material = eye2;
        }

        cur_state = STATE.NORMAL;
    }

    void Update()
    {
        float botMove = Input.GetAxisRaw(pVertical) * moveSpeed * Time.deltaTime;
        float botRotate = Input.GetAxisRaw(pHorizontal) * rotateSpeed * Time.deltaTime;

        if (isGrabbed == false)
        {
            switch (cur_state)
            {
                case STATE.NORMAL:
                    transform.Translate(0, 0, botMove);
                    transform.Rotate(0, botRotate, 0);
                    break;
                case STATE.ATTACKING:
                    transform.Translate(transform.InverseTransformDirection(rushDir.x * rushSpeed * Time.deltaTime, 0.0f, rushDir.z * rushSpeed * Time.deltaTime));
                    transform.Rotate(0, rushRotate * Time.deltaTime, 0);
                    break;
                case STATE.AIMING:
                    transform.Rotate(0, botRotate * aimRotateMod, 0);
                    break;
                case STATE.ATTRACTING:
                    break;
                case STATE.REPELING:
                    break;
                case STATE.RECOVERING:
                    break;
                case STATE.JUMPING:
                    transform.Rotate(0, botRotate * aimRotateMod, 0);
                    break;
                default:
                    break;
            }
        }

        // JUMP
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        isTurtled = Physics.CheckSphere(turtleCheck.position, 0.4f, groundLayer);
        if (Input.GetButtonDown(pJump))
        {
            if (isGrounded == true)
            {
                if (Mathf.Abs(transform.rotation.eulerAngles.x) < 1.0f &&
                    Mathf.Abs(transform.rotation.eulerAngles.z) < 1.0f)
                {
                    rb.AddForce(rb.centerOfMass + new Vector3(0f, jumpSpeed * 10, 0f), ForceMode.Impulse);
                    if (IsState(STATE.NORMAL))
                    {
                        a_pound.Activate();
                    }
                }
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

    public void SetState(STATE state)
    {
        cur_state = state;

        cam.SetPos((int)state);

        if (state == STATE.ATTACKING)
            rushDir = transform.forward;
    }

    public bool IsState(STATE state)
    {
        return cur_state == state;
    }

    public Transform GetCenter()
    {
        return center_pt;
    }
}
