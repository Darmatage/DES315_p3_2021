using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{

    [SerializeField] private GameObject MinePrefab;
    
    public string button1;

    [SerializeField] private float MineCooldownTime;
    private float mineCooldownTimer = 0f;
    private bool isCoolingDown = false;
    
    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;

    }

    // Update is called once per frame
    void Update()
    {
        // Drop a mine
        //if (Input.GetKeyDown(KeyCode.Keypad1))
        //{
        //    Instantiate(MinePrefab);
        //}
     
        if (!isCoolingDown && (Input.GetButtonDown(button1)))
        {
            Instantiate(MinePrefab, transform.position, Quaternion.identity);
            mineCooldownTimer = 0f;
            isCoolingDown = true;
        }

        if (isCoolingDown)
        {
            mineCooldownTimer += Time.deltaTime;
        }
        
        if (mineCooldownTimer >= MineCooldownTime)
        {
            isCoolingDown = false;
        }
    }
}
