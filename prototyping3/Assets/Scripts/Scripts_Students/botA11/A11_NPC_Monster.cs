using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Amogh
{
    public class A11_NPC_Monster : MonoBehaviour
    {
        public float playerLongAttackDistance = 35f; 
        public float playerShortAttackDistance = 10f; 
        public float patrolSwitchThreshold1 = 8f;
        public float patrolSwitchThreshold2 = 30f;
        public float turnThreshold = 40f;
        
        public Transform [] patrolTargets;

        private float distToPlayer1;
        private float distToPlayer2;

        private Transform player1Target;
        private Transform player2Target;
        
        private NPC_LoadPlayers playerLoader;
        private NPC_Damage damageManager;
        
        private NavMeshAgent agent;
        private Animator anim;

        private A11_IAttack[] attackInterfaces;

        private bool isPatrolling;

        private bool jukeBoxReady = true;
        void Start()
        {
            playerLoader = GetComponent<NPC_LoadPlayers>();
            agent = GetComponent<NavMeshAgent>();
            damageManager = GetComponent<NPC_Damage>();
            anim = GetComponentInChildren<Animator>();
            
            attackInterfaces = new A11_IAttack[3];
            attackInterfaces[0] = GetComponentInChildren<A11_Heal>();
            attackInterfaces[1] = GetComponentInChildren<A11_ChargeAttack>();
            attackInterfaces[2] = GetComponentInChildren<A11_JukeBox>();
            
            //InvokeRepeating(nameof(LoadPlayerTargets), 2f, 4f);
            //InvokeRepeating(nameof(UpdatePlayerDistances), 5f, 0.5f);
            
            StartCoroutine(BehaviorTree());
            StartCoroutine(Patrol());
            isPatrolling = true;
            anim.SetBool("Patrol", isPatrolling);
        }

        IEnumerator BehaviorTree()
        {
            yield return new WaitUntil( () => playerLoader.playersReady);
            UpdatePlayerDistances();
            
            while (true)
            {
                if (playersWithinRange())
                {
                    if (isPatrolling)
                    {
                        StopCoroutine(Patrol());
                        isPatrolling = false;
                        anim.SetBool("Patrol", isPatrolling);
                    }

                    if (distToPlayer1 <= playerLongAttackDistance && distToPlayer1 >= playerShortAttackDistance)
                        RangedAttack(player1Target.position);
                    else if (distToPlayer2 <= playerLongAttackDistance && distToPlayer2 >= playerShortAttackDistance)
                        RangedAttack(player2Target.position);
                    else if (jukeBoxReady && (distToPlayer1 <= playerShortAttackDistance ||
                                              distToPlayer2 <= playerShortAttackDistance))
                    {
                        StartCoroutine(ShortAttack());
                        isPatrolling = true;
                        StartCoroutine(Patrol());
                        agent.speed = Mathf.Clamp(agent.speed * 3.5f, 0f,35f);
                        anim.SetBool("Patrol", isPatrolling);
                        yield return  new WaitForSeconds(1.5f);
                        agent.speed /= 2;
                    }
                    
                    //if (NPC_Damage.health <= 30f)
                      //  attackInterfaces[0].ButtonDown();
                }
                else
                {
                    if (isPatrolling == false)
                    {
                        isPatrolling = true;
                        StartCoroutine(Patrol());
                        anim.SetBool("Patrol", isPatrolling);
                        yield return new WaitForSeconds(2f);
                    }
                }

                yield return new WaitForSeconds(1f);
                UpdatePlayerDistances();
            }

        }

        private void UpdatePlayerDistances()
        {
            LoadPlayerTargets();
            distToPlayer1 = Vector3.Distance(player1Target.position, gameObject.transform.position);
            distToPlayer2 = Vector3.Distance(player2Target.position, gameObject.transform.position);
        }

        private bool playersWithinRange()
        {
            if (distToPlayer1 <= playerLongAttackDistance || distToPlayer2 <= playerLongAttackDistance)
                return true;
            
            return false;
        }

        IEnumerator Patrol()
        {
            while (true)
            {
                agent.SetDestination(patrolTargets[Random.Range(0, patrolTargets.Length)].position);
                yield return new WaitForSeconds(2f);
            }
        }
        
        void Update()
        {
            
        }

        private void RangedAttack(Vector3 target)
        {
            transform.LookAt(target);
            var abc = (A11_ChargeAttack) (attackInterfaces[1]);
            abc.SetTarget(target);
            attackInterfaces[1].ButtonDown();
        }

        private IEnumerator ShortAttack()
        {
            Debug.Log("Short attack disco ball");
            attackInterfaces[2].ButtonDown();
            anim.SetBool("Jump", true);
            jukeBoxReady = false;
            
            yield return new WaitForSeconds(Random.Range(10f, 12f));
            attackInterfaces[2].ButtonUp();
            anim.SetBool("Jump", false);

            // Cooldown
            yield return new WaitForSeconds(7f);
            jukeBoxReady = true;
        }
        
        
        public void LoadPlayerTargets()
        {
            //load players as targets when they appear:
            if (player1Target == null && GameObject.FindWithTag("Player1") && GameObject.FindWithTag("Player1").transform.GetChild(0) != null) { 
                player1Target = GameObject.FindWithTag("Player1").transform.GetChild(0).GetComponent<Transform>(); 
            }
            if (player2Target == null && GameObject.FindWithTag("Player2") && GameObject.FindWithTag("Player2").transform.GetChild(0) != null) { 
                player2Target = GameObject.FindWithTag("Player2").transform.GetChild(0).GetComponent<Transform>(); 
            }
        }
    }
}