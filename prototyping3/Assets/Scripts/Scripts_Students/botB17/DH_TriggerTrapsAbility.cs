using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_TriggerTrapsAbility : MonoBehaviour
{
  private GameHandler gameHandler;

  public Material antennaOn;
  public Material antennaOff;

  public float cooldownTime = 3.0f;
  float cooldownTimer = 0.0f;
  bool onCooldown = false;

  Transform switcha;

  // Start is called before the first frame update
  void Start()
  {
    if (GameObject.FindWithTag("GameHandler") != null)
    {
      gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    int numChildren = transform.childCount;

    for (int i = 0; i < numChildren; ++i)
    {
      if (transform.GetChild(i).name == "switch4-r00")
      {
        switcha = transform.GetChild(i);
        break;
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (cooldownTimer > 0)
    {
      cooldownTimer -= Time.deltaTime;
    }
    else if (onCooldown)
    {
      cooldownTimer = 0;
      EndCooldown();
    }

    if (!onCooldown)
    {
      if (Input.GetKeyDown(KeyCode.T) && gameObject.transform.root.GetComponent<playerParent>().isPlayer1)
      {
        TriggerTraps();
        StartCooldown();
      }
      if (Input.GetKeyDown(KeyCode.Period) && !gameObject.transform.root.GetComponent<playerParent>().isPlayer1)
      {
        TriggerTraps();
        StartCooldown();
      }
    }

  }

  public void TriggerTraps()
  {
    GameObject[] objs = GameObject.FindGameObjectsWithTag("HazardButton");

    foreach(GameObject obj in objs)
    {
      if(obj.transform.root.name.Contains("Flare"))
      {
        obj.GetComponent<DH_FlareButton>().PressButton();
      }
      else
      {
        obj.GetComponent<DH_SpikeButton>().PressButton();
      }
    }
  }

  public void StartCooldown()
  {
    onCooldown = true;
    cooldownTimer = cooldownTime;

    int numChildren = switcha.childCount;

    for (int i = 0; i < numChildren; ++i)
    {
      if (switcha.GetChild(i).name == "Sphere")
      {
        switcha.GetChild(i).GetComponent<MeshRenderer>().material = antennaOff;
        break;
      }
    }
  }

  public void EndCooldown()
  {
    onCooldown = false;

    int numChildren = switcha.childCount;

    for (int i = 0; i < numChildren; ++i)
    {
      if (switcha.GetChild(i).name == "Sphere")
      {
        switcha.GetChild(i).GetComponent<MeshRenderer>().material = antennaOn;
        break;
      }
    }
  }
}
