﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_MiniTop : MonoBehaviour
{
    // todo - blade spin speed relates to minitop speed?

    private GameObject parent_bot;

    private float t_cooldown = 0.2f;
    private float timer;
    private bool b_justhit;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        b_justhit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (b_justhit)
        {
            timer += Time.deltaTime;
            if (timer > t_cooldown)
            {
                b_justhit = false;
            }
        }
    }

    public void SetParent(GameObject go)
    {
        parent_bot = go;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // add force again if parent hits them while they are in blade rush
        if (collision.gameObject == parent_bot || collision.gameObject.gameObject == parent_bot || collision.gameObject.gameObject.gameObject == parent_bot)
        {
            if (parent_bot.GetComponent<Bot05_Move>().IsRushing() && !b_justhit)
            {
                b_justhit = true;
                Vector3 parent_pos = parent_bot.GetComponent<Bot05_Move>().GetCenter().position;
                Vector3 pos = transform.position;
                Vector3 dir = pos - parent_pos;
                dir.Normalize();
                dir.y = 0.0f;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
                rb.AddForce(dir * 35.0f, ForceMode.Impulse);
            }
        }
    }
}