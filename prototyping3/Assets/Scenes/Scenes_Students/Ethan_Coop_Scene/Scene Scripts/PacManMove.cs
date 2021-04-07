using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PacManMove : MonoBehaviour
{
    bool PatrolWaiting = false;
    float PatrolWaitTime = 3.0f;

    public float health = 100;

    List<Pellet> Visted_Pellets;
    [SerializeField] int finished_Pellet_count = 0, length;
    bool finished = false;

    int player = -1;

    [SerializeField] float anger_timer = 15f, anger_cd = 30f;

    public Text collectedPelletsText;

    Transform player1, player2;

    NavMeshAgent Agent;
    [SerializeField] Pellet currentPellet, previousPellet, startingPellet;

    bool traveling, waiting, attack = false;
    float waitTimer;
    int PelletsVisited;
    public bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        

        length = GameObject.FindGameObjectsWithTag("Pellet").Length;

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
        if (player1 == null)
        {
            for (int i = 0; i < GameObject.FindGameObjectWithTag("Player1").transform.childCount; i++)
            {
                string childname = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(i).name;
                string prefabname = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().p1PrefabName + "(Clone)";
                //print("child name: " + childname + "    prefabname: " + prefabname);
                if (childname == prefabname)
                {
                    // print("Foudn Player 1");
                    player1 = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(i);
                }

            }
        }
        if (player2 == null)
        {
            for (int i = 0; i < GameObject.FindGameObjectWithTag("Player2").transform.childCount; i++)
            {
                string childname = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(i).name;
                string prefabname = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().p2PrefabName + "(Clone)";
                //print("child name: " + childname + "    prefabname: " + prefabname);
                if (childname == prefabname)
                {
                    // print("Foudn Player 1");
                    player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(i);
                }

            }
        }

        if (anger_cd > 0)
        {
            anger_cd -= Time.deltaTime;
        }
        else if( anger_cd < 0 && anger_timer > 0)
        {
            anger_timer -= Time.deltaTime;
            if (attack == false)
                SetDestination(true);
        }
        else if(anger_timer <= 0)
        {
            anger_cd = 30;
            anger_timer = 15;
            player = -1;
            attack = false;
        }

        if(collectedPelletsText != null)
        {
            collectedPelletsText.text = "Pellets Collected: " + finished_Pellet_count + "/" + length;
        }

        if (finished_Pellet_count >= length)
        {
            GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().coopMonsterWins = true;
            StartCoroutine(GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().CoopEndGame());
        }

        if (start)
        {
            SetDestination(false);
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
                SetDestination(false);
        }

        if(waiting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer >= PatrolWaitTime)
            {
                waiting = false;
                SetDestination(false);
            }
        }
    }

    public void Begin()
    {
        start = true;
    }

    private void SetDestination(bool chase)
    {
        if(chase)
        {
            if(player == -1)
            {
                player = Random.Range(0, 1);
            }
            else if(player == 0)
            {
                Agent.SetDestination(player1.position);
            }
            else if(player == 1)
            {
                Agent.SetDestination(player2.position);
            }
            attack = true;
        }
        else
        {
            if (PelletsVisited > 0)
            {
                previousPellet = currentPellet;

                Visted_Pellets.Add(currentPellet);
                if (Visted_Pellets.Count >= 5)
                {
                    Visted_Pellets.RemoveAt(0);
                }
                Pellet nextPellet = null;
                do
                {
                    nextPellet = currentPellet.GetNextPellet(previousPellet);
                }
                while (Visted_Pellets.Contains(nextPellet));


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
}
