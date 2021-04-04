using System.Collections;
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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
        // actions
        B05N_Move move_node = new B05N_Move(enemy_trans, agent, this);
        B05N_BladeRush rush_node = new B05N_BladeRush(GetComponent<B05_BladeRush>(), GetComponent<Bot05_Move>(), enemy_trans, this);

        // utility node
        //B05_UtilitySelector uselector 
        topNode = new B05_UtilitySelector(new List<B05_UNode> { move_node, rush_node });
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
