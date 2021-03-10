using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSplincher_DamageBox : MonoBehaviour
{
    public Vector3 m_startPos;
    public Vector3 m_targetPos;
    float speed = 20.0f;
    bool done = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            Vector3 oldToParent = m_targetPos - transform.position;
            transform.position = transform.position + (m_targetPos - transform.position).normalized * speed * Time.deltaTime;
            Vector3 toParent = m_targetPos - transform.position;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
        {
            gameObject.layer = 17;
            other.gameObject.GetComponent<Rigidbody>().AddForce((m_targetPos - m_startPos).normalized * speed * 100);
            StartCoroutine(DestroySelf());
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }
}
