using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B05_AI : MonoBehaviour
{
    private string thisPlayer;

    private NavMeshAgent agent;
    private GameObject enemy;
    private Transform enemy_trans;
    private B05_Node topNode;

    [SerializeField] private float ideal_distance;
    [SerializeField] private float flee_distance;
    public float distance_goal;

    public MeshRenderer mesh;
    public Material matdef;
    public Material matmove;
    public Material matrush;
    //public Transform goalsphere;
    public bool low_health;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        distance_goal = ideal_distance;
        low_health = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // find opponent
        if (gameObject.transform.root.tag == "Player1")
        {
            // opponent is Player 2
            enemy = GameObject.FindWithTag("Player2");
        }
        else
        {
            // opponent is Player 1
            enemy = GameObject.FindWithTag("Player1");
        }

        enemy_trans = enemy.transform.GetChild(0);

        thisPlayer = gameObject.transform.root.tag;

        ConstructBehaviorTree();
    }

    private void ConstructBehaviorTree()
    {
        // idle branch
        B05N_Idle idle_node = new B05N_Idle(GetComponent<Bot05_Move>());
        B05N_FaceEnemy turn_node = new B05N_FaceEnemy(enemy_trans, GetComponent<Bot05_Move>());
        B05_UIgnoreFailure ignore_turn = new B05_UIgnoreFailure(turn_node);
        B05_USim idle_branch = new B05_USim(new List<B05_UNode> { ignore_turn, idle_node });

        // move branch
        B05N_Move move_node = new B05N_Move(enemy_trans, agent, this);
        B05_USim moveAndTurn = new B05_USim(new List<B05_UNode> { move_node, turn_node });

        // blade rush branch
        B05N_BladeRush rush_node = new B05N_BladeRush(GetComponent<B05_BladeRush>(), GetComponent<Bot05_Move>(), enemy_trans, this);
        B05_USequence rush_seq = new B05_USequence(new List<B05_UNode> { ignore_turn, rush_node });

        // shoot branch
        B05N_Shoot shoot_node = new B05N_Shoot(enemy_trans, enemy.GetComponentInChildren<Rigidbody>(),
                                               GetComponent<Bot05_Move>(), GetComponent<B05_ShootTop>());

        // pound branch
        B05N_Pound pound_node = new B05N_Pound(GetComponent<B05_GroundPound>(), GetComponent<Bot05_Move>(), this);

        // mag branch
        B05N_Mag mag_node = new B05N_Mag(GetComponent<B05_MagneticForce>(), GetComponent<Bot05_Move>().center_pt, enemy_trans);
        //B05_USim mag_branch = new B05_USim(new List<B05_UNode> { ignore_turn, mag_node });

        // utility node 
        B05_UtilitySelector usel = new B05_UtilitySelector(new List<B05_UNode> { idle_branch, moveAndTurn, rush_seq, shoot_node, pound_node, mag_node });

        // jump node
        B05N_Jump jump_node = new B05N_Jump(GetComponent<Bot05_Move>());

        // turtling node
        topNode = new B05_GroundedSelector(usel, jump_node, GetComponent<Bot05_Move>());
    }

    // Update is called once per frame
    void Update()
    {
        mesh.material = matdef;

        if (agent.enabled && !agent.isOnNavMesh)
            return;

        if (thisPlayer.Equals("Player1"))
        {
            low_health = (GameHandler.p1Health <= 8);
        }
        else
        {
            low_health = (GameHandler.p2Health <= 8);
        }

        if (low_health)
        {
            this.distance_goal = flee_distance;
        }

        topNode.Evaluate();
        //if (topNode.nodeState == NodeState.FAILURE)
            //Debug.LogError("Tree returned failure!");
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}
