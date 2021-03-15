using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{

    [SerializeField] private GameObject MinePrefab;
    
    public string button1;

    //[SerializeField] private float MineCooldownTime;
    //private float mineCooldownTimer = 0f;
    //private bool isCoolingDown = false;

    [SerializeField] private Transform SpawnPoint;

    private int maxMines = 3;
    private int currMines = 3;

    [SerializeField] private List<GameObject> MineList;
    
    private AudioSource AudioSrc;
    [SerializeField] private AudioClip MineGainAudio;
    [SerializeField] private AudioClip MineDropAudio;

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        AudioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isCoolingDown && (Input.GetButtonDown(button1)))
        if (currMines > 0 && (Input.GetButtonDown(button1)))
        {
            AudioSrc.PlayOneShot(MineDropAudio, 1f);
            GameObject mine = Instantiate(MinePrefab, SpawnPoint.position, Quaternion.identity);
            mine.GetComponent<MineBehavior>().ParentSpawner = this;
            DecrementMines();
            //mineCooldownTimer = 0f;
            //isCoolingDown = true;
        }

        //if (isCoolingDown)
        //{
        //    mineCooldownTimer += Time.deltaTime;
        //}
        //
        //if (mineCooldownTimer >= MineCooldownTime)
        //{
        //    isCoolingDown = false;
        //}
    }

    private void DecrementMines()
    {
        --currMines;
        MineList[currMines].SetActive(false);
    }

    public void IncrementMines()
    {
        AudioSrc.PlayOneShot(MineGainAudio, 1f);
        MineList[currMines].SetActive(true);
        ++currMines;
    }
}
