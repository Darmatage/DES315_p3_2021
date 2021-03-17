using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BotB04.Controller
{
	public class RechargableShield
	{
		public float power;
		public float maxPower;
		public float delay;
		public bool recharging;

		public GameObject shieldRef;
		private AudioSource audioSource;

		private AudioClip readyClip;
		private AudioClip brokenClip;
		public RechargableShield(float power, GameObject shield)
		{
			this.power = power;
			this.maxPower = power;
			shieldRef = shield;
			audioSource = shieldRef.GetComponent<AudioSource>();

			delay = 0;
			recharging = false;
		}


		public void SetAudioClips(AudioClip ready, AudioClip broken)
        {
			readyClip = ready;
			brokenClip = broken;
        }

		public void Heal(float amount)
        {
			power = Mathf.Min(power + amount, maxPower);
			if (recharging && power == maxPower)
            {
				audioSource.PlayOneShot(readyClip);
				recharging = false;
			}
		}

		public void Damage(float amount)
        {
			power = Mathf.Max(0, power - amount);
			if(!recharging && power == 0)
            {
				audioSource.PlayOneShot(brokenClip);
				//audioSource.Play();
				recharging = true;
            }
        }

	}

	public class BotB04_DamageHandler : MonoBehaviour
	{
		public GameObject compassSides;
		public GameObject compassVertical;
		private float sidelimit = .1f;
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


        public class ShieldRuntimeData
        {
            public float front;
            public float back;
            public RechargableShield left;
            public RechargableShield right;
            public float top;
            public float bottom;

            public ShieldRuntimeData(ShieldData ShieldBaseStats, GameObject leftShield, GameObject rightShield)
            {
                front = ShieldBaseStats.PowerFrontMax;
                back = ShieldBaseStats.PowerBackMax;
				left = new RechargableShield(ShieldBaseStats.PowerLeftMax, leftShield);
                right = new RechargableShield(ShieldBaseStats.PowerRightMax, rightShield);
                top = ShieldBaseStats.PowerTopMax;
                bottom = ShieldBaseStats.PowerBottomMax;
            }

        }
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

		[SerializeField]
		private AudioClip shieldRepairedAudio;
		[SerializeField]
		private AudioClip shieldBrokenAudio;


		private void Awake()
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

			shieldRuntime = new ShieldRuntimeData(ShieldBaseStats, ShieldReferences.shieldLeftObj, ShieldReferences.shieldRightObj);
			shieldRuntime.left.SetAudioClips(shieldRepairedAudio, shieldBrokenAudio);
			shieldRuntime.right.SetAudioClips(shieldRepairedAudio, shieldBrokenAudio);


			DamageParticleReferences.dmgParticlesFront.SetActive(false);
			DamageParticleReferences.dmgParticlesBack.SetActive(false);
			DamageParticleReferences.dmgParticlesLeft.SetActive(false);
			DamageParticleReferences.dmgParticlesRight.SetActive(false);
			DamageParticleReferences.dmgParticlesTop.SetActive(false);
		}

        private void Update()
        {
			RepairShield(shieldRuntime.left,  DamageParticleReferences.dmgParticlesLeft);
			RepairShield(shieldRuntime.right, DamageParticleReferences.dmgParticlesRight);
		}

		private void RepairShield(RechargableShield shield, GameObject damageParticles)
        {
			shield.shieldRef.SetActive(true);

			shield.delay -= Time.deltaTime;
			if(shield.delay <= 0)
            {
				shield.Heal(ShieldBaseStats.RechargeRate * Time.deltaTime);

				if(damageParticles != null)
					damageParticles.SetActive(false);
			}


			Renderer shieldRenderer = shield.shieldRef.GetComponent<Renderer>();
			if(shield.recharging)
				shieldRenderer.material.color = shieldDownColor;
			else
				shieldRenderer.material.color = shieldReadyColor;
			shieldRenderer.material.color = new Color(shieldRenderer.material.color.r, shieldRenderer.material.color.g, shieldRenderer.material.color.b, shield.power / shield.maxPower);

			if (shield.power > 0)
				shieldRenderer.enabled = true;
			else
				shieldRenderer.enabled = false;


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

				Vector3 directionFore = (other.transform.position - transform.position).normalized;
				Vector3 directionSides = (other.transform.position - compassSides.transform.position).normalized;
				Vector3 directionVert = (other.transform.position - compassVertical.transform.position).normalized;

				//Hit Back!!!
				if (Vector3.Dot(transform.forward, directionFore) < (-sidelimit))
				{
					//shieldRuntime.delayBack = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.forward * knockBackSpeed * -1, ForceMode.Impulse);
					//Debug.Log("HitBack " + Vector3.Dot (transform.forward, directionFore));
					if (shieldRuntime.back <= 0)
					{
						DamageParticleReferences.dmgParticlesBack.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage);  //use in final (slotted players)
					}
					else
					{
						shieldRuntime.back -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldBackObj));
						if (shieldRuntime.back <= 0)
						{
							shieldRuntime.back = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Back"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Back");  //use in final (slotted players)
						}
					}
				}

				//Hit Front!!!
				if (Vector3.Dot(transform.forward, directionFore) > sidelimit)
				{
					//shieldRuntime.delayFront = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.forward * knockBackSpeed, ForceMode.Impulse);
					//Debug.Log("HitFront "+ Vector3.Dot (transform.forward, directionFore));
					if (shieldRuntime.front <= 0)
					{
						DamageParticleReferences.dmgParticlesFront.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.front -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldFrontObj));
						if (shieldRuntime.front <= 0)
						{
							shieldRuntime.front = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Front"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Front");  //use in final (slotted players)
						}
					}
				}

				//Hit right!!!
				if (Vector3.Dot(compassSides.transform.forward, directionSides) > sidelimit)
				{
					shieldRuntime.right.delay = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.right * knockBackSpeed, ForceMode.Impulse);
					//Debug.Log("HitRight " + Vector3.Dot (compassSides.transform.forward, directionSides));
					if (shieldRuntime.right.power <= 0)
					{
						DamageParticleReferences.dmgParticlesRight.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.right.Damage(attackDamage);
						//StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldRightObj));
						if (shieldRuntime.right.power <= 0)
						{
							shieldRuntime.right.power = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Right"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Right");  //use in final (slotted players)
						}
					}
				}

				//Hit left!!!
				if (Vector3.Dot(compassSides.transform.forward, directionSides) < (-sidelimit))
				{
					shieldRuntime.left.delay = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.right * knockBackSpeed * -1, ForceMode.Impulse);
					//Debug.Log("HitLeft " + Vector3.Dot (compassSides.transform.forward, directionSides));
					if (shieldRuntime.left.power <= 0)
					{
						DamageParticleReferences.dmgParticlesLeft.SetActive(true);
						//string playerDamaged = gameObject.tag; //remove for final;
						//Debug.Log("I hit the core of " + playerDamaged + "\n for damage = " + attackDamage); // remove in final
						//gameHandler.TakeDamage(playerDamaged, attackDamage); //remove for final;
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.left.Damage(attackDamage);

						//StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldLeftObj));
						if (shieldRuntime.left.power <= 0)
						{
							shieldRuntime.left.power = 0;
							//string playerDamaged = gameObject.tag; //remove for final;
							//gameHandler.PlayerShields(playerDamaged, "Left"); //remove for final;
							gameHandler.PlayerShields(thisPlayer, "Left");  //use in final (slotted players)
						}
					}
				}

				//Hit top!!!
				if (Vector3.Dot(compassVertical.transform.forward, directionVert) > sidelimit)
				{
					//shieldRuntime.delayTop = ShieldBaseStats.RechargeDelay;
					
					rb.AddForce(transform.up * knockBackSpeed, ForceMode.Impulse);
					//Debug.Log("HitTop " + Vector3.Dot (compassVertical.transform.forward, directionVert));
					if (shieldRuntime.top <= 0)
					{
						DamageParticleReferences.dmgParticlesTop.SetActive(true);
						gameHandler.TakeDamage(thisPlayer, attackDamage); // use in final (slotted players)
					}
					else
					{
						shieldRuntime.top -= attackDamage;
						StartCoroutine(ShieldHitDisplay(ShieldReferences.shieldTopObj));
						if (shieldRuntime.top <= 0)
						{
							shieldRuntime.top = 0;
							gameHandler.PlayerShields(thisPlayer, "Top");  //use in final (slotted players)
						}
					}
				}

				//Hit bottom!!!
				if (Vector3.Dot(compassVertical.transform.forward, directionVert) < (-sidelimit))
				{
					//shieldRuntime.delayBottom = ShieldBaseStats.RechargeDelay;

					rb.AddForce(transform.up * knockBackSpeed * -1, ForceMode.Impulse);
					//Debug.Log("HitBottom " + Vector3.Dot (compassVertical.transform.forward, directionVert));
					if (shieldRuntime.bottom <= 0)
					{
						//dmgParticlesBottom.SetActive(true);
						gameHandler.TakeDamage(thisPlayer, attackDamage);
					}
					
					else
					{
						shieldRuntime.bottom -= attackDamage;
						if (shieldRuntime.bottom <= 0)
						{
							shieldRuntime.bottom = 0;
							gameHandler.PlayerShields(thisPlayer, "Bottom");
						}
					}
				}
			}
		}

		IEnumerator ShieldHitDisplay(GameObject shieldObj)
		{
            shieldObj.SetActive(true);
            Renderer shieldRenderer = shieldObj.GetComponent<Renderer>();
            shieldRenderer.material.color = new Color(255, 200, 0, 1f);
            yield return new WaitForSeconds(0.4f);
            //shieldRenderer.material.color = new Color(255,200,0,0f);
            shieldObj.SetActive(false);

            yield return null;
		}



	




	}
}