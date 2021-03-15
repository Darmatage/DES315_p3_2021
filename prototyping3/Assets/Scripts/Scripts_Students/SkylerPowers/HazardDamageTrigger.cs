using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDamageTrigger : MonoBehaviour
{
	private string enemy;
	private SP_GoopBehavior goop;

	public float damage = 1f;
	private GameHandler gameHandler;
	private AudioSource audioSource;

	public GameObject particlesPrefab;
	public Vector3 SpawnParticlesHere;

	public bool isPlayer1Weapon = false;
	public bool isPlayer2Weapon = false;

	void Start()
	{
		if (GameObject.FindWithTag("GameHandler") != null)
		{
			gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		}

		audioSource = GetComponent<AudioSource>();
		goop = GetComponent<SP_GoopBehavior>();

		if (goop.player == "Player1") { isPlayer1Weapon = true; enemy = "Player2"; }
		if (goop.player == "Player2") { isPlayer2Weapon = true; enemy = "Player1"; }
	}

	void OnTriggerEnter(Collider other)
	{
		string target = other.gameObject.transform.root.tag;
		if (gameHandler != null && target == enemy && damage > 0f)
		{
			if (audioSource != null)
			{
				audioSource.Play();
			}
			if (target == "Player1")
			{
				gameHandler.TakeDamage("Player1", damage);
			}
			if (target == "Player2")
			{
				gameHandler.TakeDamage("Player2", damage);
			}
			if (goop != null)
			{
				goop.spawner.resetTimer = false;
				goop.spawner.ResetTimer();
			}
			//GameObject.Destroy(gameObject);
		}
	}

		IEnumerator destroyParticles(GameObject particles)
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(particles);
	}

}

//NOTE: this script is just damage
//hazard object movement is managed by their button
//reporting damage is done by the damage script on the bots 