using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject Menu, PauseMenu;
    public GameObject minimap;

    // Start is called before the first frame update
    void Start()
    {
        if(Menu.activeSelf == true)
        {
            minimap.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.activeSelf == false && PauseMenu.activeSelf == false)
        {
            minimap.gameObject.SetActive(true);
        }
        else
            minimap.gameObject.SetActive(false);
    }
}
