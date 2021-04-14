using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_FlareTrap : MonoBehaviour
{
  private GameHandler gameHandler;

  float onTime = 5.0f;
  float onTimer = 0.0f;
  bool on = false;

  float damageFrequency = 1.0f;
  float damageTimer = 1.0f;

  List<GameObject> entitiesInFlame = new List<GameObject>();

  // Start is called before the first frame update
  void Start()
  {
    gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
  }

  // Update is called once per frame
  void Update()
  {
    if(onTimer > 0)
    {
      onTimer -= Time.deltaTime;
    }
    else if(on)
    {
      onTimer = 0;
      TurnOffFlare();
    }

    if(on)
    {
      if (damageTimer > 0)
      {
        damageTimer -= Time.deltaTime;
      }
      else
      {
        damageTimer = damageFrequency;
        DealDamage();
      }
    }
  }

  public void TurnOnFlare()
  {
    on = true;
    onTimer = onTime;
    transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    transform.GetChild(2).gameObject.SetActive(true);
  }

  public void TurnOffFlare()
  {
    on = false;
    transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
    transform.GetChild(2).gameObject.SetActive(false);
  }

  public void DealDamage()
  {
    foreach(GameObject obj in entitiesInFlame)
    {
      if (!obj)
        continue;
      if (obj.GetComponent<DH_Health>())
      {
        obj.GetComponent<DH_Health>().TakeDamage(1);
      }
      else
      {
        gameHandler.TakeDamage(obj.tag, 1);
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if(!entitiesInFlame.Contains(other.transform.root.gameObject))
      entitiesInFlame.Add(other.transform.root.gameObject);
  }

  private void OnTriggerExit(Collider other)
  {
    entitiesInFlame.Remove(other.transform.root.gameObject);
  }
}
