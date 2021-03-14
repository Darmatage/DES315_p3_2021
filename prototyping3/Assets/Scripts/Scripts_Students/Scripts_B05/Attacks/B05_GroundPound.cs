using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_GroundPound : MonoBehaviour
{
    public Rigidbody rb = null;
    public Bot05_Move b05;
    public Transform center;

    public SpriteRenderer circle;

    private bool b_active;
    private bool b_offground;
    private bool b_downward;
    private bool b_cooling;
    private float impact_range = 5.25f;

    private float timer;
    private float t_cooldown = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        b_active = false;
        b_offground = false;
        b_downward = false;
        b_cooling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (b_cooling)
        {
            timer += Time.deltaTime;
            if (timer > t_cooldown)
            {
                b_cooling = false;
                rb.constraints = RigidbodyConstraints.None;
                b05.SetState(Bot05_Move.STATE.NORMAL);
                circle.enabled = false;
            }
        }

        if (!b05.isGrounded && b_active)
        {
            b_offground = true;
        }

        if (b_offground && b_active)
        {
            if (rb.velocity.y < 0.0f && !b_downward)
            {
                rb.AddForce(-transform.up * 800.0f, ForceMode.Acceleration);
                b_downward = true;
            }
        }

        if (b05.isGrounded && b_active && b_offground)
        {
            
            b_active = false;
            b_offground = false;
            b_downward = false;

            B05_MiniTop[] tops = FindObjectsOfType<B05_MiniTop>();
            for (int i = 0; i < tops.Length; ++i)
            {
                // is the top in range of the impact?
                if (Mathf.Abs(Vector3.Distance(tops[i].GetPosition(), center.position)) <= impact_range)
                {
                    tops[i].BlastAway(center.position);
                }
            }

            b_cooling = true;
            timer = 0.0f;
            circle.enabled = true;
        }
    }

    public void Activate()
    {
        b_active = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        b05.SetState(Bot05_Move.STATE.JUMPING);
    }
}
