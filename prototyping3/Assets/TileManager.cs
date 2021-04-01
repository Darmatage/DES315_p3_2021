using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Vector2Int GridWidth = new Vector2Int(2, 2);
    public GameObject TilePrefab = null;

    private List<GameObject> Grid;

    public float[] FallTimings;
    private int TimingIndex;

    // Start is called before the first frame update
    void Start()
    {
        Grid = new List<GameObject>();

        for (int row = 0; row <= GridWidth.y * 2; ++row)
        {
            for (int col = 0; col <= GridWidth.x * 2; ++col)
            {
                Grid.Add(Instantiate(TilePrefab));
                
                Vector3 Offset = new Vector3(0.0f, 0.0f, 0.0f);
                Offset.x += TilePrefab.transform.localScale.x * (col - GridWidth.x);
                Offset.z += TilePrefab.transform.localScale.z * (row - GridWidth.y);
                
                Grid[Grid.Count - 1].transform.position = transform.position + Offset;
            }
        }

        StartCoroutine(BeginFallRoutine());
    }

    IEnumerator BeginFallRoutine()
    {
        if (FallTimings.Length > TimingIndex)
        {
            yield return new WaitForSeconds(FallTimings[TimingIndex++]);
            int index = Random.Range(0, Grid.Count);
            Grid[index].GetComponent<KeeganTompkinsTileFall>().BeginFall();
            Grid.RemoveAt(index);
            StartCoroutine(BeginFallRoutine());
        }
    }

    public void FightStart()
    {
        BeginFallRoutine();
        FindObjectOfType<GameHandler>().StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
