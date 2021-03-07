using UnityEngine;

public class botB01_PushBack : MonoBehaviour, botB01_IAttack
{
    public GameObject AuraCylinder;
    
    [Range(0, 5)] public float Cooldown;
    [Range(0, 5)] public float AttackTime;
    [Range(0, 5)] public float DecayTime;
    private float cooldownTimer = 0;
    private float attackTimer   = 0;
    private float decayTimer    = 0;

    [Range(0, 10)] public float GrowthExponent;
    [Range(1, 20)] public float MaxRadiusScale;
    private Vector3 startDimensions;
    private Vector3 maxDimensions;

    public Color Warning, Danger, Safe;

    private MeshRenderer mesh;
    private Collider col;
    
    private botB01_Weapons scrWeapons;
    private int index = 0;
    private botB01_AttackState currentState;

    void Start()
    {
        AuraCylinder = Instantiate(AuraCylinder);
        Invoke(nameof(SetHazardData), 0.01f);
        
        scrWeapons = transform.parent.parent.GetComponent<botB01_Weapons>();
        
        startDimensions = AuraCylinder.transform.localScale;
        maxDimensions = startDimensions * MaxRadiusScale;
        maxDimensions.y = startDimensions.y;
        
        mesh = AuraCylinder.GetComponent<MeshRenderer>();
        col  = AuraCylinder.GetComponent<Collider>();
        
        Ready();
    }
    
    private void SetHazardData()
    {
        var hazard = AuraCylinder.GetComponent<HazardDamage>();
        if (transform.root.CompareTag("Player1")) { hazard.isPlayer1Weapon = true; }
        if (transform.root.CompareTag("Player2")) { hazard.isPlayer2Weapon = true; }

        var launcher = AuraCylinder.GetComponent<botB01_PushBackLauncher>();
        launcher.source = transform;
        launcher.GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        AuraCylinder.transform.position = transform.position;
        if (currentState == botB01_AttackState.stage_0) // Growth stage
        {
            attackTimer += Time.deltaTime;
            if (attackTimer < AttackTime)
            {
                AuraCylinder.transform.localScale = Vector3.Lerp(startDimensions, 
                                                                 maxDimensions, 
                                                                 Mathf.Pow(attackTimer / AttackTime, GrowthExponent));

                Color c = mesh.material.color;
                c = Color.Lerp(Warning, Danger, Mathf.Pow(attackTimer / AttackTime, GrowthExponent));
                mesh.material.color = c;
            }
            else
            {
                currentState = botB01_AttackState.stage_1;
                mesh.material.color = Danger;
                col.enabled = false;
            }
        }
        else if (currentState == botB01_AttackState.stage_1) // Decay stage
        {
            decayTimer += Time.deltaTime;
            if (decayTimer < DecayTime)
            {
                Vector3 scale = AuraCylinder.transform.localScale;
                scale.x = Mathf.Lerp(maxDimensions.x, 0, Mathf.Pow(decayTimer / DecayTime, GrowthExponent));
                scale.z = scale.x;
                scale.y = Mathf.Lerp(maxDimensions.y, 0, Mathf.Pow(decayTimer / DecayTime, GrowthExponent));
                AuraCylinder.transform.localScale = scale;
                
                Color c = mesh.material.color;
                c = Color.Lerp(Danger, Safe, Mathf.Pow(decayTimer / DecayTime, 1));
                mesh.material.color = c;
            }
            else
            {
                currentState = botB01_AttackState.cooldown;
                mesh.enabled = false;
            }
        }
        else if (currentState == botB01_AttackState.cooldown) // Cooldown stage
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > Cooldown)
                Ready();
        }
    }

    public void Attack()
    {
        scrWeapons.SetButtonStatus(index, true);
        
        currentState = botB01_AttackState.stage_0;
        AuraCylinder.transform.localScale = startDimensions;
        
        cooldownTimer = 0;
        attackTimer   = 0;
        decayTimer    = 0;
        
        mesh.enabled = true;
        mesh.material.color = Safe;
        
        col.enabled = true;
    }

    public void Ready()
    {
        scrWeapons.SetButtonStatus(index, false);
        currentState = botB01_AttackState.inactive;
        mesh.enabled = false;
    }

    public void Cancel()
    {
        Ready();
        col.enabled = false;
    }
}
