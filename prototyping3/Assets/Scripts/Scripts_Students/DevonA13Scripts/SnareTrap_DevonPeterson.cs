﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnareTrap_DevonPeterson : MonoBehaviour
{
    public float grabtimer = 5.0f;
    public float cooldown = 5.0f;
    float timer = 0.0f;
    float cooldowntimer = 0.0f;
    bool activated = false;
    public GameObject collisionobject = null;
    // Start is called before the first frame update
    void Start()
    {
        timer = grabtimer;
        grabtimer = 0.0f;
        cooldowntimer = cooldown;
        cooldown = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabtimer > 0.0f)
        {
            grabtimer -= Time.deltaTime;
            //if (collisionobject != null) 
            //{
            //    collisionobject.transform.position = transform.position;
            //}
        }
        else if (collisionobject != null && grabtimer <= 0.0f) 
        {
            collisionobject.GetComponent<BotBasic_Move>().isGrabbed = false;
            collisionobject = null;
            cooldown = cooldowntimer;
            foreach (Transform child in transform)
            {
                child.localPosition = new Vector3(child.localPosition.x, 4, child.localPosition.z);
            }
        }

        if (activated == true && cooldown > 0) 
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0.0f)
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = true;
                activated = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.tag == "Player1" || other.gameObject.transform.parent.tag == "Player2")
        {
            other.gameObject.GetComponent<BotBasic_Move>().isGrabbed = true;
            grabtimer = timer;
            collisionobject = other.gameObject;
            activated = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            other.gameObject.transform.position = gameObject.transform.position;
            foreach (Transform child in transform) 
            {
                child.localPosition = new Vector3(child.localPosition.x, -2.8f, child.localPosition.z);
            }
        }
    }
}
