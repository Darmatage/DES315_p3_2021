﻿using UnityEngine;
using Random = UnityEngine.Random;

public class botB01_RocketHoming : MonoBehaviour
{
    public Transform Target;
    public MeshRenderer ball;

    [Range(0, 10)] public float Speed;
    [Range(0, 10)]  public float AngularSpeed;
    public Vector3 velocity;

    [Range(0, 500)] public float Knockback = 200;
    
    [Range(0, 5)] public float WarmUpTime;
    private float warmUpTimer = 0.0f;

    [Range(0, 10)] public float Damage;
    
    private Collider col;
    public botB01_RocketLauncher scrLauncher;
    public botB01_AttackState state;

    private LineRenderer line;
    private Material lineMat;
    public Transform tip;

    private AudioSource beeper;
    [Range(0, 5)] public float BeepTime;
    private float beepTimer = 0;
    
    // Start is called before the first frame update
    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        
        state = botB01_AttackState.stage_0;
        velocity = Vector3.up + Random.insideUnitSphere * 0.1f;

        line = GetComponent<LineRenderer>();
        lineMat = line.material;
        line.enabled = false;

        beeper = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Target == null)
            return;

        if (state == botB01_AttackState.stage_0)
        {
            warmUpTimer += Time.deltaTime;
            if (warmUpTimer < WarmUpTime)
                transform.LookAt(transform.position + Vector3.up);
            else
            {
                col.enabled = true;
                state = botB01_AttackState.stage_1;
                line.enabled = true;
            }
        }
        else if (state == botB01_AttackState.stage_1)
        {
            Vector3 acceleration = (Target.position - transform.position).normalized * Time.deltaTime;
            velocity = (velocity + acceleration * AngularSpeed).normalized;
            transform.LookAt(transform.position + velocity);
            
            line.SetPosition(0, tip.position);
            line.SetPosition(1, Target.position);

            Color c = Color.red;
            foreach (RaycastHit hit in Physics.RaycastAll(tip.position, Target.position - tip.position))
            {
                if (hit.transform.root.CompareTag(scrLauncher.transform.root.tag))
                {
                    c = Color.green;
                    break;
                }
            }
            lineMat.color = c;

            beepTimer += Time.deltaTime;
            if (beepTimer > BeepTime)
            {
                beepTimer = 0.0f;
                beeper.Play();
            }
        }

        transform.position += velocity * (Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 dir = new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z).normalized;
        dir += Vector3.up;
        Rigidbody rb = other.transform.root.GetChild(0).GetComponent<Rigidbody>();

        BotBasic_Damage scrDam;
        Transform obj;
        if (other.transform.root.CompareTag(scrLauncher.transform.root.tag))
        {
            scrDam = scrLauncher.scrDamageSelf;
            obj = scrDam.transform;
        }
        else
        {
            scrDam = scrLauncher.scrDamage;
            obj = Target;
        }
        
        float dam = Damage;
        if (scrDam.shieldPowerTop <= 0)
        {
            if (rb != null)
                rb.AddForce(dir * Knockback, ForceMode.Impulse);
            scrLauncher.scrHandler.TakeDamage(obj.root.tag, Damage);
            scrDam.dmgParticlesTop.SetActive(true);
        }
        else
        {
            scrDam.shieldPowerTop -= dam;
            if (scrDam.shieldPowerTop <= 0)
            {
                scrDam.shieldPowerTop = 0;
                scrLauncher.scrHandler.PlayerShields(obj.root.tag, "Top");
            }
            if (scrLauncher.scrDamage.shieldTopObj != null)
                scrLauncher.HitShield(scrLauncher.scrDamage.shieldTopObj);

            rb.AddForce(dir * Knockback / 2.0f, ForceMode.Impulse);
        }

        scrLauncher.SpawnExplosion(transform.position);
        Destroy(gameObject);
    }
}
