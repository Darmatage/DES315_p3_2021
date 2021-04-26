using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuileBot_AI : MonoBehaviour
{
    public GameObject[] paths;

    public NavMeshAgent agent;
    public BotBasic_Move move;
    public Rigidbody rb;
    private GameObject enemyBot;
    public OK_SonicBoom weaponSystem;
    public float range = 10;
    private Vector3 escapePosition;
    private bool escaping = false;
    private int escapeCounter = 0;

    private float elapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        weaponSystem.flashCooldown = 2.0f;
        GameHandler gh = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        string playerTag = transform.root.tag;
        if (playerTag.Contains("1"))
            enemyBot = gh.Player2Holder.transform.GetChild(0).gameObject;
        if (playerTag.Contains("2"))
            enemyBot = gh.Player1Holder.transform.GetChild(0).gameObject;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.isOnNavMesh)
            return;


        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.50f)
        {
            elapsed = 0.0f;        
            if (escaping == false)
            {
                agent.SetDestination(enemyBot.transform.position);

            }
            if (escaping == true)
            {
                agent.SetDestination(escapePosition); 
                escapeCounter++;
            }
            if (escapeCounter > 3 || !agent.hasPath || Vector3.Distance(transform.position,escapePosition) < 3)
            {
                escapeCounter = 0;
                escaping = false;
            }
        }

        if (move.isTurtled && move.canFlip)
        {
            rb.AddForce(rb.centerOfMass + new Vector3(move.jumpSpeed / 2, 0, move.jumpSpeed / 2), ForceMode.Impulse);
            transform.Rotate(150f, 0, 0);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        
        float distance = Vector3.Distance(enemyBot.transform.position, this.transform.position);
        Vector3 connection = enemyBot.transform.position - this.transform.position;

        float angle = Mathf.Acos(Vector3.Dot(connection, this.transform.forward) / (connection.magnitude * transform.forward.magnitude));
        angle = Mathf.Rad2Deg * angle;

        if (!(angle > 30 || (Vector3.Dot(connection, this.transform.forward) < 0)))
        {
            weaponSystem.sonicBoomattack();
        }


        if (distance < 5)
        {
            if (!escaping)
            {
                weaponSystem.flashKickAttack();

                Vector3 randomPos = Random.insideUnitSphere * range;
                randomPos.y = transform.position.y;

                NavMeshHit hit; // NavMesh Sampling Info Container

                // from randomPos find a nearest point on NavMesh surface in range of maxDistance
                NavMesh.SamplePosition(randomPos, out hit, range * 10, NavMesh.AllAreas);
                escapePosition = hit.position;
                escaping = true;
            }
        }

    }
}
