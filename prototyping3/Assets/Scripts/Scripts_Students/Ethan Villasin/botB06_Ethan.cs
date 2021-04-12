using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botB06_Ethan : MonoBehaviour
{
    //vars pulled from teacher
    public GameObject compassSides;
    public GameObject compassVertical;
    private float sidelimit = 1.0f;
    private float attackDamage;
    public float knockBackSpeed = 10f;

    //public float shieldPowerFront = 3f;
    public float shieldPowerBack = 3f;
    public float shieldPowerLeft = 3f;
    public float shieldPowerRight = 3f;
    public float shieldPowerTop = 3f;
    public float shieldPowerBottom = 3f;

    private Rigidbody rb;
    private GameHandler gameHandler;
    private string thisPlayer;
    private bool notMyWeapon = true;

    //Shields
    public GameObject TopShield, BottomShield;//, LeftShield, RightShield, BackShield;
    //particles
    public GameObject TopParticles, BottomParticles;//, LeftParticles, RightParticles, BackParticles;

    //Weapon Vars
    public GameObject FrontWeapon;
    //public GameObject[] Weapons; //Weapon Ordering Front, FrontLeft, Left, BackLeft, Back, BackRight, Right, FrontRight
    private float thrustAmount = 1.8f;

    private bool weaponOut = false;

    public GameObject Beyblade;
    Quaternion origRotation;
    int Spin = 0;
    float spinTimer = 0;
    float dashspeed = 200;
    float spinCD = 3, dashCD = 5, spincooldown, dashcooldown;
    int spinCount = 5;
    //private Vector3 weaponScale = new Vector3(0.45f, 1, .45f);

    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script

    public GameObject[] spincounters;
    public GameObject dashcounter;

    private AudioSource audioSrc;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            rb = gameObject.GetComponent<Rigidbody>();
            print("Found the RigidBody");
        }
        if (GameObject.FindWithTag("GameHandler") != null)
        {
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
            print("Found the GameHandler");
        }

        if(spincounters.Length == 0)
        {
            spincounters = GameObject.FindGameObjectsWithTag("spincounter");
        }
        if(!dashcounter)
        {
            dashcounter = GameObject.FindGameObjectWithTag("dashcounter");
        }
        thisPlayer = gameObject.transform.root.tag;

        TopShield.SetActive(false);
        BottomShield.SetActive(false);
		//LeftShield.SetActive(false);
		//RightShield.SetActive(false);
		//BackShield.SetActive(false);

        TopParticles.SetActive(false);
        BottomParticles.SetActive(false);
		//LeftParticles.SetActive(false);
		//RightParticles.SetActive(false);
		//BackParticles.SetActive(false);

        //weapon buttons
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        origRotation = Beyblade.transform.localRotation;

        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = .2f;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T)){
        if ((Input.GetButtonDown(button1)) && Spin == 0 && spinCount > 0)
        {
            Spin = 1;
            audioSrc.PlayOneShot(audioSrc.clip);
            spinCount--;
            spincooldown = spinCD;
            spincounters[spinCount].SetActive(false);
        }
        if ((Input.GetButtonDown(button2)) && dashcooldown <= 0)
        {
            //dash
            rb.AddForce(transform.forward * dashspeed, ForceMode.Impulse);
            dashcooldown = dashCD;
            dashcounter.SetActive(false);
            //StartCoroutine(WithdrawWeapon());
        }


        if(spincooldown > 0)
        {
            spincooldown -= Time.deltaTime;
        }
        if(dashcooldown > 0)
        {
            dashcooldown -= Time.deltaTime;
        }
        if(spinCount < 5 && spincooldown <= 0)
        {
            if (spinCount != 5)
                spincooldown = spinCD;
            else
                spincooldown = 0;
            spinCount++;
            spincounters[spinCount - 1].SetActive(true);
        }
        if(dashcooldown <= 0)
        {
            dashcooldown = 0;
            dashcounter.SetActive(true);
        }
        if(Spin == 1 && spinTimer <= 0)
        {
            spinTimer = 0.5f;
            Spin = 2;
        }
        if(spinTimer > 0 && Spin == 2)
        {
            Beyblade.transform.Rotate(new Vector3(0, 0, 20));
            spinTimer -= Time.deltaTime;
        }
        else if(Spin == 2 && spinTimer <= 0)
        {
            Beyblade.transform.localRotation = origRotation;
            Spin = 0;
        }
    }

    IEnumerator WithdrawWeapon()
    {
        yield return new WaitForSeconds(0.6f);

        FrontWeapon.transform.Translate(0, thrustAmount, 0);
        weaponOut = false;
    }

    //taking Damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hazard")
        {
            if ((other.gameObject.GetComponent<HazardDamage>().isPlayer1Weapon == false) && (thisPlayer == "Player1")) { notMyWeapon = true; }
            else if ((other.gameObject.GetComponent<HazardDamage>().isPlayer2Weapon == false) && (thisPlayer == "Player2")) { notMyWeapon = true; }
            else if ((other.gameObject.GetComponent<HazardDamage>().isMonsterWeapon == false)) { notMyWeapon = true; }
            else { notMyWeapon = false; }
        }

        if ((other.gameObject.tag == "Hazard") && (notMyWeapon == true))
        {
            attackDamage = other.gameObject.GetComponent<HazardDamage>().damage;

            Vector3 directionFore = other.transform.position - transform.position;
            Vector3 directionSides = other.transform.position - compassSides.transform.position;
            Vector3 directionVert = other.transform.position - compassVertical.transform.position;

            if (Vector3.Dot(transform.forward, directionFore) < (-sidelimit))
            {
                rb.AddForce(transform.forward * knockBackSpeed * -1, ForceMode.Impulse);
                //Debug.Log("HitBack " + Vector3.Dot(transform.forward, directionFore));
                //if (shieldPowerBack <= 0)
                //{
                //    BackParticles.SetActive(true);
                //    //string playerDamaged = gameObject.tag; //remove for final;
                //    //gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
                    gameHandler.TakeDamage(thisPlayer, attackDamage);  //use in final (slotted players)
                //}
                //else
                //{
                //    shieldPowerBack -= attackDamage;
                //    StartCoroutine(ShieldHitDisplay(BackShield));
                //    if (shieldPowerBack <= 0)
                //    {
                //        shieldPowerBack = 0;
                //        //string playerDamaged = gameObject.tag; //remove for final;
                //        //gameHandler.PlayerShields(playerDamaged, "Back"); //remove for final;
                //        gameHandler.PlayerShields(thisPlayer, "Back");  //use in final (slotted players)
                //    }
                //}
            }

            if (Vector3.Dot(transform.forward, directionFore) > sidelimit)
            {
                rb.AddForce(transform.forward * knockBackSpeed, ForceMode.Impulse);
                //Debug.Log("HitFront "+ Vector3.Dot (transform.forward, directionFore));
                //if (shieldPowerFront <= 0)
                //{
                //	dmgParticlesFront.SetActive(true);
                //	//string playerDamaged = gameObject.tag; //remove for final;
                //	//Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
                //	//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
                	gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
                //}
                //else
                //{
                //	shieldPowerFront -= attackDamage;
                //	StartCoroutine(ShieldHitDisplay(shieldFrontObj));
                //	if (shieldPowerFront <= 0)
                //	{
                //		shieldPowerFront = 0;
                //		//string playerDamaged = gameObject.tag; //remove for final;
                //		//gameHandler.PlayerShields(playerDamaged, "Front"); //remove for final;
                //		gameHandler.PlayerShields(thisPlayer, "Front");  //use in final (slotted players)
                //	}
                //}
            }

            if (Vector3.Dot(compassSides.transform.forward, directionSides) > sidelimit)
            {
                rb.AddForce(transform.right * knockBackSpeed, ForceMode.Impulse);
                //Debug.Log("HitRight " + Vector3.Dot (compassSides.transform.forward, directionSides));
                //if (shieldPowerRight <= 0)
                //{
                //    RightParticles.SetActive(true);
                //    //string playerDamaged = gameObject.tag; //remove for final;
                //    //Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
                //    //gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
                    gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
                //}
                //else
                //{
                //    shieldPowerRight -= attackDamage;
                //    StartCoroutine(ShieldHitDisplay(RightShield));
                //    if (shieldPowerRight <= 0)
                //    {
                //        shieldPowerRight = 0;
                //        //string playerDamaged = gameObject.tag; //remove for final;
                //        //gameHandler.PlayerShields(playerDamaged, "Right"); //remove for final;
                //        gameHandler.PlayerShields(thisPlayer, "Right");  //use in final (slotted players)
                //    }
                //}
            }

            if (Vector3.Dot(compassSides.transform.forward, directionSides) < (-sidelimit))
            {
                rb.AddForce(transform.right * knockBackSpeed * -1, ForceMode.Impulse);
                //Debug.Log("HitLeft " + Vector3.Dot (compassSides.transform.forward, directionSides));
                //if (shieldPowerLeft <= 0)
                //{
                //    LeftParticles.SetActive(true);
                //    //string playerDamaged = gameObject.tag; //remove for final;
                //    //Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
                //    //gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
                    gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
                //}
                //else
                //{
                //    shieldPowerLeft -= attackDamage;
                //    StartCoroutine(ShieldHitDisplay(LeftShield));
                //    if (shieldPowerLeft <= 0)
                //    {
                //        shieldPowerLeft = 0;
                //        //string playerDamaged = gameObject.tag; //remove for final;
                //        //gameHandler.PlayerShields(playerDamaged, "Left"); //remove for final;
                //        gameHandler.PlayerShields(thisPlayer, "Left");  //use in final (slotted players)
                //    }
                //}
            }

            if (Vector3.Dot(compassVertical.transform.forward, directionVert) > sidelimit)
            {
                rb.AddForce(transform.up * knockBackSpeed, ForceMode.Impulse);
                //Debug.Log("HitTop " + Vector3.Dot (compassVertical.transform.forward, directionVert));
                if (shieldPowerTop <= 0)
                {
                    TopParticles.SetActive(true);
                    //string playerDamaged = gameObject.tag; //remove for final;
                    //Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
                    //gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
                    gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
                }
                else
                {
                    shieldPowerTop -= attackDamage;
                    StartCoroutine(ShieldHitDisplay(TopShield));
                    if (shieldPowerTop <= 0)
                    {
                        shieldPowerTop = 0;
                        //string playerDamaged = gameObject.tag; //remove for final;
                        //gameHandler.PlayerShields(playerDamaged, "Top"); //remove for final;
                        gameHandler.PlayerShields(thisPlayer, "Top");  //use in final (slotted players)
                    }
                }
            }

            if (Vector3.Dot(compassVertical.transform.forward, directionVert) < (-sidelimit))
            {
                rb.AddForce(transform.up * knockBackSpeed * -1, ForceMode.Impulse);
                //Debug.Log("HitBottom " + Vector3.Dot (compassVertical.transform.forward, directionVert));
                if (shieldPowerBottom <= 0)
                {
                    BottomParticles.SetActive(true);
                    //string playerDamaged = gameObject.tag; //remove for final;
                    //Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
                    //gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
                    gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
                }
                else
                {
                    shieldPowerBottom -= attackDamage;
                    if (shieldPowerBottom <= 0)
                    {
                        shieldPowerBottom = 0;
                        //string playerDamaged = gameObject.tag; //remove for final;
                        //gameHandler.PlayerShields(playerDamaged, "Bottom"); //remove for final;
                        gameHandler.PlayerShields(thisPlayer, "Bottom");  //use in final (slotted players)
                    }
                }
            }
        }
    }

    IEnumerator ShieldHitDisplay(GameObject shieldObj)
	{
		shieldObj.SetActive(true);
		// Renderer shieldRenderer = GetComponent<Renderer> ();
		// shieldRenderer.material.color = new Color(255,200,0,1f);
		yield return new WaitForSeconds(0.4f);
		//shieldRenderer.material.color = new Color(255,200,0,0f);
		shieldObj.SetActive(false);
	}



}
