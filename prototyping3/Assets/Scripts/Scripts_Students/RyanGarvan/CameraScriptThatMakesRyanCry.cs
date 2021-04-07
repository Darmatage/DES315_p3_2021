using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptThatMakesRyanCry : MonoBehaviour
{
    public GameObject camRenderBig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!camRenderBig.activeInHierarchy && camRenderBig.transform.parent.gameObject.activeInHierarchy)
        {
            camRenderBig.SetActive(true);
            Destroy(gameObject);
        }
    }
}
