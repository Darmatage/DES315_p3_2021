using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear_DRC : MonoBehaviour
{
    private Vector3 start;
    bool isFalling = false;
    float fallTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            fallTimer -= Time.deltaTime;
            if (fallTimer <= 0.0f)
            {
                transform.position = start;
                isFalling = false;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (3.0f - fallTimer) * (3.0f - fallTimer), transform.position.z);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.transform.parent.tag == "Player1" || collision.gameObject.transform.parent.tag == "Player2")
        {
            isFalling = true;
            fallTimer = 3.0f;
        }
    }
}
