using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShockWave : MonoBehaviour
{
    public GameObject shockwave;
    public GameObject newShock;
    //Vector3 shockwaveEndPos;
    //Vector3 shockwaveStartPos;

    public Vector3 targetScale;

    float aliveTimer = 3.0f;
    public float speed = 3.0f;

    public bool canGrow = false;

    public BotBasic_Move botController;

    // Start is called before the first frame update
    void Start()
    {
        //shockwaveEndPos = new Vector3(shockwave.transform.position.x + 100, shockwave.transform.position.y, shockwave.transform.position.z + 100);
        //shockwaveStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        targetScale = new Vector3(60f, 0.005f, 60f);
        botController = transform.parent.GetComponent<BotBasic_Move>();
    }

    private void Update()
    {
        if (canGrow == true)
        {
            newShock.transform.localScale = Vector3.Lerp(newShock.transform.localScale, targetScale, Time.deltaTime * speed);
        }
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    //var botController = GetComponent<BotBasic_Move>();

    //    if((botController.isGrounded == true) && (aliveTimer <= 3.0f))
    //    {
    //        aliveTimer += Time.deltaTime;
    //        Debug.Log(aliveTimer);
    //        if(aliveTimer <= 0)
    //        {
    //            aliveTimer = 0;
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("I hit the ground!");
            newShock = Instantiate(shockwave, transform.position, Quaternion.identity);
            if (gameObject.transform.root.tag == "Player1") { newShock.GetComponent<HazardDamage>().isPlayer1Weapon = true; }
            if (gameObject.transform.root.tag == "Player2") { newShock.GetComponent<HazardDamage>().isPlayer2Weapon = true; }
            //newShock.transform.localScale = Vector3.Lerp(shockwaveStartPos, shockwaveEndPos, 2.0f);
            canGrow = true;
            StartCoroutine(DestroyShock(newShock));
            //while (aliveTimer <= 3.0f && aliveTimer != 0.0f)
            //{
            //    aliveTimer -= Time.deltaTime;
            //    Debug.Log(aliveTimer);
            //}
            //Destroy(newShock);
        }
    }

    IEnumerator DestroyShock(GameObject thisShock)
    {
        yield return new WaitForSeconds(aliveTimer);
        canGrow = false;
        Destroy(thisShock);
    }
}
