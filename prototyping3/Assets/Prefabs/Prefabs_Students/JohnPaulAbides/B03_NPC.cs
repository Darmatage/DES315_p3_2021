using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class B03_NPC : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Transform target;
    public float attackRange = 10.0f;
    public Rigidbody rigid;
    public GameObject shotPrefab;
    public Transform webShot = null;

    // attack animation
    public float jumpHeight;
    public AnimationCurve jumpHeightCurve;
    public AnimationCurve jumpAimCurve;
    public Transform shotOrigin;

    private NavMeshAgent myAgent;
    private NPC_LoadPlayers playerLoader;

    private enum Action { IDLE, CHASE, ATTACK, STUNNED }
    private Action action = Action.IDLE; // monster can only do one attack at a time

    // arbitrary values used in actions
    private float timer;
    private int phase;
    private Vector3 startingPos;
    private bool webShotFlag;
    public bool webPullFlag;

    // Start is called before the first frame update
    void Start()
    {
        // retrieve components
        rigid = GetComponent<Rigidbody>();
        myAgent = GetComponent<NavMeshAgent>();
        playerLoader = GetComponent<NPC_LoadPlayers>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLoader.playersReady == true)
        {
            player1 = GameObject.FindWithTag("Player1").transform.GetChild(0);
            player2 = GameObject.FindWithTag("Player2").transform.GetChild(0);
        }
    }

    private void FixedUpdate()
    {
        // update actions
        switch (action)
        {
            case Action.IDLE:
                Idle();
                break;
            case Action.CHASE:
                break;
            case Action.ATTACK:
                Attack();
                break;
            case Action.STUNNED:
                Stunned();
                break;
        }
    }

    private void Idle()
    {
        timer += Time.fixedDeltaTime;
        transform.position = new Vector3(-9.0f, 0.0f, 2.0f);
        transform.rotation = Quaternion.identity;

        if (timer >= 1.0f)
        {
            timer = 0.0f;
            startingPos = transform.position;
            action = Action.ATTACK;
        }
    }

    private void Chase()
    {
        // select target
        if (Vector3.SqrMagnitude(player1.position - transform.position) < Vector3.SqrMagnitude(player2.position - transform.position))
        {
            target = player1;
        }
        else
        {
            target = player2;
        }    

        // move to target
        myAgent.destination = target.position;

        // if target is in range, start attack
        if (Vector3.Distance(transform.position, target.position) < attackRange)
        {
            action = Action.ATTACK;
        }
    }

    private void Attack()
    {
        // monster leaps into the air and aims its back towards target
        rigid.isKinematic = true;

        timer += Time.fixedDeltaTime;

        if (timer < 1.0f)
        {
            float lerpPercent = jumpHeightCurve.Evaluate(timer);
            float lerpTiltPercent = jumpAimCurve.Evaluate(timer);
            transform.position = (Vector3.Lerp(startingPos, startingPos + Vector3.up * jumpHeight, lerpPercent));
            Vector3 dir = target.position - transform.position;
            transform.rotation = (Quaternion.Euler(Quaternion.LookRotation(dir).eulerAngles.x - 180.0f * lerpTiltPercent, Quaternion.LookRotation(dir).eulerAngles.y, transform.rotation.eulerAngles.z));
        }
        else // fire webshot
        {
            if (!webShotFlag)
            {
                GameObject shot = Instantiate(shotPrefab, shotOrigin.position, transform.rotation * Quaternion.Euler(180.0f, 0.0f, 0.0f));
                webShot = shot.transform;
                B03_NPCShot shotComponent = shot.GetComponent<B03_NPCShot>();
                shotComponent.origin = shotOrigin;
                shotComponent.parent = this;
                webShotFlag = true;
            }
            else
            {
                // web reached, begin stun
                if (webShot == null)
                {
                    webShotFlag = false;
                    timer = 0.0f;
                    action = Action.IDLE;
                }

                if (webPullFlag)
                {
                    Vector3 dir = (webShot.position - transform.position).normalized;
                    rigid.MovePosition(transform.position + dir * 30.0f * Time.fixedDeltaTime);
                }
            }

            // spent too much time shooting, return to idle
            if (timer > 3.0f)
            {
                webShotFlag = false;
                timer = 0.0f;
                action = Action.IDLE;
            }
        }
    }

    private void Stunned()
    {

    }
}
