using System.Collections;
using UnityEngine;

public class botB01_RocketLauncher : MonoBehaviour, botB01_IAttack
{
    [Range(0, 5)] public float Cooldown;
    private float cooldownTimer = 0.0f;

    private botB01_Weapons scrWeapons;
    private int index = 3;
    private botB01_AttackState currentState;

    public GameObject RocketPrefab;
    private GameObject enemy;

    public GameObject ParticlesPrefab;

    public GameHandler scrHandler;
    public BotBasic_Damage scrDamage;
    public BotBasic_Damage scrDamageSelf;

    void Start()
    {
        scrWeapons = transform.parent.parent.GetComponent<botB01_Weapons>();
        enemy = GetEnemyBot();

        scrHandler = FindObjectOfType<GameHandler>();
        scrDamage = enemy.GetComponent<BotBasic_Damage>();
        scrDamageSelf = transform.root.GetComponentInChildren<BotBasic_Damage>();
    }

    void Update()
    {
        if (currentState == botB01_AttackState.cooldown) // Decay stage
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= Cooldown)
                Ready();
        }
    }

    public void Attack()
    {
        scrWeapons.SetButtonStatus(index, true);
        
        currentState = botB01_AttackState.cooldown;
        cooldownTimer = 0;
     
        GameObject rocket = Instantiate(RocketPrefab);
        rocket.transform.position = transform.position;
        var scr = rocket.GetComponent<botB01_RocketHoming>();
        scr.Target = enemy.transform;
        scr.scrLauncher = this;
    }

    public void Ready()
    {
        scrWeapons.SetButtonStatus(index, false);
        currentState = botB01_AttackState.inactive;
    }

    public void Cancel()
    {
        // No-op
    }

    private GameObject GetEnemyBot()
    {
        GameHandler gh = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        string playerTag = transform.root.tag;
        if (playerTag.Contains("1"))
            return gh.Player2Holder.transform.GetChild(0).gameObject;
        if (playerTag.Contains("2"))
            return gh.Player1Holder.transform.GetChild(0).gameObject;
        Debug.Log("Player is not tagged, rocket targeting shooter");
        return transform.parent.parent.gameObject;
    }

    public void SpawnExplosion(Vector3 pos)
    {
        GameObject damageParticles = Instantiate (ParticlesPrefab, pos, Quaternion.identity);
        StartCoroutine(destroyParticles(damageParticles));
    }
    
    IEnumerator destroyParticles(GameObject particles)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(particles);
    }

    public void HitShield(GameObject shield)
    {
        StartCoroutine(ShieldHitDisplay(scrDamage.shieldTopObj));
    }
    
    IEnumerator ShieldHitDisplay(GameObject shieldObj)
    {
        shieldObj.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        shieldObj.SetActive(false);
    }
}
