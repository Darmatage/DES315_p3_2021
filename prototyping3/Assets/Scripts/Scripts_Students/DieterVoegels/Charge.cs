using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
  public GameObject leftPlow;
  public GameObject rightPlow;
  public float damage = 2.0f;
  public float chargeCooldown = 2.0f;
  public float chargeSpeed = 10.0f;

  bool charge = false;
  string chargeButton;
  float chargeTimer = 0.0f;
  float damageTimer = 0.0f;

  private void Start()
  {
    chargeButton = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetButtonDown(chargeButton) == true && chargeTimer <= 0)
    {
      charge = true;
    }

    if (charge == true)
    {
      gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * chargeSpeed, ForceMode.Impulse);
      leftPlow.GetComponent<HazardDamage>().damage = damage;
      rightPlow.GetComponent<HazardDamage>().damage = damage;
      charge = false;
      chargeTimer = chargeCooldown;
      damageTimer = .25f;
    }

    if (chargeTimer > 0)
    {
      chargeTimer -= Time.deltaTime;
    }

    if (damageTimer > 0)
    {
      damageTimer -= Time.deltaTime;
    }
    else
    {
      leftPlow.GetComponent<HazardDamage>().damage = 0;
      rightPlow.GetComponent<HazardDamage>().damage = 0;
    }
  }
}
