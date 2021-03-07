using UnityEngine;

public class botB01_DeployLaser : MonoBehaviour, botB01_IAttack
{
    [Range(0, 5)] public float Cooldown;
    private float cooldownTimer = 0.0f;

    private botB01_Weapons scrWeapons;
    private int index = 2;
    private botB01_AttackState currentState;
    
    // Start is called before the first frame update
    void Start()
    {
        scrWeapons = transform.parent.parent.GetComponent<botB01_Weapons>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        scrWeapons.SetButtonStatus(index, true);
    }

    public void Ready()
    {
        scrWeapons.SetButtonStatus(index, false);
    }

    public void Cancel()
    {
        
    }
}
