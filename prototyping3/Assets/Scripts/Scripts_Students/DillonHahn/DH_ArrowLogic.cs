using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_ArrowLogic : MonoBehaviour
{

  public float rotationSpeed = 5;
  public float bobSpeed = 5;
  public float bobIntensity = 2;

  Vector3 startPos;

  float timer = 0f;
  float currentRotation = 0f;
  

  void Start()
  {
    startPos = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime * bobSpeed;

    transform.position = startPos + new Vector3(0, Mathf.Sin(timer) * bobIntensity, 0);

    currentRotation += Time.deltaTime * rotationSpeed;

    transform.rotation = Quaternion.Euler(0, currentRotation, 0);
  }
}
