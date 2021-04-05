﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B05_AI : MonoBehaviour
{
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
    public Transform goalsphere;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        distance_goal = ideal_distance;
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

        ConstructBehaviorTree();
    }

    private void ConstructBehaviorTree()
    {
        // move branch
        B05N_FaceEnemy turn_node = new B05N_FaceEnemy(enemy_trans, GetComponent<Bot05_Move>());
        B05N_Move move_node = new B05N_Move(enemy_trans, agent, this);
        B05_USim moveAndTurn = new B05_USim(new List<B05_UNode> { move_node, turn_node });

        // blade rush branch
        B05_UIgnoreFailure ignore_turn = new B05_UIgnoreFailure(turn_node);
        B05N_BladeRush rush_node = new B05N_BladeRush(GetComponent<B05_BladeRush>(), GetComponent<Bot05_Move>(), enemy_trans, this);
        B05_USequence rush_seq = new B05_USequence(new List<B05_UNode> { ignore_turn, rush_node });

        // utility node 
        topNode = new B05_UtilitySelector(new List<B05_UNode> { moveAndTurn, rush_seq });
    }

    // Update is called once per frame
    void Update()
    {
        mesh.material = matdef;

        if (!agent.isOnNavMesh)
            return;

        topNode.Evaluate();
        if (topNode.nodeState == NodeState.FAILURE)
            Debug.LogError("Tree returned failure!");
    }
}
