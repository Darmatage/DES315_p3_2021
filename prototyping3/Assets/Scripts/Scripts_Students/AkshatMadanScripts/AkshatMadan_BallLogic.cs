using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkshatMadan_BallLogic : MonoBehaviour
{
    public GameObject respawnLoc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < respawnLoc.transform.position.y)
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }
}
