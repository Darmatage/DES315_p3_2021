using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDamageMod : MonoBehaviour
{
	public GameObject compassSides;
	public GameObject compassVertical;
	private float SideLimit = 0.1f;
	private float AttackDamage;
	public float KnockBackSpeed = 10f;

	public float ShieldPower = 20f;

	public GameObject ShieldObj;
	public float ShieldRegenSpeed = 1.0f;
	public float ShieldRegenDelay = 3.0f;
	float ShieldMaxPower = 0.0f;
	float ShieldRegenTimer = 0.0f;

	public GameObject DMGParticles;

	private Rigidbody rb;
	private GameHandler gameHandler;
	private string thisPlayer;
	private bool notMyWeapon = true;
	void Start()
	{
		ShieldMaxPower = ShieldPower;
		ShieldRegenTimer = ShieldRegenDelay;
		if (gameObject.GetComponent<Rigidbody>() != null)
		{
			rb = gameObject.GetComponent<Rigidbody>();
		}
		if (GameObject.FindWithTag("GameHandler") != null)
		{
			gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		}
		thisPlayer = gameObject.transform.root.tag;

		ShieldObj.SetActive(false);

		DMGParticles.SetActive(false);
	}

    private void Update()
    {

		if (gameObject.transform.parent.tag == "Player1")
			gameHandler.p1Shields = ShieldPower;  //use in final (slotted players)

		else if (gameObject.transform.parent.tag == "Player2")
			gameHandler.p2Shields = ShieldPower;  //use in final (slotted players)
	}

    private void FixedUpdate()
    {
		if(ShieldPower < ShieldMaxPower)
			ShieldRegen();

	}

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Hazard")
		{
			if ((other.gameObject.GetComponent<HazardDamage>().isPlayer1Weapon == false) && (thisPlayer == "Player1")) { notMyWeapon = true; }
			else if ((other.gameObject.GetComponent<HazardDamage>().isPlayer2Weapon == false) && (thisPlayer == "Player2")) { notMyWeapon = true; }
			else { notMyWeapon = false; }
		}

		if ((other.gameObject.tag == "Hazard") && (notMyWeapon == true))
		{
			AttackDamage = other.gameObject.GetComponent<HazardDamage>().damage;

			Vector3 directionFore = (other.transform.position - transform.position).normalized;
			Vector3 directionSides = (other.transform.position - compassSides.transform.position).normalized;
			Vector3 directionVert = (other.transform.position - compassVertical.transform.position).normalized;

			if (Vector3.Dot(transform.forward, directionFore) < (-SideLimit))
			{
				rb.AddForce(transform.forward * KnockBackSpeed * -1, ForceMode.Impulse);
				//Debug.Log("HitBack " + Vector3.Dot (transform.forward, directionFore));
				CheckDamage();
			}

			else if (Vector3.Dot(transform.forward, directionFore) > SideLimit)
			{
				rb.AddForce(transform.forward * KnockBackSpeed, ForceMode.Impulse);
				//Debug.Log("HitFront "+ Vector3.Dot (transform.forward, directionFore));
				CheckDamage();
			}

			else if (Vector3.Dot(compassSides.transform.forward, directionSides) > SideLimit)
			{
				rb.AddForce(transform.right * KnockBackSpeed, ForceMode.Impulse);
				//Debug.Log("HitRight " + Vector3.Dot (compassSides.transform.forward, directionSides));
				CheckDamage();
			}

			else if (Vector3.Dot(compassSides.transform.forward, directionSides) < (-SideLimit))
			{
				rb.AddForce(transform.right * KnockBackSpeed * -1, ForceMode.Impulse);
				//Debug.Log("HitLeft " + Vector3.Dot (compassSides.transform.forward, directionSides));
				CheckDamage();
			}

			else if (Vector3.Dot(compassVertical.transform.forward, directionVert) > SideLimit)
			{
				rb.AddForce(transform.up * KnockBackSpeed, ForceMode.Impulse);
				//Debug.Log("HitTop " + Vector3.Dot (compassVertical.transform.forward, directionVert));
				CheckDamage();
			}

			else if (Vector3.Dot(compassVertical.transform.forward, directionVert) < (-SideLimit))
			{
				rb.AddForce(transform.up * KnockBackSpeed * -1, ForceMode.Impulse);
				//Debug.Log("HitBottom " + Vector3.Dot (compassVertical.transform.forward, directionVert));
				CheckDamage();
			}
		}
	}

	void CheckDamage()
    {
		if (ShieldPower <= 0)
		{
			DMGParticles.SetActive(true);
			//string playerDamaged = gameObject.tag; //remove for final;
			//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
			gameHandler.TakeDamage(thisPlayer, AttackDamage);  //use in final (slotted players)
		}
		else
		{
			ShieldPower -= AttackDamage;
			ShieldRegenTimer = ShieldRegenDelay;
			StartCoroutine(ShieldHitDisplay(ShieldObj));
			if (ShieldPower <= 0)
			{
				ShieldPower = 0;
				//string playerDamaged = gameObject.tag; //remove for final;
				//gameHandler.PlayerShields(playerDamaged, "Back"); //remove for final;
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

	void ShieldRegen()
    {
		if (ShieldRegenTimer <= 0)
		{
			ShieldPower += ShieldRegenSpeed;
			ShieldRegenTimer = 0.5f;
			if (ShieldPower >= ShieldMaxPower)
			{
				ShieldPower = ShieldMaxPower;
				ShieldRegenTimer = ShieldRegenDelay;
			}
		}
		else
			ShieldRegenTimer -= Time.deltaTime;
	}

	public void ConsumeForAttack()
    {
		ShieldPower -= 3;
		ShieldRegenTimer = ShieldRegenDelay;

		if (ShieldPower <= 0)
			ShieldPower = 0;
	}

	void OnCollisionEnter(Collision collision)
	{
		//if (collision.gameObject.layer.ToString() == "ground")
			GetComponent<AudioSource>().Play();
	}
}
