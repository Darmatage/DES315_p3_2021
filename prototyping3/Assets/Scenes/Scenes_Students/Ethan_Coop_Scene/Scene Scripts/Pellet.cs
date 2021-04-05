﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    List<Pellet> Pellet_List;
    public bool collected = false, finished = false;
    GameObject[] PelletGO;
    List<GameObject> FinishedPellet;

    float pellet_radius = 5f;
    // Start is called before the first frame update
    void Start()
    {

        PelletGO = GameObject.FindGameObjectsWithTag("Pellet");

        Pellet_List = new List<Pellet>();
        FinishedPellet = new List<GameObject>();

        for(int i = 0; i < PelletGO.Length; i++)
        {
            Pellet nextPellet = PelletGO[i].GetComponent<Pellet>();

            if(nextPellet != null)
            {
                float distance = Vector3.Distance(transform.position, nextPellet.transform.position);
                if (distance <= pellet_radius && nextPellet != this)
                {
                    Pellet_List.Add(nextPellet);
                }
            }
        }

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pellet_radius);
    }

    public Pellet GetNextPellet(Pellet PreviousPellet)
    {
        if(Pellet_List.Count == 0)
        {
            return null;
        }
        else if(Pellet_List.Count == 1 && Pellet_List.Contains(PreviousPellet))
        {
            return PreviousPellet;
        }
        else
        {
            Pellet nextPellet = null;
            //int nextIndex = 0;
            
            for(int i = 0; i < Pellet_List.Count; i++)
            {
                if(!Pellet_List[i].collected)
                {
                    nextPellet = Pellet_List[i];
                    return nextPellet;
                }  
            }
            nextPellet = Pellet_List[Random.Range(0, Pellet_List.Count)];

            return nextPellet;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < PelletGO.Length; i++)
        {
            if(PelletGO[i].GetComponent<Pellet>().collected && !FinishedPellet.Contains(PelletGO[i]))
            {
                FinishedPellet.Add(PelletGO[i]);
            }
        }

        if (FinishedPellet.Count == PelletGO.Length)
            finished = true;
    }
}