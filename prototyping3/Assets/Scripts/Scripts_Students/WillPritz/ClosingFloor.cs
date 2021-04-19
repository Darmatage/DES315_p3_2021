using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingFloor : MonoBehaviour
{
    public Vector3 finalScale;
    public float startCollapseTimer = 40.0f;
    public float finishCollapseTimer = 10.0f;
    public GameHandler Handler;

    private bool collapseStarted = false;
    private Vector3 startScale;
    private float scaleLength;
    private float startedJourneyTime;
    private float collapseSpeed;
    // Start is called before the first frame update
    void Start()
    {
        scaleLength = Vector3.Distance(gameObject.transform.localScale, finalScale);
        startScale = transform.localScale;
        collapseSpeed = (startScale.x - finalScale.x) / (2 * (startCollapseTimer - finishCollapseTimer));

    }

    // Update is called once per frame
    void Update()
    {
        if (Handler.gameTime == startCollapseTimer && collapseStarted == false)
        {
            collapseStarted = true;
            startedJourneyTime = Time.time;
        }
        else if (collapseStarted && Handler.gameTime > finishCollapseTimer)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startedJourneyTime) * collapseSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / scaleLength;
            
            transform.localScale = Vector3.Lerp(startScale, finalScale, fractionOfJourney);

        }
    }
}
