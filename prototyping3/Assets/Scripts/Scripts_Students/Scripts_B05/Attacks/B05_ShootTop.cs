﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_ShootTop : MonoBehaviour
{
    private bool b_aiming; // true if attack button is being held down
    private bool b_cooling; // true if attack is in cooldown

    private float timer = 0.0f;
    private float t_cooldown = 1.0f; // time before another top can be shot out

    private int top_count = 0;
    private int top_max = 5;

    private float charge;               // 0.0f - no charge    1.0f - full charge
    private float chargeSpeed = 0.5f;
    private float topSpeed = 65.0f;

    public GameObject minitop = null;

    public Transform arrow_trans;       // X scale and Z position go from 0.5 to 1.5 
    public SpriteRenderer arrow_sprite; // Color go from Green to Yellow to Red
    private Vector3 start_pos = new Vector3(0.0f, 0.0f, 0.5f);
    private Vector3 start_scale = new Vector3(0.5f, 1.0f, 1.0f);
    private Color start_color = new Color(0.0f, 1.0f, 0.0f);

    public Transform spawn_point;

    public Bot05_Move b05;

    public MeshRenderer[] ammo;
    public Material mat_ammo_on;
    public Material mat_ammo_off;

    // Start is called before the first frame update
    void Start()
    {
        b_aiming = false;
        charge = 0.0f;
        arrow_trans.localPosition = start_pos;
        arrow_trans.localScale = start_scale;
        arrow_sprite.color = start_color;
        arrow_sprite.enabled = false;
        top_count = 0;

        for(int i = 0; i < 5; ++i)
        {
            ammo[i].material = mat_ammo_on;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (b_aiming)
        {
            if (charge < 1.0f)
            {
                charge += chargeSpeed * Time.deltaTime;
                if (charge > 1.0f) charge = 1.0f;
            }
            arrow_trans.localPosition = new Vector3(0.0f, 0.0f, start_pos.z + charge);
            arrow_trans.localScale = new Vector3(start_scale.x + charge, 1.0f, 1.0f);
            arrow_sprite.color = new Color(Mathf.Min(charge * 2.0f, 1.0f), Mathf.Min(1.0f - (charge - 0.5f) / 0.5f, 1.0f), 0.0f);
        }

        if (b_cooling)
        {
            timer += Time.deltaTime;
            if (timer > t_cooldown)
                EndCool();
        }
    }

    private void Shoot()
    {
        GameObject new_top = Instantiate(minitop, spawn_point.position, Quaternion.identity);
        new_top.GetComponent<Rigidbody>().AddForce(transform.forward * charge * topSpeed, ForceMode.Impulse);
        new_top.GetComponent<B05_MiniTop>().SetParent(gameObject);
        new_top.tag = gameObject.transform.root.tag;
        if (new_top.tag == "Player1")
        {
            new_top.GetComponentInChildren<HazardDamage>().gameObject.layer = 16;
            new_top.GetComponent<B05_MiniTop>().ball.material = new_top.GetComponent<B05_MiniTop>().eye1;
        }
        else if (new_top.tag == "Player2")
        {
            new_top.GetComponentInChildren<HazardDamage>().gameObject.layer = 19;
            new_top.GetComponent<B05_MiniTop>().ball.material = new_top.GetComponent<B05_MiniTop>().eye2;
        }
        
        ++top_count;
        ammo[5 - top_count].material = mat_ammo_off;
    }

    public void BeginAttack()
    {
        if (b_aiming || b_cooling || !b05.IsState(Bot05_Move.STATE.NORMAL) || top_count == top_max) return;

        b_aiming = true;
        b05.SetState(Bot05_Move.STATE.AIMING);
        charge = 0.0f;
        arrow_trans.localPosition = start_pos;
        arrow_trans.localScale = start_scale;
        arrow_sprite.color = start_color;
        arrow_sprite.enabled = true;
    }

    public void EndAttack()
    {
        if (!b_aiming || b_cooling || top_count == top_max) return;

        b_aiming = false;
        b05.SetState(Bot05_Move.STATE.NORMAL);
        arrow_sprite.enabled = false;
        Shoot();
        BeginCool();
    }

    private void BeginCool()
    {
        b_cooling = true;
        timer = 0.0f;
    }

    private void EndCool()
    {
        b_cooling = false;
    }

    public bool IsAvaliable()
    {
        return top_count < top_max;
    }
}
