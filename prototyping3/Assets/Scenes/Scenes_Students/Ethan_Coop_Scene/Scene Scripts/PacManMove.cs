using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PacManMove : MonoBehaviour
{
    bool PatrolWaiting = false;
    float PatrolWaitTime = 3.0f;

    List<Pellet> Visted_Pellets;

    NavMeshAgent Agent;
    [SerializeField] Pellet currentPellet, previousPellet, startingPellet;

    bool traveling, waiting;
    float waitTimer;
    int PelletsVisited;
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

        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if(traveling && Agent.remainingDistance <= 1 && !currentPellet.finished)
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
        previousPellet.collected = true;
    }
}
