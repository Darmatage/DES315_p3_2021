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

  List<string> playersInFlame = new List<string>();

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
  }

  public void TurnOffFlare()
  {
    on = false;
    transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
  }

  public void DealDamage()
  {
    foreach(string name in playersInFlame)
    {
      gameHandler.TakeDamage(name, 1);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if(!playersInFlame.Contains(other.transform.root.tag))
      playersInFlame.Add(other.transform.root.tag);
  }

  private void OnTriggerExit(Collider other)
  {
    playersInFlame.Remove(other.transform.root.tag);
  }
}
