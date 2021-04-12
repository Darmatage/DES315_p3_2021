using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_Health : MonoBehaviour
{
  int StartingHP = 5;
  int currentHP;

  // Start is called before the first frame update
  void Start()
  {
    currentHP = StartingHP;
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  public void TakeDamage(int damage)
  {
    currentHP -= damage;

    if (currentHP <= 0)
      Death();
  }  

  private void Death()
  {
    Destroy(gameObject);
  }
}
