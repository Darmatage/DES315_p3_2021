using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class botA03_NPC_Movement : MonoBehaviour
{
    public float move_speed = 10f;
    public float rotate_speed = 100f;
    public float jump_speed = 5f;
    public float flip_speed = 150f;
    public float boost_speed = 10f;
    public float distance;

    public string parent_name;
    public string parent_vertical;
    public string parent_horizontal;
    public string parent_jump;

    public Transform ground_check;
    public Transform turtle_check;
    public LayerMask ground_layer;
    public bool is_grounded;
    public bool is_turtled;

    private Rigidbody rigidbody;
    private GameObject enemy;
    private NavMeshAgent nm_agent;

    private RaycastHit hit;
    private float floor_distance = 1f;
    
    
    private botA03_NPC_Weapons weapon;
    
    void Start()
    {
        weapon = GetComponent<botA03_NPC_Weapons>();

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
        if (nm_agent == null)
            nm_agent = GetComponent<NavMeshAgent>();
        
        if (enemy)
        {
            /*
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
            }*/
            Movement();
        }
        else
        {
            enemy = EnemySetup();
        }

    }

    private void Movement()
    {
        is_grounded = Physics.CheckSphere(ground_check.position, 0.2f, ground_layer);
        is_turtled = Physics.CheckSphere(turtle_check.position, 0.4f, ground_layer);

        if ((Vector3.Dot(transform.up, Vector3.down) > 0) && Physics.Raycast(transform.position, Vector3.down, out hit, floor_distance))
        {
            if (is_grounded == true)
            {
                rigidbody.AddForce(rigidbody.centerOfMass + new Vector3(0f, jump_speed*10, 0f), ForceMode.Impulse);
            }
        
            if (is_turtled == true)
            {
                rigidbody.AddForce(rigidbody.centerOfMass + new Vector3(jump_speed / 2, 0, jump_speed / 2), ForceMode.Impulse);
                transform.Rotate(150f, 0, 0);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            else
            {
                Vector3 betterEulerAngles = new Vector3(gameObject.transform.parent.eulerAngles.x, transform.eulerAngles.y, gameObject.transform.parent.eulerAngles.z); 
                transform.eulerAngles = betterEulerAngles;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        
        Vector3 destination = enemy.transform.position;

        if ((weapon.DashReady() == false) && (weapon.WallReady() == false))
        {
            
            
            var distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < 7f)
            {
                if (weapon.SmokeReady() == true)
                {
                    weapon.SmokeOn();
                }
                //Vector3 escape_direction = Random.insideUnitSphere * 15;
                //NavMeshHit navmesh_hit;
                //if (NavMesh.SamplePosition(transform.position + escape_direction, out navmesh_hit, 15, NavMesh.AllAreas))
                //{
                //    nm_agent.destination = navmesh_hit.position;
                    //nm_agent.Resume();
                //}
            }
            
            //Vector3 direction = Random.insideUnitSphere * 15;

            //transform.position += direction * move_speed;
            
            
                Vector3 direction = Random.insideUnitSphere * 15;
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(transform.position + direction, out navHit, 15, -1))
                {
                    nm_agent.destination = navHit.position;
                }

        }
        else if (weapon.DashReady() == true && weapon.WallReady() == false)
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < 4f)
            {
                transform.LookAt(destination);
                weapon.DashAttack();
            }

            if (weapon.SmokeReady() == true)
            {
                weapon.SmokeOn();
                weapon.SmokeWait();
            }
        }
        else if (weapon.DashReady() == false && weapon.WallReady() == true)
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < 3f)
            {
                weapon.WallAttack();
            }
            
            if (weapon.SmokeReady() == true)
            {
                weapon.SmokeOn();
                weapon.SmokeWait();
            }
        }
        else //If everything is usable
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            
            if (distance < 10f)
            {
                transform.LookAt(destination);
                weapon.DashAttack();
            }
            else
            {
                if (Physics.Raycast(transform.position, Vector3.down, out hit, floor_distance))
                {
                    transform.LookAt(destination);
                    Vector3 vel = transform.forward * move_speed;
                    vel.y = rigidbody.velocity.y;

                    rigidbody.velocity = vel;
                    
                }
            }
        }
    }
    
    private GameObject EnemySetup()
    {
        GameHandler game_handler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        string bot_tag = transform.root.tag;

        if (bot_tag == "Player1")
            return game_handler.Player2Holder.transform.GetChild(0).gameObject;
        else
            return game_handler.Player1Holder.transform.GetChild(0).gameObject;

    }
}
