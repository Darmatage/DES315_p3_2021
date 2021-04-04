using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PacManMove : MonoBehaviour
{
    bool PatrolWaiting = false;
    float PatrolWaitTime = 3.0f;

<<<<<<< HEAD
    List<Pellet> Visted_Pellets;

    NavMeshAgent Agent;
    [SerializeField] Pellet currentPellet, previousPellet, startingPellet;
=======
    float switchProbability = 0.2f;

    NavMeshAgent Agent;
    [SerializeField] Pellet currentPellet, previousPellet;
>>>>>>> origin/main

    bool traveling, waiting;
    float waitTimer;
    int PelletsVisited;
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD

        Visted_Pellets = new List<Pellet>();
=======
>>>>>>> origin/main
        Agent = GetComponent<NavMeshAgent>();
        if(Agent == null)
        {
            Debug.LogError("There is no NavMeshAgent on: " + gameObject.name);
        }
        else
        {
            if(currentPellet == null)
            {
<<<<<<< HEAD
                GameObject[] PelletsGO = GameObject.FindGameObjectsWithTag("Pellet Start");
                if(PelletsGO.Length > 0)
                {
                    int random = Random.Range(0, PelletsGO.Length);
                    startingPellet = PelletsGO[random].GetComponent<Pellet>();

                    if(startingPellet != null)
                    {
                        currentPellet = startingPellet;
=======
                GameObject[] PelletsGO = GameObject.FindGameObjectsWithTag("Pellet");
                if(PelletsGO.Length > 0)
                {
                    while(currentPellet == null)
                    {
                        int random = Random.Range(0, PelletsGO.Length);
                        Pellet startingPellet = PelletsGO[random].GetComponent<Pellet>();

                        if(startingPellet != null)
                        {
                            currentPellet = startingPellet;
                        }
>>>>>>> origin/main
                    }
                }
                else
                {
                    Debug.LogError("No Waypoints found");
                }
            }
        }

        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if(traveling && Agent.remainingDistance <= 1 && !currentPellet.finished)
=======
        if(traveling && Agent.remainingDistance <= 1)
>>>>>>> origin/main
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

    private void SetDestination()
    {
        if(PelletsVisited > 0)
        {
<<<<<<< HEAD
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

=======
            Pellet nextPellet = currentPellet.GetNextPellet(previousPellet);
>>>>>>> origin/main
            previousPellet = currentPellet;
            currentPellet = nextPellet;
        }

        Vector3 Target = currentPellet.transform.position;
        Agent.SetDestination(Target);
        traveling = true;
<<<<<<< HEAD
        previousPellet.collected = true;
=======
>>>>>>> origin/main
    }
}
