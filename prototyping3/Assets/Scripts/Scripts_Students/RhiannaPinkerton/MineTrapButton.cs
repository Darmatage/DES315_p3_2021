using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrapButton : MonoBehaviour
{
    public GameObject Spawner;
    public GameObject theButton;
    public bool isMoving = false;
    private float theButtonUpPos;

    public float MoveTime = 1.0f;

    private Vector3 targetPosition;
    
    [SerializeField] private GameObject MinePrefab;
    [SerializeField] private float SpawnTime = 0.5f;
    private float spawnTimer = 0f;

    public bool atStart = true;

    private bool SpawnMines = false;
    
    public Transform pathStart; 
    public Transform pathEnd;

    [SerializeField] private float damageAmount = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (theButton != null){
            theButtonUpPos = theButton.transform.localPosition.y;
        }
    }

    private void Update()
    {
        if (SpawnMines)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= SpawnTime)
            {
                spawnTimer = 0f;
                //GameObject mine = Instantiate(MinePrefab, Spawner.transform.localPosition, Quaternion.Euler(new Vector3(0f, 0f, 0f)), Spawner.transform.parent.transform);
                GameObject mine = Instantiate(MinePrefab, Spawner.transform.localPosition, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                mine.transform.SetParent(Spawner.transform.parent);
                mine.transform.localPosition = Spawner.transform.localPosition;
                mine.GetComponent<HazardDamage>().damage = damageAmount;
                mine.GetComponent<MineBehavior>().KnockBackStrength = 5f;
            }
        }
    }

    void FixedUpdate()
    {
        //move the spawner
        if (isMoving)
        {
            if (atStart == true){
                Vector3 targetPosition = pathEnd.localPosition;
                StartCoroutine(LerpPosition(targetPosition, MoveTime));
            }
            else if (atStart == false){
                Vector3 targetPosition = pathStart.localPosition;
                StartCoroutine(LerpPosition(targetPosition, MoveTime));
            }
        }
        //stop the spawner
        if (Spawner.transform.localPosition.y == pathEnd.transform.localPosition.y){
            ButtonUp();
            atStart = false;
            SpawnMines = false;
        }
        else if (Spawner.transform.localPosition.y == pathStart.transform.localPosition.y){
            ButtonUp();
            atStart = true;
            SpawnMines = false;
        }
    }
    
    public void OnTriggerEnter(Collider other){
        if ((other.transform.root.gameObject.tag=="Player1")||(other.transform.root.gameObject.tag=="Player2")){
            theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos - 0.4f, theButton.transform.localPosition.z);
            isMoving = true;
            Renderer buttonRend = theButton.GetComponent<Renderer>();
            buttonRend.material.color = new Color(2.0f, 0.5f, 0.5f, 2.5f); 
            
            SpawnMines = true;
        }
    }
    
    public void ButtonUp(){
        isMoving = false;
        theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos, theButton.transform.localPosition.z);
        Renderer buttonRend = theButton.GetComponent<Renderer>();
        buttonRend.material.color = Color.white;
    }
    
    IEnumerator LerpPosition(Vector3 targetPosition, float duration){
        float time = 0;
        Vector3 startPosition = Spawner.transform.localPosition;
        
        while (time < duration)
        {
            Spawner.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;

            yield return null;
            
        }
        Spawner.transform.localPosition = targetPosition;
    }

    private void MoveSpawner(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = Spawner.transform.localPosition;

        while (time < duration)
        {
            Spawner.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            

        }
        Spawner.transform.localPosition = targetPosition;
    }
}
