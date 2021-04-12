using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class botA03_NPC_Movement : MonoBehaviour
{
    public float move_speed = 10f;
    public float rotate_speed = 100f;
    public float jump_speed = 5f;
    public float boost_speed = 10f;
    public float distance;

    public string parent_name;
    public string parent_vertical;
    public string parent_horizontal;
    public string parent_jump;

    private Rigidbody rigidbody;
    private GameObject enemy;
    private NavMeshAgent nm_agent;

    private botA03_Weapons weapon;
    
    void Start()
    {
        weapon = GetComponent<botA03_Weapons>();

        parent_name = this.transform.parent.gameObject.name;
        parent_vertical = gameObject.transform.parent.GetComponent<playerParent>().moveAxis;
        parent_horizontal = gameObject.transform.parent.GetComponent<playerParent>().rotateAxis;
        parent_jump = gameObject.transform.parent.GetComponent<playerParent>().jumpInput;

        enemy = EnemySetup();
        nm_agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        float movement = Input.GetAxisRaw(parent_vertical) * move_speed * Time.deltaTime;
        float rotation = Input.GetAxisRaw(parent_horizontal) * rotate_speed * Time.deltaTime;

        if (nm_agent == null)
            nm_agent = GetComponent<NavMeshAgent>();
        else if (enemy)
        {
            Vector3 destination = enemy.transform.position;
            destination.y = transform.position.y;

            if (Vector3.Distance(destination, transform.position) < 10)
            {
                nm_agent.enabled = false;
            }
            else if(Vector3.Distance(destination, transform.position) > 2)
            {
                transform.LookAt(destination);
                Vector3 velocity = transform.forward * move_speed;
                velocity.y = rigidbody.velocity.y;
				
                rigidbody.velocity = velocity;
            }
        }
        else
        {
            enemy = EnemySetup();

            //if (Vector3.Distance(transform.position, nm_agent.destination) < 6)
            //{
            //    Vector3 direction = Random.insideUnitSphere * distance;
            //    NavMeshHit navHit;
            //    if (NavMesh.SamplePosition(transform.position + direction, out navHit, distance, -1))
            //    {
            //        nm_agent.destination = navHit.position;
            //    }
            //}
        }
    }

    private GameObject EnemySetup()
    {
        GameHandler game_handler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        string bot_tag = transform.root.tag;

        if (bot_tag == "Player1")
            return game_handler.Player2Holder.transform.GetChild(0).gameObject;
        else if (bot_tag == "Player2")
            return game_handler.Player1Holder.transform.GetChild(0).gameObject;
        else
            return null;
        
    }
}
