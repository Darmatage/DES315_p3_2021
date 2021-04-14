using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_DestroyBelowY : MonoBehaviour
{
  public float minY = -20f;

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    if (transform.position.y < minY)
      Destroy(gameObject);
  }
}
