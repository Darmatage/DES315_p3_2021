using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BotB04.Controller
{
	public class BotB04_DamageHandler : MonoBehaviour
	{
		public GameObject compassSides;
		public GameObject compassVertical;
		private float sidelimit = 3.0f;
		private float attackDamage;
		public float knockBackSpeed = 10f;

		[System.Serializable]
        public class ShieldData
        {
			public float RechargeDelay = 2f;
			public float RechargeRate  = 0f;

			public float PowerFrontMax  = 5f;
			public float PowerBackMax   = 5f;
			public float PowerLeftMax   = 5f;
			public float PowerRightMax  = 5f;
			public float PowerTopMax    = 5f;
			public float PowerBottomMax = 5f;
		}
		[SerializeField]
		public ShieldData ShieldBaseStats;

		[System.Serializable]
		public class ShieldRuntimeData
		{
			public float powerFront;
			public float powerBack;
			public float powerLeft;
			public float powerRight;
			public float powerTop;
			public float powerBottom;

			public float delayFront;
			public float delayBack;
			public float delayLeft;
			public float delayRight;
			public float delayTop;
			public float delayBottom;

			public ShieldRuntimeData(ShieldData ShieldBaseStats)
            {
				powerFront  = ShieldBaseStats.PowerFrontMax;
				powerBack   = ShieldBaseStats.PowerBackMax;
				powerLeft   = ShieldBaseStats.PowerLeftMax;
				powerRight  = ShieldBaseStats.PowerRightMax;
				powerTop    = ShieldBaseStats.PowerTopMax;
				powerBottom = ShieldBaseStats.PowerBottomMax;

				delayFront  = 0;
				delayBack   = 0;
				delayLeft   = 0;
				delayRight  = 0;
				delayTop    = 0;
				delayBottom = 0;
			}

		}
		[SerializeField]
		public ShieldRuntimeData shieldRuntime;

		[System.Serializable]
        public class ShieldReferenceStorage
        {
			public GameObject shieldFrontObj;
			public GameObject shieldBackObj;
			public GameObject shieldLeftObj;
			public GameObject shieldRightObj;
			public GameObject shieldTopObj;
			public GameObject shieldBottomObj;
		}
		[SerializeField]
		public ShieldReferenceStorage ShieldReferences;

		[System.Serializable]
        public class DamageParticleStorage
        {
			public GameObject dmgParticlesFront;
			public GameObject dmgParticlesBack;
			public GameObject dmgParticlesLeft;
			public GameObject dmgParticlesRight;
			public GameObject dmgParticlesTop;
		}
		[SerializeField]
		public DamageParticleStorage DamageParticleReferences;

		private Rigidbody rb;
		private GameHandler gameHandler;
		private string thisPlayer;
		private bool notMyWeapon = true;



		[SerializeField]
		private Color shieldReadyColor;
		[SerializeField]
		private Color shieldDownColor;

		void Start()
		{
			if (gameObject.GetComponent<Rigidbody>() != null)
			{
				rb = gameObject.GetComponent<Rigidbody>();
			}
			if (GameObject.FindWithTag("GameHandler") != null)
			{
				gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
			}
			thisPlayer = gameObject.transform.root.tag;

			ShieldReferences.shieldFrontObj.SetActive(false);
			ShieldReferences.shieldBackObj.SetActive(false);
			ShieldReferences.shieldLeftObj.SetActive(false);
			ShieldReferences.shieldRightObj.SetActive(false);
			ShieldReferences.shieldTopObj.SetActive(false);
			ShieldReferences.shieldBottomObj.SetActive(false);

			shieldRuntime = new ShieldRuntimeData(ShieldBaseStats);

			DamageParticleReferences.dmgParticlesFront.SetActive(false);
			DamageParticleReferences.dmgParticlesBack.SetActive(false);
			DamageParticleReferences.dmgParticlesLeft.SetActive(false);
			DamageParticleReferences.dmgParticlesRight.SetActive(false);
			DamageParticleReferences.dmgParticlesTop.SetActive(false);


		}

        private void Update()
        {
			//RepairShield(ref shieldRuntime.powerFront,  ShieldBaseStats.PowerFrontMax,  ref shieldRuntime.delayFront, DamageParticleReferences.dmgParticlesFront, ShieldReferences.shieldFrontObj);
			//RepairShield(ref shieldRuntime.powerBack,   ShieldBaseStats.PowerBackMax,   ref shieldRuntime.delayBack, DamageParticleReferences.dmgParticlesBack, ShieldReferences.shieldBackObj);
			RepairShield(ref shieldRuntime.powerLeft,   ShieldBaseStats.PowerLeftMax,   ref shieldRuntime.delayLeft, DamageParticleReferences.dmgParticlesLeft, ShieldReferences.shieldLeftObj);
			RepairShield(ref shieldRuntime.powerRight,  ShieldBaseStats.PowerRightMax,  ref shieldRuntime.delayRight, DamageParticleReferences.dmgParticlesRight, ShieldReferences.shieldRightObj);
			//RepairShield(ref shieldRuntime.powerTop,    ShieldBaseStats.PowerTopMax,    ref shieldRuntime.delayTop, DamageParticleReferences.dmgParticlesTop, ShieldReferences.shieldTopObj);
			//RepairShield(ref shieldRuntime.powerBottom, ShieldBaseStats.PowerBottomMax, ref shieldRuntime.delayBottom, null, ShieldReferences.shieldBottomObj);
		}

		private void RepairShield(ref float shieldHealth, float shieldMax, ref float delay, GameObject damageParticles, GameObject shieldObj)
        {
			delay -= Time.deltaTime;
			if(delay <= 0)
            {
				shieldHealth = Mathf.Min(shieldHealth + ShieldBaseStats.RechargeRate * Time.deltaTime, shieldMax);
				if(damageParticles != null)
					damageParticles.SetActive(false);

				

			}


			shieldObj.SetActive(true);
			Renderer shieldRenderer = shieldObj.GetComponent<Renderer>();

			if (shieldObj == ShieldReferences.shieldLeftObj)
				shieldRenderer.material.color = shieldReadyColor;
			else if (shieldObj == ShieldReferences.shieldRightObj)
				shieldRenderer.material.color = shieldReadyColor;
			else
				shieldRenderer.material.color = new Color(255f / 255f, 0 / 255f, 0 / 255f);

			shieldRenderer.material.color = new Color(shieldRenderer.material.color.r, shieldRenderer.material.color.g, shieldRenderer.material.color.b, shieldHealth / shieldMax);
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
				attackDamage = other.gameObject.GetComponent<HazardDamage>().damage;

				Vector3 directionFore = other.transform.position - transform.position;
				Vector3 directionSides = other.transform.position - compassSides.transform.position;
				Vector3 directionVert = other.transform.position - compassVertical.transform.position;

				//Hit Back!!!
				if (Vector3.Dot(transform.forward, directionFore) < (-sidelimit))
				{
					shieldRuntime.delayBack = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.forward * knockBackSpeed * -1, ForceMode.Impulse);
					//Debug.Log("HitBack " + Vector3.Dot (transform.forward, directionFore));
					if (shieldRuntime.powerBack <= 0)
					{
						DamageParticleReferences.dmgParticlesBack.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage);  //use in final (slotted players)
					}
					else
					{
						shieldRuntime.powerBack -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldBackObj));
						if (shieldRuntime.powerBack <= 0)
						{
							shieldRuntime.powerBack = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Back"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Back");  //use in final (slotted players)
						}
					}
				}

				//Hit Front!!!
				if (Vector3.Dot(transform.forward, directionFore) > sidelimit)
				{
					shieldRuntime.delayFront = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.forward * knockBackSpeed, ForceMode.Impulse);
					//Debug.Log("HitFront "+ Vector3.Dot (transform.forward, directionFore));
					if (shieldRuntime.powerFront <= 0)
					{
						DamageParticleReferences.dmgParticlesFront.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.powerFront -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldFrontObj));
						if (shieldRuntime.powerFront <= 0)
						{
							shieldRuntime.powerFront = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Front"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Front");  //use in final (slotted players)
						}
					}
				}

				//Hit right!!!
				if (Vector3.Dot(compassSides.transform.forward, directionSides) > sidelimit)
				{
					shieldRuntime.delayRight = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.right * knockBackSpeed, ForceMode.Impulse);
					//Debug.Log("HitRight " + Vector3.Dot (compassSides.transform.forward, directionSides));
					if (shieldRuntime.powerRight <= 0)
					{
						DamageParticleReferences.dmgParticlesRight.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.powerRight -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldRightObj));
						if (shieldRuntime.powerRight <= 0)
						{
							shieldRuntime.powerRight = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Right"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Right");  //use in final (slotted players)
						}
					}
				}

				//Hit left!!!
				if (Vector3.Dot(compassSides.transform.forward, directionSides) < (-sidelimit))
				{
					shieldRuntime.delayLeft = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.right * knockBackSpeed * -1, ForceMode.Impulse);
					//Debug.Log("HitLeft " + Vector3.Dot (compassSides.transform.forward, directionSides));
					if (shieldRuntime.powerLeft <= 0)
					{
						DamageParticleReferences.dmgParticlesLeft.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.powerLeft -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldLeftObj));
						if (shieldRuntime.powerLeft <= 0)
						{
							shieldRuntime.powerLeft = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Left"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Left");  //use in final (slotted players)
						}
					}
				}

				//Hit top!!!
				if (Vector3.Dot(compassVertical.transform.forward, directionVert) > sidelimit)
				{
					shieldRuntime.delayTop = ShieldBaseStats.RechargeDelay;
					
					rb.AddForce(transform.up * knockBackSpeed, ForceMode.Impulse);
					//Debug.Log("HitTop " + Vector3.Dot (compassVertical.transform.forward, directionVert));
					if (shieldRuntime.powerTop <= 0)
					{
						DamageParticleReferences.dmgParticlesTop.SetActive(true);
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.powerTop -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldTopObj));
						if (shieldRuntime.powerTop <= 0)
						{
							shieldRuntime.powerTop = 0;
							gameHandler.PlayerShields(thisPlayer, "Top");  //use in final (slotted players)
						}
					}
				}

				//Hit bottom!!!
				if (Vector3.Dot(compassVertical.transform.forward, directionVert) < (-sidelimit))
				{
					shieldRuntime.delayBottom = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.up * knockBackSpeed * -1, ForceMode.Impulse);
					//Debug.Log("HitBottom " + Vector3.Dot (compassVertical.transform.forward, directionVert));
					if (shieldRuntime.powerBottom <= 0)
					{
						//dmgParticlesBottom.SetActive(true);
						gameHandler.TakeDamage(thisPlayer, attackDamage);
					}
					
					else
					{
						shieldRuntime.powerBottom -= attackDamage;
						if (shieldRuntime.powerBottom <= 0)
						{
							shieldRuntime.powerBottom = 0;
							gameHandler.PlayerShields(thisPlayer, "Bottom");
						}
					}
				}
			}
		}

		IEnumerator ShieldHitDisplay(GameObject shieldObj)
		{
			//shieldObj.SetActive(true);
			//Renderer shieldRenderer = shieldObj.GetComponent<Renderer> ();
			//shieldRenderer.material.color = new Color(255,200,0,1f);
			//yield return new WaitForSeconds(0.4f);
			////shieldRenderer.material.color = new Color(255,200,0,0f);
			//shieldObj.SetActive(false);

			yield return null;
		}



	




	}
}