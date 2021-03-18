using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    private GameObject hp1;
    private GameObject hp2;

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
        cv.GetComponent<Canvas>().worldCamera = Camera.main;

        AcidMeter = Instantiate(AcidBar, cv.transform.position, Quaternion.identity, cv.transform);
        AcidMeter.GetComponent<Text>().text = (Mathf.Round(Acid * 10.0f) / 10.0f).ToString();
        
        //Assign Damage script to bots
        GameObject player = GameObject.Find("PLAYER1_SLOT");

        if (player != null)
        {
            player.transform.GetChild(0).gameObject.AddComponent<ZMB_Damage>();
        }

        player = GameObject.Find("PLAYER2_SLOT");

        if (player != null)
        {
            player.transform.GetChild(0).gameObject.AddComponent<ZMB_Damage>();
        }
        
        //Get Health
        GameHandler gm = GameObject.Find("GameHandler").GetComponent<GameHandler>();

        hp1 = gm.p1HealthText;
        hp2 = gm.p2HealthText;
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

            ParticleSystem ps = pool.GetComponentInChildren<ParticleSystem>();
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.radius *= Acid;

            if (gameObject.transform.root.tag == "Player1")
            {
                pool.transform.GetChild(0).GetComponent<HazardDamage>().isPlayer1Weapon = true;
            }
            else if (gameObject.transform.root.tag == "Player2")
            {
                pool.transform.GetChild(0).GetComponent<HazardDamage>().isPlayer2Weapon = true;
            }
            
            Destroy(pool, 5.0f);

            //Reset Acid level
            Acid = 0.0f;
        }

        UpdateAcid();

        AcidMeter.transform.rotation = transform.root.GetChild(1).rotation;
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
