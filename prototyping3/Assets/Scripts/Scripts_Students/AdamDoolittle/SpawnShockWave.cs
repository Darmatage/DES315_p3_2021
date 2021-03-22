using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShockWave : MonoBehaviour
{
    public GameObject shockwave;

    Vector3 shockwaveEndPos;
    Vector3 shockwaveStartPos;

    float aliveTimer = 3.0f;

    BotBasic_Move botController;

    // Start is called before the first frame update
    void Start()
    {
        shockwaveEndPos = new Vector3(shockwave.transform.position.x + 100, shockwave.transform.position.y, shockwave.transform.position.z + 100);
        shockwaveStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //var botController = GetComponent<BotBasic_Move>();

        if(botController.isGrounded == true && aliveTimer <= 3.0f)
        {
            aliveTimer += Time.deltaTime;
            Debug.Log(aliveTimer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Instantiate(shockwave, shockwaveStartPos, Quaternion.identity);
            Vector3.Lerp(shockwaveStartPos, shockwaveEndPos, 2.0f);
            while (aliveTimer <= 3.0f && aliveTimer != 0.0f)
            {
                aliveTimer -= Time.deltaTime;
                Debug.Log(aliveTimer);
            }
        }
    }
}
