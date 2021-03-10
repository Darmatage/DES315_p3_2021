using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB12_CannonBall : MonoBehaviour
{
    void Start()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
