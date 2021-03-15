using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_GoopBehavior : MonoBehaviour
{
  public string player;
  public float lifetime;
  public float growthRate;
  public SP_GoopSpawner spawner;
  private HazardDamageTrigger hazard;
  public float damage = 1f;
  private bool occupied;

  private Vector3 pos;

  // Start is called before the first frame update
  void Start()
  {
    hazard = GetComponent<HazardDamageTrigger>();

    occupied = false;
  }

  void OnTriggerEnter(Collider other)
  {
    string target = other.gameObject.transform.root.tag;
    if (target != player)
    {
      occupied = true;
    }
  }

  void OnTriggerExit(Collider other)
  {
    string target = other.gameObject.transform.root.tag;
    if (target != player)
    {
      occupied = false;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (lifetime <= 0)
    {
      GameObject.Destroy(gameObject);
    }
    else
    {
      lifetime -= Time.deltaTime;

      float growth = 1.0f + (growthRate - 1.0f) * Time.deltaTime;
      transform.localScale = new Vector3(transform.localScale.x * growth, transform.localScale.y, transform.localScale.z * growth);
    }

    if (spawner != null && spawner.resetTimer && occupied)
    {
      hazard.damage = damage;
    }
    else if (!spawner.resetTimer)
    {
      hazard.damage = 0f;
    }

    for (int i = 0; i < 10; ++i)
    {
      if (transform.position.y > 0.3f)
      {
        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime, transform.position.z);
        if (transform.position.y < 0.3f)
        {
          transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
        }
      }
    }
  }
}
