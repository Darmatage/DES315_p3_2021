using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_MiniTop : MonoBehaviour
{
    // todo - blade spin speed relates to minitop speed?
    private GameObject parent_bot;

    private float t_cooldown = 0.2f;
    private float timer;
    private bool b_justhit;

    private float magSpeed = 7.0f;
    private float blastSpeed = 25.0f;

    public Material eye1;
    public Material eye2;
    public MeshRenderer ball;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        b_justhit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (b_justhit)
        {
            timer += Time.deltaTime;
            if (timer > t_cooldown)
            {
                b_justhit = false;
            }
        }
    }

    public void SetParent(GameObject go)
    {
        parent_bot = go;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // add force again if parent hits them while they are in blade rush
        if (collision.gameObject == parent_bot || collision.gameObject.gameObject == parent_bot || collision.gameObject.gameObject.gameObject == parent_bot)
        {
            if (parent_bot.GetComponent<Bot05_Move>().IsState(Bot05_Move.STATE.ATTACKING) && !b_justhit)
            {
                b_justhit = true;
                Vector3 parent_pos = parent_bot.GetComponent<Bot05_Move>().GetCenter().position;
                Vector3 pos = transform.position;
                Vector3 dir = pos - parent_pos;
                dir.Normalize();
                dir.y = 0.0f;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
                rb.AddForce(dir * 35.0f, ForceMode.Impulse);
            }
        }
    }

    public void MoveToward(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        dir.Normalize();
        transform.localPosition += dir * magSpeed * Time.deltaTime;
    }

    public void MoveAway(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        dir.Normalize();
        transform.localPosition -= dir * magSpeed * Time.deltaTime;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void BlastAway(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        dir.Normalize();
        gameObject.GetComponent<Rigidbody>().AddForce(-dir * blastSpeed, ForceMode.Impulse);
    }
}
