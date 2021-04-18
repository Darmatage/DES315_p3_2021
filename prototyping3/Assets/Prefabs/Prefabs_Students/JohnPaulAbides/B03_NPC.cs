using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class B03_NPC : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Transform target;
    public float attackRange = 10.0f;
    public Rigidbody rigid;
    public GameObject shotPrefab;
    public GameObject damageTextPrefab;
    public GameObject hitbox;
    public Transform webShot = null;
    public int startingHealth;
    public Slider healthBar;

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

        // update health
        healthBar.maxValue = startingHealth;
        healthBar.value = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLoader.playersReady == true)
        {
            GameObject player = GameObject.FindWithTag("Player1");
            if (player != null && GameObject.FindWithTag("Player1").transform.childCount >= 1)
                player1 = GameObject.FindWithTag("Player1").transform.GetChild(0);

            player = GameObject.FindWithTag("Player2");
            if (player != null && GameObject.FindWithTag("Player2").transform.childCount >= 1)
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
                Chase();
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
        rigid.MoveRotation(Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f));

        if (timer >= 1.0f)
        {
            timer = 0.0f;
            startingPos = transform.position;
            action = Action.CHASE;
        }
    }

    private void Chase()
    {
        myAgent.enabled = true;
        timer += Time.fixedDeltaTime;

        // select target
        if (player1 != null && player2 == null) // only player1 exists
            target = player1;
        else if (player2 != null && player1 == null) // only player2 exists
            target = player2;
        else if (player1 != null && player2 != null ) // both players exist
        {
            if (Vector3.SqrMagnitude(player1.position - transform.position) < Vector3.SqrMagnitude(player2.position - transform.position))
                target = player1;
            else
                target = player2;
        }

        // move to target
        myAgent.destination = target.position;

        // if target is in range, start attack
        if (Vector3.Distance(transform.position, target.position) < attackRange && timer > 1.0f)
        {
            timer = 0.0f;
            startingPos = transform.position;
            action = Action.ATTACK;
        }
    }

    private void Attack()
    {
        // monster leaps into the air and aims its back towards target
        rigid.isKinematic = true;
        myAgent.enabled = false;

        timer += Time.fixedDeltaTime;

        // start jump
        if (timer < 1.0f)
        {
            float lerpPercent = jumpHeightCurve.Evaluate(timer);
            float lerpTiltPercent = jumpAimCurve.Evaluate(timer);
            rigid.MovePosition(Vector3.Lerp(startingPos, startingPos + Vector3.up * jumpHeight, lerpPercent));
            Vector3 dir;
            if (target == null)
                dir = (startingPos + transform.forward * 8.0f) - (startingPos + Vector3.up * jumpHeight);
            else
                dir = target.position - (startingPos + Vector3.up * jumpHeight);
            Quaternion dirAngle = Quaternion.LookRotation(dir);
            rigid.MoveRotation(Quaternion.Euler(dirAngle.eulerAngles.x - 175.0f * lerpTiltPercent, dirAngle.eulerAngles.y, transform.eulerAngles.z));
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
                    action = Action.STUNNED;
                    rigid.MoveRotation(Quaternion.Euler(-175.0f, transform.eulerAngles.y, 0.0f));
                    hitbox.SetActive(false);
                }
                else // shot is still out
                {
                    // shot has made contact, begin pull
                    if (webPullFlag)
                    {
                        hitbox.SetActive(true);
                        Vector3 dir = (webShot.position - transform.position).normalized;
                        rigid.MovePosition(transform.position + dir * 40.0f * Time.fixedDeltaTime);
                        rigid.MoveRotation(Quaternion.Euler(transform.eulerAngles + Vector3.forward * 2160.0f * Time.fixedDeltaTime));
                    }
                    else // shot is still moving, move out
                    {
                        Vector3 dir = (webShot.position - transform.position).normalized;
                        rigid.MovePosition(transform.position - dir * 5.0f * Time.fixedDeltaTime);
                    }
                }
            }

            // spent too much time shooting, return to idle
            if (timer > 3.0f)
            {
                Destroy(webShot.gameObject);
                webShot = null;
                webShotFlag = false;
                timer = 0.0f;
                action = Action.STUNNED;
                rigid.MoveRotation(Quaternion.Euler(-175.0f, transform.eulerAngles.y, 0.0f));
                hitbox.SetActive(false);
            }
        }
    }

    private void Stunned()
    {
        rigid.isKinematic = false;
        timer += Time.fixedDeltaTime;
        if (timer >= 4.0f)
        {
            timer = 0.0f;
            action = Action.IDLE;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Hazard") && (other.gameObject.GetComponent<HazardDamage>().isMonsterWeapon == false))
        {
            int damage = (int)other.gameObject.GetComponent<HazardDamage>().damage;
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        // minimum 1 damage
        if (damage == 0) damage = 1;

        healthBar.value -= damage;
        if (healthBar.value <= 0)
        {
            healthBar.value = 0;
            Destroy(gameObject);
        }
        Text damageText = Instantiate(damageTextPrefab, transform.position, transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f)).GetComponentInChildren<Text>();
        damageText.text = damage.ToString();
    }

    private void OnDestroy()
    {
        if (webShot != null)
        {
            Destroy(webShot.gameObject);
        }
    }
}
