using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ZMB_AI : MonoBehaviour
{
    private GameObject target;
    private GameHandler handler;
    private BotBasic_Move move;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private ZMB_WeaponManager weapons;

    private bool moving = false;
    private bool attacking = false;
    private bool showcase = false;

    // Start is called before the first frame update
    void Start()
    {
        Scene sc = SceneManager.GetActiveScene();
        if (sc.name == "EndScene")
        {
            showcase = true;
        }
        
        handler = GameObject.Find("GameHandler").GetComponent<GameHandler>();

        if (transform.root.CompareTag("Player1") && handler.Player2Holder.transform.childCount != 0)
        {
            target = handler.Player2Holder.transform.GetChild(0).gameObject;
        }
        else if (handler.Player1Holder.transform.childCount != 0)
        {
            target = handler.Player1Holder.transform.GetChild(0).gameObject;
        }

        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();

        move = GetComponent<BotBasic_Move>();

        weapons = GetComponent<ZMB_WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showcase)
            return;
        
        if (move.isGrabbed)
            return;
        
        float dist = (transform.position - target.transform.position).magnitude;
        float angle = Vector3.Dot((transform.position + transform.forward).normalized,
            (transform.position + (target.transform.position - transform.position)).normalized);

        angle = angle * 180f / (float)Math.PI;
        
        if (dist > 9f)
            MoveToTarget();
        else if (dist < 9f)
        {
            agent.isStopped = true;
        }

        if (dist < 9f)
        {
            Vector3 len = transform.position - target.transform.position;
            Vector3 flat = new Vector2(transform.forward.x, transform.forward.z);
            
            if (!attacking && weapons.Acid > 6.0f)
            {
                weapons.StartAcidSpray();
                attacking = true;
            }
            else if (attacking && weapons.Acid < 2f)
            {
                weapons.StopAcidSpray();
                attacking = false;
            }
            
            if (weapons.Acid > 2.0f && dist < 6.0f)
            {
                weapons.ActivateAcidPool();
            }
        }

        if (weapons.Acid <= 0.2f)
            attacking = false;

        if (move.isTurtled && move.canFlip)
        {
            rb.AddForce(rb.centerOfMass + new Vector3(move.jumpSpeed / 2, 0, move.jumpSpeed / 2), ForceMode.Impulse);
            transform.Rotate(150f, 0, 0);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    void MoveToTarget()
    {
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
    }

    void MoveAwayFromTarget()
    {
        agent.SetDestination(target.transform.position + (transform.position - target.transform.position));
    }

    void MoveRandom()
    {
        Vector3 dest = transform.position + new Vector3(Random.Range(-10, 10), 0.0f, Random.Range(-10, 10));
        while (!agent.SetDestination(dest))
        {
            dest = transform.position + new Vector3(Random.Range(-10, 10), 0.0f, Random.Range(-10, 10));
        }
    }

    void RotateToTarget(float angle)
    {
        //Vector3 rotation = new Vector3(0f, angle, 0f) * 5;
        //transform.Rotate(rotation * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, Time.deltaTime, 0f));
    }
}
