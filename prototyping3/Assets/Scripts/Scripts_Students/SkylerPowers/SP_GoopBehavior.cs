using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_GoopBehavior : MonoBehaviour
{
  public float lifetime;
  public float growthRate;

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

      float growth = 1.0f + (growthRate - 1.0f) * Time.deltaTime;
      transform.localScale = new Vector3(transform.localScale.x * growth, transform.localScale.y, transform.localScale.z * growth);
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
