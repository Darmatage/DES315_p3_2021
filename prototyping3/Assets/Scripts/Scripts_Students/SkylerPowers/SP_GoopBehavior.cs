using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_GoopBehavior : MonoBehaviour
{
  // Who the goop should not damage
  public GameObject self;

  public float lifetime;

  // Start is called before the first frame update
  void Start()
  {
        
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
    }
  }
  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject != self)
    {

    }
  }
}
