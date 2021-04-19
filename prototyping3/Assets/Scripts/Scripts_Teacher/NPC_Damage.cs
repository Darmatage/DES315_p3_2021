using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Damage : MonoBehaviour{
	
	public static float health;
	public float healthStart = 80f;
	
	private float attackDamage;
	
	private GameHandler gameHandler;
	
	public bool isInvincible = false;
	
	
    // Start is called before the first frame update
    void Start(){
        if (GameObject.FindWithTag("GameHandler") != null){
			gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		}
		
		health = healthStart;
		gameHandler.CoopUpdateMonster(health);
    }

	private void OnTriggerEnter (Collider other) {
		if ((other.gameObject.tag == "Hazard")&&(other.gameObject.GetComponent<HazardDamage>().isMonsterWeapon==false)){
			attackDamage = other.gameObject.GetComponent<HazardDamage>().damage;
			TakeDamage(attackDamage);
		}
	}

	public void TakeDamage(float damage){ 
		if (isInvincible == false){
			health -= damage;
			if (health <= 0){
				health = 0;
			}
			gameHandler.CoopUpdateMonster(health);
		}
	}

}
