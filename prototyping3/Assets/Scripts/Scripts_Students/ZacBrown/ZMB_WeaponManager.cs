﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZMB_WeaponManager : MonoBehaviour
{

    public string SprayInput;
    public string AcidPoolInput;

    public GameObject AcidPool;
    public GameObject AcidBar;
    //public GameObject 
    
    private ParticleSystem ps;
    private GameObject cv;
    private GameObject AcidMeter;
    
    private float Acid = 10.0f;

    private bool ability1 = false;
    private bool ability2 = false;

    // Start is called before the first frame update
    void Start()
    {
        //Set Controls and find Particle System for Acid Spray
        SprayInput = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        AcidPoolInput = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        ps = transform.Find("AcidSpray").GetComponent<ParticleSystem>();

        //Setup AcidMeter text over battle-bot
        cv = Instantiate(new GameObject(), transform);
        cv.AddComponent<Canvas>();
        cv.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        cv.transform.position = transform.position + new Vector3(0, 1, 0);
        cv.transform.localScale *= 0.01f;
        cv.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        AcidMeter = Instantiate(AcidBar, cv.transform.position, Quaternion.identity, cv.transform);
        AcidMeter.GetComponent<Text>().text = (Mathf.Round(Acid * 10.0f) / 10.0f).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is spraying acid
        if (Input.GetButtonDown(SprayInput))
        {
            if (!ps.isPlaying) //Start playing particles if off
            {
                ps.Play();
                ability1 = true;
            }
        }
        else if (Input.GetButtonUp(SprayInput))
        {
            if (ps.isPlaying) //Start playing particles if on
            {
                ps.Stop();
                ability1 = false;
            }
        }

        //Check if playing is dumping Acid
        if (Input.GetButtonDown(AcidPoolInput))
        {
            GameObject pool = Instantiate(AcidPool, transform.position - new Vector3(0, 0.6f, 0), Quaternion.identity);
            pool.transform.localScale = new Vector3(pool.transform.localScale.x * Acid, pool.transform.localScale.y,
                pool.transform.localScale.z * Acid);
            Destroy(pool, 5.0f);

            //Reset Acid level
            Acid = 0.0f;
        }

        UpdateAcid();
    }

    private void UpdateAcid()
    {
        if (ability1 && Acid > Time.deltaTime)
            Acid -= Time.deltaTime * 4;
        else
        {
            if (ps.isPlaying)
            {
                ps.Stop();
                ability1 = false;
            }

            Acid += Time.deltaTime;
            if (Acid > 10.0f)
                Acid = 10.0f;
        }
        
        AcidMeter.GetComponent<Text>().text = (Mathf.Round(Acid * 10.0f) / 10.0f).ToString();
    }

    private void OnDestroy()
    {
        Destroy(AcidMeter);
        Destroy(cv);
    }
}