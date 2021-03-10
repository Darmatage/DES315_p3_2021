using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_SpikeHazard : MonoBehaviour
{
  bool active = false;
  float activeTime = 5;
  float activeTimer = 0;

  float speed = 8f;

  //Vector3 currentOffset = new Vector3();

  Vector3 buttonStartPos;

  // Start is called before the first frame update
  void Start()
  {
    buttonStartPos = transform.GetChild(0).position;
  }

  // Update is called once per frame
  void Update()
  {
    if (activeTimer > activeTime - 1)
    {
      activeTimer -= Time.deltaTime;
      Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
      localForward = localForward.normalized;
      transform.Translate(localForward * speed * Time.deltaTime);
      //currentOffset += -transform.forward * speed * Time.deltaTime;
    }
    else if(activeTimer > 0)
    {
      activeTimer -= Time.deltaTime;
      Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
      localForward = localForward.normalized;
      transform.Translate(localForward * -(speed/4.0f) * Time.deltaTime);
      //currentOffset += -transform.forward * -(speed / 4.0f) * Time.deltaTime;
    }
    else if (active)
    {
      active = false;
      activeTimer = 0;
    }

    transform.GetChild(0).transform.position = buttonStartPos;// + currentOffset;

  }

  public void TurnOnSpikes()
  {
    active = true;
    activeTimer = activeTime;
  }
}
