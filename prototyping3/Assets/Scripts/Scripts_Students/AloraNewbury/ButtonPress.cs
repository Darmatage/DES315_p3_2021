using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public GameObject button;
    public AudioSource click;
    public bool is_pressing;
    public bool gate_opened;
    // Start is called before the first frame update
    void Start()
    {
        gate_opened = false;
        is_pressing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (click.isPlaying == false && is_pressing == true)
        {
            gate_opened = true;
            Destroy(button);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.tag == "Player1" || other.gameObject.transform.parent.tag == "Player2")
        {
            click.Play();
            is_pressing = true;

        }


    }
}
