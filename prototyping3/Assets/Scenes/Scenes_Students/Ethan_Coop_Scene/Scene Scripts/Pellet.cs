using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    List<Pellet> Pellet_List;

    float pellet_radius = 160f;
    // Start is called before the first frame update
    void Start()
    {

        GameObject[] PelletGO = GameObject.FindGameObjectsWithTag("Pellet");

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
            Pellet nextPellet;
            int nextIndex = 0;
            do
            {
                nextIndex = UnityEngine.Random.Range(0, Pellet_List.Count);
                nextPellet = Pellet_List[nextIndex];
            }
            while (nextPellet == PreviousPellet);

            return nextPellet;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
