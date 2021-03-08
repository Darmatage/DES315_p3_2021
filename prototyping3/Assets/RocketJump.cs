using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketJump : MonoBehaviour
{
    public float Cooldown = 5.0f;
    public float SpinSpeed = 5.0f;
    public float UpStrength = 3000.0f;
    public float SideStrength = 1000.0f;

    public Rigidbody RigidBod = null;

    //grab axis from parent object
    public string button1;
    public string pVertical;
    public string pHorizontal;

    public GameObject RightIndicator;
    public GameObject LeftIndicator;
    public GameObject FrontIndicator;
    public GameObject BackIndicator;

    public Color SelectedColor;
    public Color UsedColor;
    public Color ActiveColor;

    public float CooldownTime = 5.0f;

    float LeftCooldown  = 0.0f;
    float RightCooldown = 0.0f;
    float FrontCooldown = 0.0f;
    float BackCooldown  = 0.0f;

    const int front = 1;
    const int back =  1 << 1;
    const int left =  1 << 2;
    const int right = 1 << 3;

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        pVertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
        pHorizontal = gameObject.transform.parent.GetComponent<playerParent>().rotateAxis;
        RigidBod = GetComponent<Rigidbody>();
    }

    int GetSides (float ForwardAxis, float SideAxis)
    {
        int retVal = 0;
        if (ForwardAxis > 0)
        {
            retVal |= front;
        }
        else if (ForwardAxis < 0)
        {
            retVal |= back;
        }
        if (SideAxis > 0)
        {
            retVal |= right;
        }
        else if (SideAxis < 0)
        {
            retVal |= left;
        }

        return retVal;
    }

    void UpdateColors(float ForwardAxis, float SideAxis)
    {
        RightIndicator.GetComponent<Renderer>().material.SetColor("_Color", ActiveColor);
        LeftIndicator.GetComponent<Renderer>().material.SetColor("_Color", ActiveColor);
        FrontIndicator.GetComponent<Renderer>().material.SetColor("_Color", ActiveColor);
        BackIndicator.GetComponent<Renderer>().material.SetColor("_Color", ActiveColor);

        int BitField = GetSides(ForwardAxis, SideAxis);
        if ((BitField & front) != 0) FrontIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        if ((BitField & back) != 0) BackIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        if ((BitField & left) != 0) LeftIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        if ((BitField & right) != 0) RightIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);

        if (FrontCooldown > 0.0f) FrontIndicator.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.5f * Mathf.Pow((FrontCooldown / CooldownTime), 2.0f)));
        if (BackCooldown > 0.0f) BackIndicator.GetComponent<Renderer>().material.SetColor("_Color",   Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.5f * Mathf.Pow((BackCooldown / CooldownTime), 2.0f)));
        if (LeftCooldown > 0.0f) LeftIndicator.GetComponent<Renderer>().material.SetColor("_Color",   Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.5f * Mathf.Pow((LeftCooldown / CooldownTime), 2.0f)));
        if (RightCooldown > 0.0f) RightIndicator.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.5f * Mathf.Pow((RightCooldown / CooldownTime), 2.0f)));
    }
        // Update is called once per frame
        void Update()
    {
        float FrontAxis = Input.GetAxisRaw(pVertical);
        float SideAxis  = Input.GetAxisRaw(pHorizontal);

        int BitField = GetSides(FrontAxis, SideAxis);

        FrontCooldown -= Time.deltaTime;
        BackCooldown -= Time.deltaTime;
        LeftCooldown -= Time.deltaTime;
        RightCooldown -= Time.deltaTime;

        bool FrontAvailable = ((BitField & front) != 0) && FrontCooldown <= 0.0f;
        bool BackAvailable = ((BitField & back) != 0) && BackCooldown <= 0.0f;
        bool LeftAvailable =  ((BitField & left) != 0)  && LeftCooldown <= 0.0f;
        bool RightAvailable = ((BitField & right) != 0) && RightCooldown <= 0.0f;

        Vector3 JumpVec = new Vector3(0.0f, 0.0f, 0.0f);

        if (FrontAvailable || BackAvailable)
            JumpVec += SideStrength * FrontAxis * transform.forward;
        if (LeftAvailable || RightAvailable)
            JumpVec += SideStrength * SideAxis * transform.right;

        Vector3 AngleVec = new Vector3(0.0f, 0.0f, 0.0f);

        AngleVec += -SideAxis  * SpinSpeed * transform.forward;
        AngleVec += FrontAxis * SpinSpeed * transform.right;

        UpdateColors(FrontAxis, SideAxis);

        // Check for the jump button
        if (Input.GetButtonDown(button1) && JumpVec != new Vector3(0.0f, 0.0f, 0.0f))
        {
            JumpVec += new Vector3(0.0f, 1.0f, 0.0f) * UpStrength;
            RigidBod.AddForce(JumpVec, ForceMode.Impulse);

            RigidBod.angularVelocity = AngleVec;

            if (FrontAxis > 0) FrontCooldown = CooldownTime;
            if (FrontAxis < 0) BackCooldown = CooldownTime;
            if (SideAxis > 0) RightCooldown = CooldownTime;
            if (SideAxis < 0) LeftCooldown = CooldownTime;
        }



        Debug.Log(JumpVec);
    }
}
