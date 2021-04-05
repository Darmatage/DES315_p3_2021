using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_AI : MonoBehaviour
{
  public float moveSpeed;
  public float rotateSpeed;

  private Rigidbody rb;

  Transform enemy;

  // Start is called before the first frame update
  void Start()
  {
    if (gameObject.GetComponent<Rigidbody>() != null)
    {
      rb = gameObject.GetComponent<Rigidbody>();
    }

    if (transform.root.tag == "Player2" && GameObject.Find("PLAYER1_SLOT") != null)
    {
      enemy = GameObject.Find("PLAYER1_SLOT").transform.GetChild(0);
    }
    if (transform.root.tag == "Player1" && GameObject.Find("PLAYER2_SLOT") != null)
    {
      enemy = GameObject.Find("PLAYER2_SLOT").transform.GetChild(0);
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (enemy != null)
    {

      //Vector3 dir = Vector3.Normalize(new Vector3(0f, transform.position.y, 0f) - new Vector3(0f, enemy.position.y, 0f));
      //Vector3 rot = new Vector3(0f, transform.forward.y, 0f);

      Vector3 dir = Vector3.Normalize(transform.position - enemy.position);
      Vector3 rot = transform.forward;
      rot = Vector3.RotateTowards(rot, dir, rotateSpeed * Time.deltaTime, 0f);
      rot.y = 0f;
      transform.rotation = Quaternion.LookRotation(rot);
      transform.Translate(0, 0, moveSpeed * Time.deltaTime);
    }
  }
}
