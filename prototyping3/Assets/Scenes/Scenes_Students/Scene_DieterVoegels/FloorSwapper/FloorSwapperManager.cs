using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwapperManager : MonoBehaviour
{
  //Player references
  GameObject player1;
  GameObject player2;

  //Floor swapper script references
  FloorSwapperTileManager tile;
  FloorSwapperTileRingManager tileRing;
  FloorSwapperButtonManager button;

  //Floor swapper public variables
  public float activateStartingTime;
  public float inactiveStartingTime;

  //Floor swapper private variables
  bool active;
  float activateTimer;
  float inactiveTimer;

  void Start()
  {
    //Get script references
    tile = transform.Find("FloorSwapper").Find("TileRotator").Find("Tile").GetComponent<FloorSwapperTileManager>();
    tileRing = transform.Find("FloorSwapper").Find("TileRing").GetComponent<FloorSwapperTileRingManager>();
    button = transform.Find("FloorSwapperButton").GetComponent<FloorSwapperButtonManager>();

    //Set active to false
    active = false;

    GameHandler.onBattleStart += BattleStart;
    enabled = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (player1 == null)
    {
      player1 = GameObject.Find("PLAYER1_SLOT").transform.GetChild(0).gameObject;
    }

    if (player2 == null)
    {
      player2 = GameObject.Find("PLAYER2_SLOT").transform.GetChild(0).gameObject;
    }

    if (active == true)
    {
      if (activateTimer >= 0.0f)
      {
        activateTimer -= Time.deltaTime;
        tileRing.UpdateRingForward(activateTimer / activateStartingTime);

        if (activateTimer < 0.0f)
        {
          tile.Activate();
        }
      }
      else if (inactiveTimer >= 0.0f)
      {
        inactiveTimer -= Time.deltaTime;
        tileRing.UpdateRingReverse(inactiveTimer / inactiveStartingTime);
      }
      else
      {
        active = false;
        button.ResetButton();
        tileRing.ResetRing();
      }
    }
  }

  void BattleStart()
  {
    enabled = true;
  }

  public void Activate()
  {
    if (active == false)
    {
      active = true;
      activateTimer = activateStartingTime;
      inactiveTimer = inactiveStartingTime;
    }
  }

  bool checkBounds(Vector3 botPosition)
  {
    float swapperUpperBounds = transform.position.z + transform.lossyScale.z / 2.0f;
    float swapperLowerBounds = transform.position.z - transform.lossyScale.z / 2.0f;
    float swapperRightBounds = transform.position.x + transform.lossyScale.x / 2.0f;
    float swapperLeftBounds = transform.position.x - transform.lossyScale.x / 2.0f;

    if (botPosition.z <= swapperUpperBounds &&
        botPosition.z >= swapperLowerBounds &&
        botPosition.x <= swapperRightBounds &&
        botPosition.x >= swapperLeftBounds)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
}
