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

    private bool Fall = false;
    private float deltaccumulator = 0.0f;
    private float Gravity = 0.0f;

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
    }

    public void BeginFall()
    {
        StartCoroutine(BeginFallRoutine());
    }

    IEnumerator BeginFallRoutine()
    {
        GetComponentInChildren<MeshRenderer>().material = Yellow;
        yield return new WaitForSeconds(YellowTime);


        GetComponentInChildren<MeshRenderer>().material = Red;
        yield return new WaitForSeconds(RedTime);

        Fall = true;
    }
}
