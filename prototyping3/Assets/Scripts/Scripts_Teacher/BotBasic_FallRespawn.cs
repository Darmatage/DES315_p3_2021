using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBasic_FallRespawn : MonoBehaviour
{
	private Rigidbody rb;
	private GameObject fallRespawn;
	private Transform playerParent;

	//fall-does-damage variables
	private GameHandler gameHandler;
	private string thisPlayer;
	public bool doesFallDamage = false;
	public int playerFallDamage = 4;

    // Start is called before the first frame update
    void Start(){
		fallRespawn = GameObject.FindWithTag("FallRespawn");
		playerParent = gameObject.transform.parent;
		
		if (gameObject.GetComponent<Rigidbody>() != null){
			rb = gameObject.GetComponent<Rigidbody>();
		}
		
		if (GameObject.FindWithTag("GameHandler") != null){
			gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		}
		thisPlayer = gameObject.transform.root.tag;
    }

    // Update is called once per frame
    void Update()
    {
		if (gameObject.transform.position.y <=  fallRespawn.transform.position.y){
			gameObject.transform.position = playerParent.position;
			gameObject.transform.rotation = gameObject.transform.parent.rotation;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			
			if (doesFallDamage == true){
				gameHandler.TakeDamage(thisPlayer, playerFallDamage);
			}
		}
    }
}
