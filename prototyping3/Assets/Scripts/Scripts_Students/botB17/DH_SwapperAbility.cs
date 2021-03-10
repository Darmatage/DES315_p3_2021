using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DH_SwapperAbility : MonoBehaviour
{

  private GameHandler gameHandler;

  public Material redArrowOn;
  public Material redArrowOff;
  public Material greenArrowOn;
  public Material greenArrowOff;

  public float cooldownTime = 3.0f;
  float cooldownTimer = 0.0f;
  bool onCooldown = false;

  Transform switcha;

  public GameObject flarePrefab;
  public GameObject spikePrefab;

  // Start is called before the first frame update
  void Start()
  {
    if (GameObject.FindWithTag("GameHandler") != null)
    {
      gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    if (SceneManager.GetActiveScene().name == "Arena1")
    {
      Instantiate(flarePrefab, new Vector3(1, 0, 5), new Quaternion());
      Instantiate(spikePrefab, new Vector3(12, 3, -12), Quaternion.Euler(0, -45, 0));
    }

    int numChildren = transform.childCount;

    for(int i = 0; i < numChildren; ++i)
    {
      if(transform.GetChild(i).name == "switch4-r00")
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

    if(!onCooldown)
    {
      if (Input.GetKeyDown(KeyCode.E) && gameObject.transform.root.GetComponent<playerParent>().isPlayer1)
      {
        Swap();
        StartCooldown();
      }
      if (Input.GetKeyDown(KeyCode.M) && !gameObject.transform.root.GetComponent<playerParent>().isPlayer1)
      {
        Swap();
        StartCooldown();
      }
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

  public void StartCooldown()
  {
    onCooldown = true;
    cooldownTimer = cooldownTime;

    int numChildren = switcha.childCount;

    for(int i = 0; i < numChildren; ++i)
    {
      if(switcha.GetChild(i).name == "switch4-r00_arrow_001")
      {
        switcha.GetChild(i).GetComponent<MeshRenderer>().material = greenArrowOff;
      }
      else if(switcha.GetChild(i).name == "switch4-r00_arrow_002")
      {
        switcha.GetChild(i).GetComponent<MeshRenderer>().material = redArrowOff;
      }
    }
  }

  public void EndCooldown()
  {
    onCooldown = false;

    int numChildren = switcha.childCount;

    for (int i = 0; i < numChildren; ++i)
    {
      if (switcha.GetChild(i).name == "switch4-r00_arrow_001")
      {
        switcha.GetChild(i).GetComponent<MeshRenderer>().material = greenArrowOn;
      }
      else if (switcha.GetChild(i).name == "switch4-r00_arrow_002")
      {
        switcha.GetChild(i).GetComponent<MeshRenderer>().material = redArrowOn;
      }
    }
  }


}
