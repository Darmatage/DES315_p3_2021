using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingFloor : MonoBehaviour
{
    public Vector3 finalScale;
    private float startCollapseTimer = 0f;
    private float finishCollapseTimer = 0f;
    public GameHandler Handler;
    bool testedOnce = false;

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

    }

    // Update is called once per frame
    void Update()
    {
        if(Handler.isGameTime && !testedOnce)
        {
            startCollapseTimer = Handler.gameTime * 5.0f / 6.0f;
            finishCollapseTimer = Handler.gameTime / 5.0f;
            collapseSpeed = (startScale.x - finalScale.x) / (2 * (startCollapseTimer - finishCollapseTimer));
            testedOnce = true;
        }
        if (Handler.isGameTime && Handler.gameTime <= startCollapseTimer && collapseStarted == false)
        {
            collapseStarted = true;
            startedJourneyTime = Time.time;
        }
        else if (Handler.isGameTime && collapseStarted && Handler.gameTime > finishCollapseTimer)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startedJourneyTime) * collapseSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / scaleLength;
            
            transform.localScale = Vector3.Lerp(startScale, finalScale, fractionOfJourney);

        }
    }
}
