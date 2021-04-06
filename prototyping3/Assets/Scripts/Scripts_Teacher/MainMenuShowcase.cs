using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuShowcase : MonoBehaviour{

	public string myScene;
	public GameObject botPrefab;
	public GameHandler gameHandlerObj;		
		
	public void Showcase(){
		//GameHandler.player1Prefab = botPrefab; 	//This command was for project #3
		SceneManager.LoadScene(myScene);
	}

}
