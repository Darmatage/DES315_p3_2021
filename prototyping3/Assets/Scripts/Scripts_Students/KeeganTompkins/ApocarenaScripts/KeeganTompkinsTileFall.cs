using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeganTompkinsTileFall : MonoBehaviour
{
    

    public float YellowTime = 0.3f;
    public float RedTime = 0.5f;
    public float FallSpeed = 10.0f;
    public float AccelSpeed = 2.0f;

    public Material Yellow;
    public Material Red;
    public Material mSafe;

    private bool Fall = false;
    private float deltaccumulator = 0.0f;
    private float Gravity = 0.0f;

    private Vector3 PrevShakeOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private float Shake;

    public float ShakeLerp;
    public float ShakeYellow;
    public float ShakeRed;

    public float pitchOff;

    public Transform ShakeObj;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Fall)
        {
            deltaccumulator = Mathf.Clamp(deltaccumulator + Time.deltaTime * AccelSpeed, 0.0f, 1.0f);
            Gravity = Mathf.Lerp(0.0f, FallSpeed, deltaccumulator * deltaccumulator);
        }

        transform.position -= new Vector3(0.0f, Gravity, 0.0f) * Time.deltaTime;

        if (Shake != 0.0f)
        {
            Vector3 ShakeOff = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            ShakeOff = Vector3.Lerp(ShakeOff.normalized * Shake, PrevShakeOffset, ShakeLerp);
            ShakeObj.position -= PrevShakeOffset;
            PrevShakeOffset = ShakeOff;
            ShakeObj.position += ShakeOff;
        }
        else
        {
            ShakeObj.position -= PrevShakeOffset;
            PrevShakeOffset = Vector3.zero;
        }
    }

    public void BeginFall()
    {
        GetComponent<AudioSource>().pitch = 1.0f + Random.Range(-pitchOff, pitchOff);
        GetComponent<AudioSource>().PlayDelayed(Random.Range(0.0f, 0.1f));
        StartCoroutine(BeginFallRoutine());
    }

    IEnumerator BeginFallRoutine()
    {
        Shake = ShakeYellow;
        GetComponentInChildren<MeshRenderer>().material = Yellow;
        yield return new WaitForSeconds(YellowTime);


        Shake = ShakeRed;
        GetComponentInChildren<MeshRenderer>().material = Red;
        yield return new WaitForSeconds(RedTime);

        Shake = 0.0f;
        Fall = true;
    }

    public void Safe()
    {
        GetComponentInChildren<MeshRenderer>().material = mSafe;
    }
}
