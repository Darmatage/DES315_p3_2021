using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileManager : MonoBehaviour
{
    public int SafeTiles = 1;

    public Vector2Int GridWidth = new Vector2Int(2, 2);
    public GameObject TilePrefab = null;
    public GameObject LaserPrefab = null;

    [System.Serializable]
    public class LaserTimings
    {
        [System.Serializable]
        public struct TimingPercent
        {
            public float percent;
            public int Number;
            public float Wait;
        }

        public List<TimingPercent> TimingPercents;

        [System.NonSerialized]
        public float maxTime;

        private int IndexFromTiming(float timing)
        {
            int i = -1;
            float Percent = ((maxTime - timing) / maxTime);
            foreach (var LT in TimingPercents)
            {
                ++i;
                Percent -= LT.percent; 
                if (Percent <= 0)
                {
                    break;
                }
            }
            return i;
        }

        public int GetNumberLasers(float timing)
        {
            return TimingPercents[IndexFromTiming(timing)].Number;
        }
        public float GetWait(float timing)
        {
            return TimingPercents[IndexFromTiming(timing)].Wait;
        }
    };

    public LaserTimings LaserTimes;

    private List<GameObject> Grid;

    public float[] FallTimings;
    private int TimingIndex;

    private float LevelTimingMax;
    private float LevelTiming;

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

        for (int i = 0; i < SafeTiles; ++i)
        {
            int index = Random.Range(0, Grid.Count);
            Grid[index].GetComponent<KeeganTompkinsTileFall>().Safe();
            Grid.RemoveAt(index);
        }
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

    public Vector3 RandomTilePos()
    {
        int index = Random.Range(0, Grid.Count);
        return Grid[index].transform.position;
    }

    IEnumerator BeginLasersRoutine()
    {
        yield return new WaitForSeconds(LaserTimes.GetWait(FindObjectOfType<GameHandler>().gameTime));

        for (int i = LaserTimes.GetNumberLasers(FindObjectOfType<GameHandler>().gameTime); i > 0; --i)
        {
            Vector3 pos = RandomTilePos();
            Instantiate(LaserPrefab, pos, Quaternion.identity);
        }
        StartCoroutine(BeginLasersRoutine());
    }

    public void FightStart()
    {
        LevelTimingMax = FindObjectOfType<GameHandler>().gameTime;
        Debug.Log("MaxTiming = " + LevelTimingMax);
        StartCoroutine(BeginFallRoutine());
        LaserTimes.maxTime = LevelTimingMax;
        StartCoroutine(BeginLasersRoutine());
        FindObjectOfType<GameHandler>().StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        //NavMeshBuilder.UpdateNavMeshData(NavMesh.GetNav)
    }
}
