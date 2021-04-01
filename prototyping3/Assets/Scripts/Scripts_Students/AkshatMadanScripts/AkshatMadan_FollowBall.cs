using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkshatMadan_FollowBall : MonoBehaviour
{
    private GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z);
        transform.position = newPos;
    }
}
