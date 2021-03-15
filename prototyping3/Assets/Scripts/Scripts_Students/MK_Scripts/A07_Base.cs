using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A07_Base : MonoBehaviour
{
    [Header("Management")] 
    [SerializeField] private BotBasic_Damage damageHandler;
    [SerializeField] private playerParent parentObj;
    [Header("Shields")] 
    [SerializeField] private GameObject shieldFront;
    [SerializeField] private GameObject shieldBack;
    [SerializeField] private GameObject shieldLeft;
    [SerializeField] private GameObject shieldRight;

    [Header("Attacks")] 
    [SerializeField] private float shieldSlamDuration = 1.0f;
    [SerializeField] private float shieldSlamDistance = 5.0f;
    
    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script

    private bool _shieldsOut;

    void Start(){
        button1 = parentObj.action1Input;
        button2 = parentObj.action2Input;
        button3 = parentObj.action3Input;
        button4 = parentObj.action4Input;
    }
    private void Update()
    {
        // Make sure the shields are visible, since it actually matters in this case.
        if (damageHandler.shieldPowerFront > 0f && !shieldFront.activeSelf)
            shieldFront.SetActive(true);
        if (damageHandler.shieldPowerBack > 0f && !shieldBack.activeSelf)
            shieldBack.SetActive(true);
        if (damageHandler.shieldPowerLeft > 0f && !shieldLeft.activeSelf)
            shieldLeft.SetActive(true);
        if (damageHandler.shieldPowerRight > 0f && !shieldRight.activeSelf)
            shieldRight.SetActive(true);
        
        if ((Input.GetButtonDown(button1))&&(_shieldsOut==false)){
            _shieldsOut = true;
            StartCoroutine(ExtendShields());
        }
    }
    IEnumerator ExtendShields()
    {
        float elapsed = 0f;
        if (damageHandler.shieldPowerFront > 0f && !shieldFront.activeSelf)
            shieldFront.SetActive(true);
        if (damageHandler.shieldPowerBack > 0f && !shieldBack.activeSelf)
            shieldBack.SetActive(true);
        if (damageHandler.shieldPowerLeft > 0f && !shieldLeft.activeSelf)
            shieldLeft.SetActive(true);
        if (damageHandler.shieldPowerRight > 0f && !shieldRight.activeSelf)
            shieldRight.SetActive(true);

        while (elapsed < shieldSlamDuration)
        {
            elapsed += Time.deltaTime;
            
            
            
            yield return null;
        }
        
        _shieldsOut = false;
    }
}
