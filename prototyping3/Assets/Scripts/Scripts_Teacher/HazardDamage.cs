﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDamage : MonoBehaviour{
	public float damage = 1f;
	private GameHandler gameHandler;

	public GameObject particlesPrefab;
	public Vector3 SpawnParticlesHere;

	public bool isPlayer1Weapon = false;
	public bool isPlayer2Weapon = false;
	public bool isMonsterWeapon = false;

    void Start(){
		if (GameObject.FindWithTag("GameHandler") != null){
			gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		}

		if (gameObject.transform.root.tag == "Player1"){isPlayer1Weapon = true;}
		if (gameObject.transform.root.tag == "Player2"){isPlayer2Weapon = true;}
    }

	void OnCollisionEnter(Collision other){
		//if (other.relativeVelocity.magnitude > 5) {
			//get impact location
			SpawnParticlesHere = other.contacts[0].point;
			//make particles
			GameObject damageParticles = Instantiate (particlesPrefab, SpawnParticlesHere, other.transform.rotation);
			StartCoroutine(destroyParticles(damageParticles));
		//}

	}

	IEnumerator destroyParticles(GameObject particles){
		yield return new WaitForSeconds(0.5f);
		Destroy(particles);
	}

}

	//NOTE: this script is just damage
	//hazard object movement is managed by their button
	//reporting damage is done by the damage script on the bots 