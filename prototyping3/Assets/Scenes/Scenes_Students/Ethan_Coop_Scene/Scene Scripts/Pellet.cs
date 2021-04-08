using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public List<Pellet> Pellet_List;
    public bool collected = false;

    PelletHolder p_holder;

    float pellet_radius = 5f;
    // Start is called before the first frame update
    void Start()
    {

        p_holder = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<PelletHolder>();

        Pellet_List = new List<Pellet>();

        for(int i = 0; i < p_holder.All_Pellets.Length; i++)
        {
            Pellet nextPellet = p_holder.All_Pellets[i].GetComponent<Pellet>();

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

        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, pellet_radius);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
