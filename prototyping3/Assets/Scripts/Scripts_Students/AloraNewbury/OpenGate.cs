using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{

    public AudioSource open;

    public ButtonPress button;
    public bool is_open;

    // Start is called before the first frame update
    void Start()
    {
        is_open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_open == true && open.isPlaying == false)
        {
            Destroy(gameObject);

        }
        if (button.gate_opened == true && is_open == false)
        {
            open.Play();
            is_open = true;
        }
      
    }
}
