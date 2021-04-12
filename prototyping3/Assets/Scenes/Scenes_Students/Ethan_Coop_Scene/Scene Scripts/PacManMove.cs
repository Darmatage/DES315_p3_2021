using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PacManMove : MonoBehaviour
{

    PelletHolder p_holder;
    GameObject[] All_Pellets;
    [SerializeField] Pellet closestPellet;

    List<Pellet> Visted_Pellets;
    [SerializeField] int finished_Pellet_count = 0, length;
    bool finished = false;

    int player = -1, starter = 0;

    [SerializeField] float anger_timer = 10f, anger_cd = 20f;

    public Text collectedPelletsText;

    public Transform player1, player2;

    NavMeshAgent Agent;
    [SerializeField] Pellet currentPellet, previousPellet, startingPellet;

    public bool traveling, attack = false;
    int PelletsVisited;
    public bool start = false;

    AudioSource nom;
    // Start is called before the first frame update
    void Start()
    {
        nom = GetComponent<AudioSource>();

        GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[0];

        p_holder = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<PelletHolder>();
        All_Pellets = p_holder.GetPellets();

        length = GameObject.FindGameObjectsWithTag("Pellet").Length;

        Visted_Pellets = new List<Pellet>();
        Agent = GetComponent<NavMeshAgent>();
        if (Agent == null)
        {
            Debug.LogError("There is no NavMeshAgent on: " + gameObject.name);
        }
        else
        {
            if (currentPellet == null)
            {
                GameObject[] PelletsGO = GameObject.FindGameObjectsWithTag("Pellet Start");
                if (PelletsGO.Length > 0)
                {
                    int random = Random.Range(0, PelletsGO.Length);
                    startingPellet = PelletsGO[random].GetComponent<Pellet>();

                    if (startingPellet != null)
                    {
                        currentPellet = startingPellet;
                        PelletsVisited++;
                        //SetDestination(false);
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
        //Start
        if (start)
        {
            SetDestination(false);
            start = false;
            starter++;
        }

        //Get player transforms
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

        //Chase player logic
        if (anger_cd > 0 && starter > 0)
        {
            anger_cd -= Time.deltaTime;
        }
        else if (anger_cd < 0 && anger_timer > 0)
        {
            anger_timer -= Time.deltaTime;
            if (!attack)
            {
                GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[1];
                SetDestination(true);
            }
        }
        else if (anger_timer <= 0)
        {
            anger_cd = 30;
            anger_timer = 15;
            player = -1;
            attack = false;
            GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[0];
        }

        //Collected pellet code
        if (collectedPelletsText != null)
        {
            collectedPelletsText.text = "Pellets Collected: " + finished_Pellet_count + "/" + length;
        }

        //Win condition
        if (finished_Pellet_count >= length)
        {
            GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().coopMonsterWins = true;
            StartCoroutine(GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().CoopEndGame());
        }

        //Set new destinations
        if (traveling && Agent.remainingDistance <= 1 && !finished && !attack)
        {
            traveling = false;
            PelletsVisited++;

            SetDestination(false);
        }

    }

    public void Begin()
    {
        start = true;
    }

    private void SetDestination(bool chase)
    {
        if (chase)
        {
            if (player == -1)
            {
                player = Random.Range(0, 2);
            }
            if (player == 0)
            {
                Agent.SetDestination(player1.position);
            }
            else if (player == 1)
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
                int count = 0;
                do
                {
                    count++;
                    nextPellet = GetNextPellet(previousPellet, player1, player2);
                    if (count >= 5) //infinite loop break
                    {
                        break;
                    }
                }
                while (Visted_Pellets.Contains(nextPellet));


                currentPellet = nextPellet;
            }

            Vector3 Target = currentPellet.transform.position;
            Agent.SetDestination(Target);
            traveling = true;
            if (previousPellet && !previousPellet.collected && !attack)
            {
                previousPellet.collected = true;
                finished_Pellet_count++;
                previousPellet.gameObject.GetComponent<MeshRenderer>().enabled = false;
                nom.PlayOneShot(nom.clip);
            }
        }
    }


    public Pellet GetNextPellet(Pellet PreviousPellet, Transform player1, Transform player2)
    {
        if (previousPellet.Pellet_List.Count == 0)
        {
            return null;
        }
        else if (previousPellet.Pellet_List.Count == 1 && previousPellet.Pellet_List.Contains(PreviousPellet))
        {
            return PreviousPellet;
        }
        else
        {
            Pellet nextPellet = null;
            closestPellet = ClosestPellet(PreviousPellet);

            for (int i = 0; i < previousPellet.Pellet_List.Count; i++)
            {
                if (!previousPellet.Pellet_List[i].collected)
                {
                    nextPellet = previousPellet.Pellet_List[i];
                }
                else
                {
                    nextPellet = closestPellet;
                }
                //else if (Vector3.Distance(previousPellet.Pellet_List[i].transform.position, player1.position) > distance)
                //{
                //    distance = Vector3.Distance(previousPellet.Pellet_List[i].transform.position, player1.position);
                //    nextPellet = previousPellet.Pellet_List[i];
                //}
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
            //nextPellet = previousPellet.Pellet_List[Random.Range(0, previousPellet.Pellet_List.Count)];

            return nextPellet;
        }
    }

    Pellet ClosestPellet(Pellet currentPellet) //finds the closest uncollected Pellet
    {

        List<Pellet> Uncollected_Pellets = new List<Pellet>();

        for (int i = 0; i < All_Pellets.Length; i++)
        {
            if (!All_Pellets[i].GetComponent<Pellet>().collected)
                Uncollected_Pellets.Add(All_Pellets[i].GetComponent<Pellet>());
        }

        float distance = float.MaxValue;
        Pellet closestPellet = null;
        for (int i = 0; i < Uncollected_Pellets.Count; i++)
        {
            if (Vector3.Distance(Uncollected_Pellets[i].gameObject.transform.position, currentPellet.gameObject.transform.position) < distance)
            {
                distance = Vector3.Distance(Uncollected_Pellets[i].gameObject.transform.position, currentPellet.gameObject.transform.position);
                closestPellet = Uncollected_Pellets[i];
            }
        }



        return closestPellet;
    }
}
