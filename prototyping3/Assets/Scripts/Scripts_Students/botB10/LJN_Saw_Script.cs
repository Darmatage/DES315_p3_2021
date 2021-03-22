using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJN_Saw_Script : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public string myPlyr;

    public float hitTime = 0.1f;
    private float internalhit = 0;
    private bool toggl = true;
    private AudioSource source;
    void Start()
    {
        myPlyr = gameObject.transform.root.tag;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        internalhit -= Time.deltaTime;
        if(internalhit < 0)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = toggl;
            toggl = !toggl;
            internalhit = hitTime;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        transform.parent.parent.GetComponent<LJN_Weapon_Script>().SawCanDown = false;

        if(source != null)
        {
            source.Play();
        }
    }


}
