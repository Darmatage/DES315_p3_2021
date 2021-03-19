using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBlade : MonoBehaviour
{
  public GameObject blade;
  public float speed = 1;

  // Update is called once per frame
  void Update()
  {
    blade.transform.Rotate(new Vector3(0,1,0), speed * Time.deltaTime);
  }
}
