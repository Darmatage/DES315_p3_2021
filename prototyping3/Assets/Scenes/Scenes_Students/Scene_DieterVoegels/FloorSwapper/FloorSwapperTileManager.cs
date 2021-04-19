using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwapperTileManager : MonoBehaviour
{
  public GameObject rotator;
  bool active = false;
  float rotation = 0;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    if (active == true)
    {
      rotation += 180 * Time.deltaTime;

      if (rotation <= 360)
      {
        rotator.transform.Rotate(Vector3.up, 180 * Time.deltaTime);
      }
      else
      {
        rotator.transform.rotation = Quaternion.identity;
        active = false;
      }
    }
  }

  public void Activate()
  {
    active = true;
    rotation = 0;
  }
}
