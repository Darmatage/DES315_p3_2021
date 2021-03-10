﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_Attack : MonoBehaviour
{
	public Rigidbody rigid;
	public GameObject clawLeft;
	public GameObject clawRight;
	public GameObject projectile;
	public GameObject status;
	public GameObject explosion;
	public ParticleSystem magnetParticles;
	public HazardDamage hitbox;
	public AnimationCurve throwHeightCurve;
	public AnimationCurve throwTiltCurve;

	// arena and enemy
	private GameHandler gameHandler;
	private BotBasic_Move enemyMove;
	private BotBasic_Damage enemyDamage;
	private Rigidbody enemyRigid;

	// player
	private enum Action { NONE, GRAPPLE, PROJECTILE, THROW }
	private Action action = Action.NONE; // player can only do one attack at a time
	private float clawDegreeMin = -100.0f; // left = -135.0f +dt, right = 135.0f -dt
	private float clawDegreeMax = -30f;

	// inputs are retrieved from parent "slot" object
	public string button1;
	public string button2;
	public string button3;
	public string button4;

	// arbitrary values used in actions
	private float timer;
	private float magnetizedTimer;
	private float angle;
	private int phase;
	private bool magnetized;
	private Vector3 startingPos;
	private float throwHeight = 15.0f;
	private float cooldown;

	void Start()
	{
		// retrieve inputs
		button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

		// retrieve components
		rigid = GetComponent<Rigidbody>();

		// retrieve enemy components and handler
		GameObject handlerObj = GameObject.FindGameObjectWithTag("GameHandler");
		string slotName = null;
		if (handlerObj != null) slotName = button1.Equals("p2Fire1") ? "PLAYER1_SLOT" : "PLAYER2_SLOT";
		if (button1.Equals("p2Fire1")) hitbox.isPlayer2Weapon = true; else hitbox.isPlayer1Weapon = true;
		GameObject slotObj = GameObject.Find(slotName);
		GameObject enemyObj = null;
		if (slotObj != null) enemyObj = slotObj.transform.GetChild(0).gameObject;
		if (enemyObj != null)
		{
			enemyRigid = enemyObj.GetComponent<Rigidbody>();
			enemyMove = enemyObj.GetComponent<BotBasic_Move>();
			enemyDamage = enemyObj.GetComponent<BotBasic_Damage>();
		}
	}

	void Update()
	{
		// only update if enemy is available
		if (enemyRigid == null) return;

		// update magnetized status
		if (magnetized)
        {
			magnetizedTimer -= Time.deltaTime;
			if (magnetizedTimer <= 0.0f) magnetized = false;
        }

		// update action cooldown
		cooldown -= Time.deltaTime;
		if (cooldown > 0.0f) return;

		// attack 1 = grapple
		if (Input.GetButtonDown(button1) && action == Action.NONE)
		{
			action = Action.GRAPPLE;
			angle = clawDegreeMin;
			phase = 0;
		}

		// attack 2 = projectile
		if (Input.GetButtonDown(button2) && action == Action.NONE)
        {
			// create explosion
			GameObject obj = Instantiate(projectile, transform.position + transform.forward * 4.0f, transform.rotation);
			B03_Bullet bullet = obj.GetComponent<B03_Bullet>();
			bullet.target = enemyRigid.transform.root.name;
			bullet.parent = this;

			cooldown = 1.0f;
		}
	}

    private void FixedUpdate()
    {
		// only update if enemy is available
		if (enemyRigid == null) return;

		// update attacks
		switch (action)
		{
			case Action.GRAPPLE:
				GrappleUpdate();
				break;
			case Action.PROJECTILE:
				break;
			case Action.THROW:
				ThrowUpdate();
				break;
		}
	}

    void GrappleUpdate()
	{
		if (phase == 0)
        {
			// open claw
			if (angle < clawDegreeMax)
			{
				angle += 250.0f * Time.fixedDeltaTime;
				clawLeft.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
				clawRight.transform.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
			}

			if (angle >= clawDegreeMax)
            {
				angle = clawDegreeMax;
				clawLeft.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
				clawRight.transform.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
				timer = 0.0f;
				phase = 1;
				magnetParticles.Play();
			}
		}
		else if (phase == 1)
        {
			timer += Time.fixedDeltaTime;
			Vector3 grappleOffset = transform.position + transform.forward * 5.0f;

			// move the enemy to grabbing range if magnetized
			if (magnetized)
            {
				// snap to position if close enough
				if ((grappleOffset - enemyRigid.transform.position).magnitude < 0.1f)
					enemyRigid.MovePosition(grappleOffset);
				else // move towards position otherwise
					enemyRigid.MovePosition(enemyRigid.transform.position + 5.0f * (grappleOffset - enemyRigid.transform.position).normalized * Time.fixedDeltaTime);
			}

			// start throw if enemy makes contact within range
			Collider[] hits = Physics.OverlapSphere(grappleOffset, 2.0f);
			// determine if any of the collisions belong to the enemy, begin grab if so
			foreach (var hit in hits)
            {
				// begins throw if parent is enemy
				if (hit.transform.root.name.Equals(enemyRigid.transform.root.name))
                {
					action = Action.THROW;
					rigid.MoveRotation(Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f));
					startingPos = transform.position;
					angle = rigid.rotation.eulerAngles.x;
					timer = 0.0f;
					phase = 0;
					magnetParticles.Stop();
					magnetized = false;
					break;
				}
            }

			// grab duration lasts this long
			if (timer >= 0.75f)
			{
				magnetParticles.Stop();
				phase = 2;
			}
        }
		else if (phase == 2)
        {
			// close claw
			if (angle > clawDegreeMin)
			{
				angle -= 150.0f * Time.fixedDeltaTime;
				clawLeft.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
				clawRight.transform.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
			}

			if (angle <= clawDegreeMin)
			{
				angle = clawDegreeMin;
				clawLeft.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
				clawRight.transform.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
				phase = 3;

				action = Action.NONE;
				cooldown = 1.0f;
			}
		}
	}

	void ThrowUpdate()
    {
		if (phase == 0)
		{
			rigid.isKinematic = true;

			timer += Time.fixedDeltaTime;

			enemyRigid.MovePosition(transform.position + transform.forward * 6.0f);
			float lerpPercent = throwHeightCurve.Evaluate(timer / 2.0f);
			float lerpTiltPercent = throwTiltCurve.Evaluate(timer / 2.0f);
			transform.position = (Vector3.Lerp(startingPos, startingPos + Vector3.up * throwHeight, lerpPercent));
			transform.rotation = (Quaternion.Euler(90.0f - 180.0f * lerpTiltPercent, transform.rotation.eulerAngles.y + 540.0f * Time.fixedDeltaTime, transform.rotation.eulerAngles.z));


			// adjust landing
			if (timer >= 1.25f && timer < 1.35f)
			{
				// raycast to locate the ground
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(transform.position + Vector3.down * 5.0f, Vector3.down, out hit))
				{
					startingPos = hit.point + Vector3.up * 5.0f;
				}
			}

			if (timer >= 2.0f)
			{
				phase = 1;
				timer = 0.0f;
				DealDamage();
			}
		}
		else if (phase == 1)
		{
			// close claw
			if (angle > clawDegreeMin)
			{
				angle -= 500.0f * Time.fixedDeltaTime;
				clawLeft.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
				clawRight.transform.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
			}

			if (angle <= clawDegreeMin)
			{
				angle = clawDegreeMin;
				clawLeft.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
				clawRight.transform.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
				phase = 2;

				action = Action.NONE;
				// deactivate hitbox
				hitbox.gameObject.SetActive(false);
				rigid.drag = 0.0f;
				cooldown = 1.0f;
			}
		}
	}

	void DealDamage()
    {
		// launch both player and enemy
		rigid.isKinematic = false;
		enemyMove.isGrabbed = false;
		rigid.velocity = transform.up * -15.0f;
		enemyRigid.velocity = transform.up * 15.0f;
		rigid.drag = 10.0f; // so this bot doesn't go flying off

		// activate hitbox
		hitbox.gameObject.SetActive(true);

		// create explosion
		GameObject explodeObj = Instantiate(explosion, transform.position + transform.forward * 5.0f, Quaternion.identity);
		Destroy(explodeObj, 2.0f);
	}

	public void ActivateMagnetize()
    {
		magnetized = true;
		magnetizedTimer = 5.0f;
		GameObject obj = Instantiate(status, enemyRigid.transform.position, Quaternion.identity);
		obj.GetComponent<B03_Status>().target = enemyRigid.transform;
		Destroy(obj, magnetizedTimer);
	}
}