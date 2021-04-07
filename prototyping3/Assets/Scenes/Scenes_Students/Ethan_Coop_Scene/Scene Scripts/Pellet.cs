using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    List<Pellet> Pellet_List;
    public bool collected = false;
    GameObject[] PelletGO;

    float pellet_radius = 5f;
    // Start is called before the first frame update
    void Start()
    {

        PelletGO = GameObject.FindGameObjectsWithTag("Pellet");

        Pellet_List = new List<Pellet>();

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
            Pellet nextPellet = null, closestPellet;
            for (int i = 0; i < Pellet_List.Count; i++)
            {
                if(!Pellet_List[i].collected)
                {
                    nextPellet = Pellet_List[i];
                    return nextPellet;
                }
            }

            //closestPellet = ClosestPellet(PreviousPellet);
            ////int nextIndex = 0;

            //float distance = float.MaxValue;

            ////Get the next pellet that will lead to the next uncollected pellet
            //for (int i = 0; i < Pellet_List.Count; i++)
            //{
            //    if(Vector3.Distance(Pellet_List[i].transform.position, closestPellet.transform.position) < distance)
            //    {
            //        distance = Vector3.Distance(Pellet_List[i].transform.position, closestPellet.transform.position);
            //        nextPellet = Pellet_List[i];
            //    }  
            //}
            nextPellet = Pellet_List[Random.Range(0, Pellet_List.Count)];

            return nextPellet;
        }
    }

    Pellet ClosestPellet(Pellet currentPellet) //finds the closest uncollected Pellet
    {

        List<Pellet> Uncollected_Pellets = new List<Pellet>();

        for(int i = 0; i < PelletGO.Length; i++)
        {
            if (!PelletGO[i].GetComponent<Pellet>().collected)
                Uncollected_Pellets.Add(PelletGO[i].GetComponent<Pellet>());
        }

        float distance = float.MaxValue;
        Pellet closestPellet = null;
        for(int i = 0; i < Uncollected_Pellets.Count; i++)
        {
            if(Vector3.Distance(Uncollected_Pellets[i].gameObject.transform.position, currentPellet.gameObject.transform.position) < distance)
            {
                distance = Vector3.Distance(Uncollected_Pellets[i].gameObject.transform.position, currentPellet.gameObject.transform.position);
                closestPellet = Uncollected_Pellets[i];
            }
        }



        return closestPellet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
