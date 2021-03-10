using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_GoopSpawner : MonoBehaviour
{
  public GameObject goopObject;

  public float spawnFrequency;
  private float spawnTimer;

  public float goopLifetime;

  // Start is called before the first frame update
  void Start()
  {
    spawnTimer = 0;
  }

  // Update is called once per frame
  void Update()
  {
    if (spawnTimer <= 0)
    {
      GameObject goop = Instantiate(goopObject);

      // goop says trans rights
      Transform goopTrans = goop.GetComponent<Transform>();
      goopTrans.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

      SP_GoopBehavior goopBehavior = goop.GetComponent<SP_GoopBehavior>();
      goopBehavior.lifetime = goopLifetime;

      HazardDamageTrigger goopHazardDamageTrigger = goop.GetComponent<HazardDamageTrigger>();
      goopHazardDamageTrigger.player = transform.root.tag;

      spawnTimer = spawnFrequency;
    }
    else
    {
      spawnTimer -= Time.deltaTime;
    }
  }
}
