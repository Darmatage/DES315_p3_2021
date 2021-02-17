﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameHandler : MonoBehaviour{

	//Menus
	public GameObject playerMenuUI;
	public GameObject gameHUD;
	public GameObject pauseMenuUI;
	public static bool GameisPaused = false;
	public GameObject fightButton;
	public AudioMixer mixer;
    public static float volumeLevel = 1.0f;
    private Slider sliderVolumeCtrl;

	public bool isShowcase = false;

	//Players
	public GameObject Player1Holder;
	public GameObject Player2Holder;
	public GameObject cam1Prefab;
	public GameObject cam2Prefab;
	public GameObject camStart;
	
	public static GameObject player1Prefab;
	public static GameObject player2Prefab;
	public string p1PrefabName;
	public string p2PrefabName;
	
	//Stats
	public float playersHealthStart = 20f;
	public static float p1Health;
	public static float p2Health;
	public float p1Shields;
	public float p2Shields;
	public static string winner;
	
	//Text Objects to display stats
	public GameObject p1HealthText;
	public GameObject p1ShieldsText;
	public GameObject p1NameText;	
	public GameObject p2HealthText;
	public GameObject p2ShieldsText;
	public GameObject p2NameText;
	public GameObject winnerText;
	
	//Timer
	public GameObject gameTimerText;
	private float gameTimer = 0f;
	public int gameTime;
	
	//Variables for Bot arrays
	public string[] botNames;	
	public GameObject[] botPrefabs;
	Dictionary<string, GameObject> botDictionary;
	
	Scene thisScene;
	
	void Awake (){
		SetLevel (volumeLevel);
		GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
		if (sliderTemp != null){
			sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
			sliderVolumeCtrl.value = volumeLevel;
		}
    }
	
	void Start(){
		//initialize the dictionary and populate with arrays content
		botDictionary = new Dictionary<string, GameObject>();
		for (int i=0; i < botPrefabs.Length; i++) {
			botDictionary.Add(botNames[i], botPrefabs[i]);
		}

		// check for endscene
		thisScene = SceneManager.GetActiveScene();
		if (thisScene.name == "EndScene"){
			if (player1Prefab != null){
				Instantiate(player1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
				Instantiate(player2Prefab, Player2Holder.transform.position, Player2Holder.transform.rotation, Player2Holder.transform);
				UpdateStats();
			} else {Debug.Log("This Scene depends on static variables from an Arena Scene");}
		}

		// check for Showcase
		if (isShowcase == true){
		if (player1Prefab != null){
				Instantiate(player1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
				Instantiate(cam1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
				camStart.SetActive(false);
			} else {Debug.Log("This Scene depends on static variables from MainMenu Scene");}
		}
		
		//initial menu displays
		playerMenuUI.SetActive(true);
		gameHUD.SetActive(false);
		pauseMenuUI.SetActive(false);
		camStart.SetActive(true);
		fightButton.SetActive(false);
		
		//initial player and game stats
		gameTime = 60;
		p1Health= playersHealthStart;
		p2Health= playersHealthStart;
		p1Shields = 6;
		p2Shields = 6;
	}

	void Update(){
		if (p1Health <= 0){
			p1Health = 0;
			winner = "Player2";
			StartCoroutine(EndGame());
		}
		if (p2Health <= 0){
			p2Health = 0;
			winner = "Player1";
			StartCoroutine(EndGame());
		}

		//Pause Menu
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (GameisPaused){ Resume(); }
			else{ Pause(); }
		}
		
		if ((p1PrefabName == "")||(p2PrefabName == "")){fightButton.SetActive(false);}
		else {fightButton.SetActive(true);}
		
	}

	void FixedUpdate(){
		gameTimer += 0.01f;
		if (gameTime <= 0){
			gameTime = 0;
			winner = "Time's up! \nNo winner. \nP1 Health = " + p1Health + " \nP2 Health = " + p2Health;
			StartCoroutine(EndGame());
		}
		else if (gameTimer >= 1f){
			gameTime -= 1;
			UpdateStats();
			gameTimer = 0;
		}
	}


	public void TakeDamage(string player, float damage){
		if (player == "Player1"){
			p1Health -= damage; 
			if (p1Health <= 0){
				p1Health = 0;
			}
		}
		else if (player == "Player2"){
			p2Health -= damage; 
			if (p2Health <= 0){
				p2Health = 0;
			}
		}
		UpdateStats();
	}
	
	public void PlayerShields(string player, string lostShield){
		if (player == "Player1"){
			p1Shields -= 1;
		}
		else if (player == "Player2"){
			p2Shields -= 1;
		}
		Debug.Log("" + player + " lost the " + lostShield + " shield");
		UpdateStats();
	}
	
	public void UpdateStats(){
		Text p1Htemp = p1HealthText.GetComponent<Text>();
		p1Htemp.text = "P1 Health: " + p1Health;
		
		Text p1Stemp = p1ShieldsText.GetComponent<Text>();
		p1Stemp.text = "P1 Shields: " + p1Shields;

		Text p1Ntemp = p1NameText.GetComponent<Text>();
		if (player1Prefab != null){	p1Ntemp.text = "" + player1Prefab.name;}
		else { p1Ntemp.text = ""; }

		Text p2Htemp = p2HealthText.GetComponent<Text>();
		p2Htemp.text = "P2 Health: " + p2Health;

		Text p2Stemp = p2ShieldsText.GetComponent<Text>();
		p2Stemp.text = "P2 Shields: " + p2Shields;
		
		Text p2Ntemp = p2NameText.GetComponent<Text>();
		if (player2Prefab != null){	p2Ntemp.text = "" + player2Prefab.name;}
		else { p2Ntemp.text = ""; }
		
		Text GTtemp = gameTimerText.GetComponent<Text>();
		GTtemp.text = "" + gameTime;
		
		Text winTemp = winnerText.GetComponent<Text>();
		winTemp.text = "WINNER: \n" + winner;
	}
	
	IEnumerator EndGame(){
		yield return new WaitForSeconds(0.5f);
		//Debug.Log("Game Over! \n Winner = " + winner);
		
		if ((thisScene.name != "EndScene")&&(isShowcase == false)&&(thisScene.name != "MainMenu")){
			SceneManager.LoadScene ("EndScene");
		}
	}
	
	//Pause Menu
	void Pause(){
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameisPaused = true;
	}
	public void Resume(){
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameisPaused = false;
	}
	public void Restart(){
		Time.timeScale = 1f;
		//restart the game:
		p1Health = 20f;
		p2Health = 20f;
		p1Shields = 6;
		p2Shields = 6;
		SceneManager.LoadScene("Arena1");
	}
	
	//MainMenu buttons
	public void MainMenu(){SceneManager.LoadScene("MainMenu");}
	public void Quit(){
		#if UNITY_EDITOR 
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	public void StartGame(){
		SceneManager.LoadScene("Arena1");
	}
	
	//hit FIGHT button in player choice menu to start the battle
	public void StartBattle(){
		UpdateStats();
		playerMenuUI.SetActive(false);
		gameHUD.SetActive(true);
		pauseMenuUI.SetActive(false);
			
		player1Prefab = botDictionary[p1PrefabName];
		player2Prefab = botDictionary[p2PrefabName];
		
		//Instantiate players and cameras, and turn off StartCamera:
		Instantiate(player1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
		Instantiate(player2Prefab, Player2Holder.transform.position, Player2Holder.transform.rotation, Player2Holder.transform);
		Instantiate(cam1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
		Instantiate(cam2Prefab, Player2Holder.transform.position, Player2Holder.transform.rotation, Player2Holder.transform);
		camStart.SetActive(false);
	}
	
	public void SetLevel (float sliderValue){
		mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
		volumeLevel = sliderValue;
    } 
	
}



//transform string to a reference to a gameobject
//dictionary
//array of structs
//getvalue
//dictionary
//	 List<string> listWith$$anonymous$$eys = new List<string>();

		// #if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false;
        // #elif UNITY_WEBPLAYER
        // Application.OpenURL(webplayerQuitURL);
        // #else
        // Application.Quit();
        // #endif