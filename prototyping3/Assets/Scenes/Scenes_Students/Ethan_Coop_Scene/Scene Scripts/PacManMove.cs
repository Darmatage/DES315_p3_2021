using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PacManMove : MonoBehaviour
{
    bool PatrolWaiting = false;
    float PatrolWaitTime = 3.0f;

    public float health = 100;

    List<Pellet> Visted_Pellets;
    int finished_Pellet_count = 0;
    bool finished = false;

    NavMeshAgent Agent;
    [SerializeField] Pellet currentPellet, previousPellet, startingPellet;

    bool traveling, waiting;
    float waitTimer;
    int PelletsVisited;
    public bool start = false;
    // Start is called before the first frame update
    void Start()
    {

        Visted_Pellets = new List<Pellet>();
        Agent = GetComponent<NavMeshAgent>();
        if(Agent == null)
        {
            Debug.LogError("There is no NavMeshAgent on: " + gameObject.name);
        }
        else
        {
            if(currentPellet == null)
            {
                GameObject[] PelletsGO = GameObject.FindGameObjectsWithTag("Pellet Start");
                if(PelletsGO.Length > 0)
                {
                    int random = Random.Range(0, PelletsGO.Length);
                    startingPellet = PelletsGO[random].GetComponent<Pellet>();

                    if(startingPellet != null)
                    {
                        currentPellet = startingPellet;
                    }
                }
                else
                {
                    Debug.LogError("No Waypoints found");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (finished_Pellet_count == GameObject.FindGameObjectsWithTag("Pellet").Length)
        {
            finished = true;
        }

        if (start)
        {
            SetDestination();
            start = false;
        }

        if (traveling && Agent.remainingDistance <= 1 && !finished)
        {
            traveling = false;
            PelletsVisited++;

            if (PatrolWaiting)
            {
                waiting = true;
                waitTimer = 0;
            }
            else
                SetDestination();
        }

        if(waiting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer >= PatrolWaitTime)
            {
                waiting = false;
                SetDestination();
            }
        }
    }

    public void Begin()
    {
        start = true;
    }

    private void SetDestination()
    {
        if(PelletsVisited > 0)
        {
            Visted_Pellets.Add(currentPellet);
            if(Visted_Pellets.Count >= 5)
            {
                Visted_Pellets.RemoveAt(0);
            }
            Pellet nextPellet = null;
            do
            {
                nextPellet = currentPellet.GetNextPellet(previousPellet);
            }
            while (Visted_Pellets.Contains(nextPellet));

            previousPellet = currentPellet;
            currentPellet = nextPellet;
        }

        Vector3 Target = currentPellet.transform.position;
        Agent.SetDestination(Target);
        traveling = true;
        if (previousPellet && !previousPellet.collected)
        {
            previousPellet.collected = true;
            finished_Pellet_count++;
        }
    }
}
