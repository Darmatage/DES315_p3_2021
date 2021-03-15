using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotA14_ChargeMeterScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float chargeTimer = GetComponentInParent<BotA14_AttackScript>().chargeTimeCounter;

        RectTransform t = gameObject.GetComponent<RectTransform>();
        Vector3 vec = t.localScale;
        vec.z = chargeTimer / 2.0f;
        t.localScale = vec;
    }
}
