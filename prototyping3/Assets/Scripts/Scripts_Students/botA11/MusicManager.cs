using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource source;

    private GameHandler handler;
    private Scene currScene;

    private static bool playedOnce = false;
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        
        if (playedOnce == false)
            source.Play();

        playedOnce = true;
        
        handler = FindObjectOfType<GameHandler>();

        currScene = SceneManager.GetActiveScene();

        GameHandler.onBattleStart += StopMusicAndDestroy;
        
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (handler == null)
            handler = FindObjectOfType<GameHandler>();
        
        if (source.isPlaying == false || handler.isShowcase)
            Destroy(gameObject);
    }

    private void StopMusicAndDestroy()
    {
        source.Stop();
        Destroy(gameObject);
    }
    
}
