using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_MoveToDestination : MonoBehaviour
{
  Vector3 currentDestination;
  float arrivalTime;
  float distance;

  bool travelling = false;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    if(travelling)
    {
      Vector3 dir = currentDestination - transform.position;

      dir.Normalize();

      float speed = distance / arrivalTime;

      transform.position += dir * Time.deltaTime * speed;

      if (Vector3.Distance(transform.position, currentDestination) < 0.1)
      {
        GameObject.Destroy(gameObject, 1);
      }
    }
  }

  public void MoveToInTime(Vector3 destination, float time)
  {
    currentDestination = destination;
    arrivalTime = time;
    travelling = true;

    Vector3 dir = currentDestination - transform.position;

    distance = dir.magnitude;
  }
}
