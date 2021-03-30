using UnityEngine;

public class botB01_LavaHazard : MonoBehaviour
{
    public GameHandler gh;
    public Transform TopPlatform;
    
    [Range(0.01f, 10)] public float GrowthExponent;
    [Range(0, 2)] public float KillDelay;
    [Range(0, 1)] public float LavaFlowSpeed;
    [Range(0, 10)] public float LavaColorSpeed;
    
    private Vector3 spawnPos;
    private Vector3 goalPos;
    private bool Initialized = false;
    private float gameTime;
    private float gameTimer;

    private bool playerKilled = false;
    private string loserTag = "";

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
        Initialized = true;
        spawnPos = transform.position;
        goalPos = TopPlatform.position;
        gameTimer = 0.0f;
        gameTime = gh.gameTime;
        mat = GetComponent<MeshRenderer>().material;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!Initialized)
            return;

        gameTimer += Time.deltaTime;
        transform.position = Vector3.Lerp(spawnPos, goalPos, Mathf.Pow(gameTimer / gameTime, GrowthExponent));
        
        float texInterpolant = Mathf.Pow(Mathf.PingPong(LavaFlowSpeed * Time.time, 0.5f) + 0.25f, 2);
        mat.SetTextureOffset("_MainTex", new Vector2(texInterpolant, 1 - texInterpolant));

        float colorInterpolant = Mathf.Pow(Mathf.PingPong(LavaColorSpeed * Time.time, 1.0f), 2);
        mat.color = Color.Lerp(Color.red, new Color(1, .35f, .20f), colorInterpolant);
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
