using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_SwapperAbility : MonoBehaviour
{

  private GameHandler gameHandler;

  // Start is called before the first frame update
  void Start()
  {
    if (GameObject.FindWithTag("GameHandler") != null)
    {
      gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }
  }

    // Update is called once per frame
    void Update()
  {
    if(Input.GetKeyDown(KeyCode.R) && gameObject.transform.root.GetComponent<playerParent>().isPlayer1)
    {
      Swap();
    }
    if (Input.GetKeyDown(KeyCode.Comma) && !gameObject.transform.root.GetComponent<playerParent>().isPlayer1)
    {
      Swap();
    }
  }

  public void Swap()
  {
    bool isPlayer1 = gameObject.transform.root.GetComponent<playerParent>().isPlayer1;
    GameObject other;
    if (isPlayer1)
      other = GameObject.Find("PLAYER2_SLOT").transform.GetChild(0).gameObject;
    else
      other = GameObject.Find("PLAYER1_SLOT").transform.GetChild(0).gameObject;

    Vector3 otherPos = other.transform.position;
    other.transform.position = transform.position;
    transform.position = otherPos;
  }
}
