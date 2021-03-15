using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_GoopSpawner : MonoBehaviour
{
  public GameObject goopObject;

  public float spawnFrequency;
  private float spawnTimer;

  public float goopLifetime;

  public float damageFrequency;
  public float damageTimer;
  public bool resetTimer;

  // Start is called before the first frame update
  void Start()
  {
    spawnTimer = spawnFrequency;
    damageTimer = 0;
    resetTimer = false;

    // Take 15 damage immediately
    GameHandler gameHandler = null;
    if (GameObject.FindWithTag("GameHandler") != null)
    {
      gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }
    if (gameHandler != null)
    {
      if (transform.root.tag == "Player1")
      {
        gameHandler.TakeDamage("Player1", 15f);
      }
      if (transform.root.tag == "Player2")
      {
        gameHandler.TakeDamage("Player2", 15f);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (spawnTimer <= 0)
    {
      GameObject goop = Instantiate(goopObject);

      // goop says trans rights
      Transform goopTrans = goop.GetComponent<Transform>();
      goopTrans.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

      SP_GoopBehavior goopBehavior = goop.GetComponent<SP_GoopBehavior>();
      goopBehavior.lifetime = goopLifetime;
      goopBehavior.player = transform.root.tag;
      goopBehavior.spawner = GetComponent<SP_GoopSpawner>();

      spawnTimer = spawnFrequency;
    }
    else
    {
      spawnTimer -= Time.deltaTime;
    }

    if (damageTimer > 0)
    {
      damageTimer -= Time.deltaTime;
    }
    else
    {
      resetTimer = true;
    }
  }

  public void ResetTimer()
  {
    damageTimer = damageFrequency;
  }
}
