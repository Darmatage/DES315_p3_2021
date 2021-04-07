using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZoneScript : MonoBehaviour
{
    public float timer = 10.0f;
    public float growthRate = 0.4f;
    public float growthTime = 5.0f;
    public float timerTracker = 0;
    public float growthTimeTracker = 0;

    public bool hasTriggered = false;

    Vector3 obScale;
    Transform obTransform;

    // Start is called before the first frame update
    void Start()
    {
        obTransform = transform;
        obScale = obTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        obTransform.localScale = Vector3.Lerp(obTransform.localScale, obScale, 0.5f * Time.deltaTime);

        timerTracker += Time.deltaTime;

        if(timerTracker >= timer && hasTriggered == false)
        {
            hasTriggered = true;
            StartCoroutine(runGrowthTimer());

            timerTracker = 0;
        }
    }

    IEnumerator runGrowthTimer()
    {
        growthTimeTracker += Time.deltaTime;

        if (growthTimeTracker < growthTime)
        {
            //Object Growth 
            obTransform = gameObject.GetComponent<Transform>();
            obScale = obTransform.localScale;
            obScale.x += growthRate;
            obScale.y += growthRate;
            obScale.z += growthRate;
        }

        hasTriggered = false;
        yield return null;
    }
}
