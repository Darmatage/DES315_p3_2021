using UnityEngine;

public class botB01_LavaHazard : MonoBehaviour
{
    public GameHandler gh;
    public Transform TopPlatform;
    
    [Range(0, 1)] public float GrowthSpeed;
    [Range(0, 2)] public float KillDelay;
    [Range(0, 1)] public float LavaFlowSpeed;
    [Range(0, 10)] public float LavaColorSpeed;
    [Range(0, 5)] public float StartDelay;
    
    private float goalPos = 0;
    private bool Initialized = false;

    private bool playerKilled = false;
    private string loserTag = "";

    private Transform p1, p2;

    private Material mat;
    
    private void OnEnable()
    {
        GameHandler.onBattleStart += Initialize;
    }
    private void OnDisable()
    {
        GameHandler.onBattleStart -= Initialize;
    }

    private void Initialize()
    {
        Invoke(nameof(Startup), StartDelay);
    }

    private void Startup()
    {
        Initialized = true;
        mat = GetComponent<MeshRenderer>().material;
        mat.color = Color.yellow;
        p1 = gh.Player1Holder.transform.GetChild(0);
        p2 = gh.Player2Holder.transform.GetChild(0);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!Initialized)
            return;

        goalPos = Mathf.Max(p1.position.y - 5, p2.position.y - 5, transform.position.y);
        int mult = Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(Mathf.Abs(goalPos - transform.position.y), 4)), 1, 6);
        if (goalPos > transform.position.y + 1)
            transform.position += Vector3.up * (GrowthSpeed * Time.deltaTime * mult);
        
        float texInterpolant = Mathf.Pow(Mathf.PingPong(LavaFlowSpeed * Time.time, 0.5f) + 0.25f, 2);
        mat.SetTextureOffset("_MainTex", new Vector2(texInterpolant, 1 - texInterpolant));

        mat.color = new[] {Color.black, Color.yellow, Color.red, new Color(.75f, 0, .75f), Color.blue, Color.cyan, Color.white}[mult];
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerKilled && other.transform.root.tag.Contains("Player"))
        {
            playerKilled = true;
            loserTag = other.transform.root.tag;
            Invoke(nameof(KillPlayer), KillDelay);
        }
    }

    private void KillPlayer()
    {
        gh.TakeDamage(loserTag, 10000);
    }
}
