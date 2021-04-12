using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_EndGoalLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  private void OnTriggerEnter(Collider other)
  {
    if ((other.transform.root.gameObject.tag == "Player1" || other.transform.root.gameObject.tag == "Player2"))
    {
      GameObject.Find("GameHandler").GetComponent<GameHandler>().coopPlayersWin = true;
      StartCoroutine(GameObject.Find("GameHandler").GetComponent<GameHandler>().CoopEndGame());
    }
  }
}
