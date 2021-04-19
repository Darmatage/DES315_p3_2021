using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingWalls : MonoBehaviour
{
    public Vector3 finalPosition;
    public Vector3 finalScale;
    public float startCollapseTimer = 40.0f;
    public float finishCollapseTimer = 10.0f;
    public GameHandler Handler;

    private bool collapseStarted = false;
    private Vector3 startPosition;
    private Vector3 startScale;
    private float journeyLength;
    private float scaleLength;
    private float startedJourneyTime;
    private float collapseSpeed;
    // Start is called before the first frame update
    void Start()
    {
        finalPosition *= 1.5f;
        journeyLength = Vector3.Distance(gameObject.transform.position, finalPosition);
        scaleLength = Vector3.Distance(gameObject.transform.localScale, finalScale);
        startPosition = transform.position;
        startScale = transform.localScale;
        if (Mathf.Abs(startPosition.x) > Mathf.Abs(startPosition.z))
        {
            if (startPosition.x < 0)
            {
                collapseSpeed = (Mathf.Abs(startPosition.x) - Mathf.Abs(finalPosition.x)) / (2 * (startCollapseTimer - finishCollapseTimer));
            }
            else
            {

                collapseSpeed = (startPosition.x - finalPosition.x) / (2 * (startCollapseTimer - finishCollapseTimer));
            }
        }
        else
        {
            if (startPosition.z < 0)
            {
                collapseSpeed = (Mathf.Abs(startPosition.z) - Mathf.Abs(finalPosition.z)) / (2 * (startCollapseTimer - finishCollapseTimer));
            }
            else
            {

                collapseSpeed = (startPosition.z - finalPosition.z) / (2 * (startCollapseTimer - finishCollapseTimer));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Handler.gameTime == startCollapseTimer && collapseStarted == false)
        {
            collapseStarted = true;
            startedJourneyTime = Time.time;
        }
        else if (collapseStarted && Handler.gameTime > finishCollapseTimer)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startedJourneyTime) * collapseSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, finalPosition, fractionOfJourney);
            transform.localScale = Vector3.Lerp(startScale, finalScale, fractionOfJourney);
            
        }
    }
}
