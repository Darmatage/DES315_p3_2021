using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_RoomLogic : MonoBehaviour
{
  public Vector3 minCubeCorner;
  public Vector3 maxCubeCorner;

  public List<GameObject> enemiesToSpawn;

  public GameObject door;

  bool spawned = false;

  int timer = 0;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    if (spawned && timer <= 5)
      ++timer;

    if(timer > 5)
    {
      bool done = true;
      foreach(GameObject obj in enemiesToSpawn)
      {
        if(GameObject.Find(obj.name + "(Clone)") != null)
        {
          done = false;
          break;
        }
      }

      if(done && door.active)
      {
        door.SetActive(false);
        GetComponent<AudioSource>().Play();
      }

      if(Input.GetKeyDown(KeyCode.K))
      {
        foreach(GameObject obj in enemiesToSpawn)
        {
          Destroy(GameObject.Find(obj.name + "(Clone)"));
        }
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if((other.transform.root.gameObject.tag == "Player1" || other.transform.root.gameObject.tag == "Player2") && !spawned)
    {
      SpawnEnemies();
      spawned = true;
    }
  }

  void SpawnEnemies()
  {
    foreach (GameObject obj in enemiesToSpawn)
    {
      Vector3 spawnLocation = new Vector3(Random.Range(minCubeCorner.x, maxCubeCorner.x), Random.Range(minCubeCorner.y, maxCubeCorner.y), Random.Range(minCubeCorner.z, maxCubeCorner.z));
      GameObject spawnedObj = Instantiate(obj, spawnLocation, Quaternion.identity);
      spawnedObj.AddComponent<DH_Health>();
      spawnedObj.GetComponent<NPC_LoadPlayers>().playersReady = true;
    }
  }
}
