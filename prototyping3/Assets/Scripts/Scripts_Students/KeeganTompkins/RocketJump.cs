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

    float LeftCooldown = 0.0f;
    float RightCooldown = 0.0f;
    float FrontCooldown = 0.0f;
    float BackCooldown = 0.0f;

    const int front = 1;
    const int back = 1 << 1;
    const int left = 1 << 2;
    const int right = 1 << 3;

    public AudioClip JumpSound;

    public GameObject ExplosionEffect = null;
    public float ExplosionOffset = -0.2f;

    bool Jumping = false;
    bool Falling = false;

    public float ExtraGravity = 1.0f;
    public float PushDown = 2.0f;

    public Vector3 RotVec;

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        pVertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
        pHorizontal = gameObject.transform.parent.GetComponent<playerParent>().rotateAxis;
        RigidBod = GetComponent<Rigidbody>();
    }

    int GetSides(float ForwardAxis, float SideAxis)
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
        FrontIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = false;
        BackIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = false;
        LeftIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = false;
        RightIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = false;

        int BitField = GetSides(ForwardAxis, SideAxis);
        if ((BitField & front) != 0)
        {
            FrontIndicator.GetComponentInChildren<TextMesh>().text = "Button 1";
            FrontIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            FrontIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        }
        if ((BitField & back) != 0)
        {
            BackIndicator.GetComponentInChildren<TextMesh>().text = "Button 1";
            BackIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            BackIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        }
        if ((BitField & left) != 0) {
            LeftIndicator.GetComponentInChildren<TextMesh>().text = "Button 1";
            LeftIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            LeftIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        }
        if ((BitField & right) != 0) {
            RightIndicator.GetComponentInChildren<TextMesh>().text = "Button 1";
            RightIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            RightIndicator.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        }

        if (FrontCooldown > 0.0f)
        {
            FrontIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            FrontIndicator.GetComponentInChildren<TextMesh>().text = System.Math.Round(FrontCooldown, 2).ToString();
            FrontIndicator.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.25f * Mathf.Pow((FrontCooldown / CooldownTime), 2.0f)));
        }
        if (BackCooldown > 0.0f) {
            BackIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            BackIndicator.GetComponentInChildren<TextMesh>().text = System.Math.Round(BackCooldown, 2).ToString();
            BackIndicator.GetComponent<Renderer>().material.SetColor("_Color",   Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.25f * Mathf.Pow((BackCooldown / CooldownTime), 2.0f)));
        }
        if (LeftCooldown > 0.0f) {
            LeftIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            LeftIndicator.GetComponentInChildren<TextMesh>().text = System.Math.Round(LeftCooldown, 2).ToString();
            LeftIndicator.GetComponent<Renderer>().material.SetColor("_Color",   Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.25f * Mathf.Pow((LeftCooldown / CooldownTime), 2.0f)));
        }
        if (RightCooldown > 0.0f){
            RightIndicator.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
            RightIndicator.GetComponentInChildren<TextMesh>().text = System.Math.Round(RightCooldown, 2).ToString();
            RightIndicator.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(ActiveColor, UsedColor, 0.5f + 0.25f * Mathf.Pow((RightCooldown / CooldownTime), 2.0f)));
        }
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

        if (Jumping || Falling)
        {
            GetComponent<BotBasic_Move>().enabled = false;
        }
        else
        {
            GetComponent<BotBasic_Move>().enabled = true;
        }

        if (Jumping)
        {
            RigidBod.velocity -= new Vector3(0.0f, Time.deltaTime * ExtraGravity, 0.0f);

            RigidBod.angularVelocity = RotVec;

            if (RigidBod.velocity.y < 0.0f)
            {
                Falling = true;
                Jumping = false;
            }
        }
        else if (Falling)
        {


            Vector3 newRot = transform.rotation.eulerAngles;
            newRot.x = 0.0f;
            newRot.z = 0.0f;

            transform.rotation = Quaternion.Euler(newRot);
            RigidBod.velocity -= new Vector3(0.0f, PushDown * Time.deltaTime, 0.0f);

            var a = GetComponent<BotBasic_Move>();
            if (a && Physics.CheckSphere(a.groundCheck.position, 0.4f, a.groundLayer))
            {
                Falling = false;
            }
        }

        if (FrontAvailable || BackAvailable)
            JumpVec += SideStrength * FrontAxis * transform.forward;
        if (LeftAvailable || RightAvailable)
            JumpVec += SideStrength * SideAxis * transform.right;

        Vector3 AngleVec = new Vector3(0.0f, 0.0f, 0.0f);

        AngleVec += -SideAxis * transform.forward;
        AngleVec += FrontAxis * transform.right;

        UpdateColors(FrontAxis, SideAxis);

        // Check for the jump button
        if (Input.GetButtonDown(button1) && JumpVec != new Vector3(0.0f, 0.0f, 0.0f))
        {
            GetComponent<AudioSource>().PlayOneShot(JumpSound);

            Jumping = true;
            Falling = false;
            RigidBod.velocity = Vector3.zero;

            transform.position += new Vector3(0.0f, 0.1f, 0.0f);

            if (GetComponent<LotsaShot>() != null) GetComponent<LotsaShot>().Flip();

            JumpVec += new Vector3(0.0f, 1.0f, 0.0f) * UpStrength;
            RigidBod.velocity = JumpVec;

            RotVec = AngleVec * SpinSpeed;

            if (FrontAxis > 0)
            {
                FrontCooldown = CooldownTime;
                var Explosion = Instantiate(ExplosionEffect);
                Explosion.transform.position = FrontIndicator.transform.position - Vector3.up * ExplosionOffset;
            }
            else if (FrontAxis < 0)
            {
                BackCooldown = CooldownTime;
                var Explosion = Instantiate(ExplosionEffect);
                Explosion.transform.position = BackIndicator.transform.position - Vector3.up * ExplosionOffset;

            }
            if (SideAxis > 0)
            {
                RightCooldown = CooldownTime;
                var Explosion = Instantiate(ExplosionEffect);
                Explosion.transform.position = RightIndicator.transform.position - Vector3.up * ExplosionOffset;
            }
            else if (SideAxis < 0)
            {
                LeftCooldown = CooldownTime;
                var Explosion = Instantiate(ExplosionEffect);
                Explosion.transform.position = LeftIndicator.transform.position - Vector3.up * ExplosionOffset;
            }
        }
    }
}
