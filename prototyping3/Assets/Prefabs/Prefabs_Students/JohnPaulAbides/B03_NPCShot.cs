using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_NPCShot : MonoBehaviour
{
    public B03_NPC parent;
    public Transform origin;
    public Transform web;
    public float speed = 20.0f;

    private Rigidbody rigid;
    private bool hitFlag;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // web hit something, stop moving
        if (!hitFlag)
            rigid.MovePosition(transform.position + speed * transform.forward * Time.fixedDeltaTime);
    }

    void Update()
    {
        timer += Time.deltaTime;
        // update web appearance
        web.position = (origin.position + transform.position) / 2.0f;
        web.localScale = new Vector3(0.25f, 0.25f, Vector3.Distance(origin.position, transform.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        // only activate after shot leaves parent
        if (timer < 0.3f) return;

        if (other.gameObject.transform.root.name.StartsWith("B03_NPCMonster"))
        {
            parent.webShot = null;
            parent.webPullFlag = false;
            Destroy(gameObject);
        }
        else
        {
            // web hit something, stop moving
            parent.webPullFlag = true;
            hitFlag = true;
        }
    }
}
