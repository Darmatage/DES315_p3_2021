using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PacManMove : MonoBehaviour
{
    bool PatrolWaiting = false;
    float PatrolWaitTime = 3.0f;

    float switchProbability = 0.2f;

    NavMeshAgent Agent;
    Pellet currentPellet, previousPellet;

    bool traveling, waiting;
    float waitTimer;
    int PelletsVisited;
    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        if(Agent == null)
        {
            Debug.LogError("There is no NavMeshAgent on: " + gameObject.name);
        }
        else
        {
            if(currentPellet == null)
            {
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
        if(traveling && Agent.remainingDistance <= 1)
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
            Pellet nextPellet = currentPellet.GetNextPellet(previousPellet);
            previousPellet = currentPellet;
            currentPellet = nextPellet;
        }

        Vector3 Target = currentPellet.transform.position;
        Agent.SetDestination(Target);
        traveling = true;
    }
}
