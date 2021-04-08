using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletHolder : MonoBehaviour
{

    public GameObject[] All_Pellets;


    // Start is called before the first frame update
    void Start()
    {
        All_Pellets = GameObject.FindGameObjectsWithTag("Pellet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
