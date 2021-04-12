using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour{

	//Menus
	public GameObject playerMenuUI;
	public GameObject gameHUD;
	public GameObject pauseMenuUI;
	public bool GameisPaused = false;
	public GameObject fightButton;
	public AudioMixer mixer;
    public static float volumeLevel = 1.0f;
    private Slider sliderVolumeCtrl;

	public bool isShowcase = false;
	public static string lastPlayScene;
	
	//Co-op game mode variables
	public bool isCoop = false;
	public bool coopMonstersWillSpawn = false;
	private static bool coopPlayer1Dead = false;
	private static bool coopPlayer2Dead = false;
	private static bool coopMonsterDead = false;
	public bool coopMonsterWins = false;	
	public bool coopPlayersWin = false;
	private static float monsterHealth;
	public GameObject monsterHealthText;
	public string coopDefeatMsg = "The Monster defeated the players.";
	public string coopWinMsg = "The Players win!";
	private bool sendP1death = true;
	private bool sendP2death = true;

	//Players
	public GameObject Player1Holder;
	public GameObject Player2Holder;
	public GameObject cam1Prefab;
	public GameObject cam2Prefab;
	public GameObject camStart;
	public GameObject camTopPrefab;
	public Transform camTopHolder;
	
	public static GameObject player1Prefab;
	public static GameObject player2Prefab;
	public static string p1PrefabNameLast;
	public static string p2PrefabNameLast;
	public string p1PrefabName; //the actual p1 prefab
	public string p2PrefabName; //the actual p2 prefab
	public string p1PlayerChoiceName;
	public string p2PlayerChoiceName;
	public bool isP1_NPC = false; 
	public bool isP2_NPC = false; 
	
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
	public GameObject inputFieldGameTime;
	public int gameTime = 60;
	public GameObject gameTimerText;
	private float gameTimer = 0f;
	private bool isGameTime = false;

	//Fall Does Damage
	public bool fallDoesdamage = false;
	public int fallDamage = 4;

	//Variables for Bot arrays
	public string[] botNames;	
	public GameObject[] botPrefabs;
	Dictionary<string, GameObject> botDictionary;

	//Variables for Bot NPC arrays
	public string[] botNamesNPC;	
	public GameObject[] botPrefabsNPC;
	Dictionary<string, GameObject> botDictionaryNPC;

	private Scene thisScene;
	public static bool notFirstGame = false;

	public static UnityAction onBattleStart;
	
	void Awake (){
		SetLevel (volumeLevel);
		GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
		if (sliderTemp != null){
			sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
			sliderVolumeCtrl.value = volumeLevel;
		}
    }
	
	void Start(){
		//initialize the Bot dictionary and populate with arrays content
		botDictionary = new Dictionary<string, GameObject>();
		for (int i=0; i < botPrefabs.Length; i++) {
			botDictionary.Add(botNames[i], botPrefabs[i]);
		}
		
		//initialize the NPC dictionary and populate with arrays content
		botDictionaryNPC = new Dictionary<string, GameObject>();
		for (int i=0; i < botPrefabsNPC.Length; i++) {
			botDictionaryNPC.Add(botNamesNPC[i], botPrefabsNPC[i]);
		}

		//rematch display
		if ((notFirstGame==true)&&(gameObject.GetComponent<GameHandler_LastBots>()!= null)){
			gameObject.GetComponent<GameHandler_LastBots>().UpdateLastBots(player1Prefab, player2Prefab);
		}

		// check for endscene PvP
		thisScene = SceneManager.GetActiveScene();
		if (thisScene.name == "EndScene"){
			if (player1Prefab != null){
				Instantiate(player1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
				Instantiate(player2Prefab, Player2Holder.transform.position, Player2Holder.transform.rotation, Player2Holder.transform);
				UpdateStats();
				
				GameObject celebratePlayer1 = GameObject.FindWithTag("p1celeb");
				GameObject celebratePlayer2 = GameObject.FindWithTag("p2celeb");
				if(winner.Split(' ')[0] == "Player1:"){
					celebratePlayer1.SetActive(true);
					celebratePlayer2.SetActive(false);
				} else if (winner.Split(' ')[0] == "Player2:"){
					celebratePlayer1.SetActive(false);
					celebratePlayer2.SetActive(true);
				} else {
					celebratePlayer1.SetActive(false);
					celebratePlayer2.SetActive(false);
				}
				
			} else {Debug.Log("This Scene depends on static variables from an Arena Scene");}
		}


		// check for endscene Co-op
		thisScene = SceneManager.GetActiveScene();
		if (thisScene.name == "EndSceneCoop"){
			if (player1Prefab != null){
				Instantiate(player1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
				Instantiate(player2Prefab, Player2Holder.transform.position, Player2Holder.transform.rotation, Player2Holder.transform);
				UpdateStats();
				
				GameObject celebratePlayer1 = GameObject.FindWithTag("p1celeb");
				GameObject celebratePlayer2 = GameObject.FindWithTag("p2celeb");
				if(coopMonsterDead == true){
					celebratePlayer1.SetActive(true);
					celebratePlayer2.SetActive(true);
				} else if ((coopPlayer1Dead == true)&&(coopPlayer2Dead == true)){
					celebratePlayer1.SetActive(false);
					celebratePlayer2.SetActive(false);
				}
				
			} else {Debug.Log("This Scene depends on static variables from a Co-op Scene");}
		}

		// check for Showcase
		if (isShowcase == true){
		if (player1Prefab != null){
				Instantiate(player1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
				Instantiate(cam1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
				camStart.SetActive(false);
			} else {Debug.Log("This Scene depends on static variables from MainMenu Scene");}
		}
		
		//if this scene is a play scene, load into variable for replay:
		if ((thisScene.name != "Mainmenu")&&(thisScene.name != "EndScene")&&(thisScene.name != "EndSceneCoop")){
			lastPlayScene = thisScene.name;
		}
		
		//initial menu displays
		playerMenuUI.SetActive(true);
		gameHUD.SetActive(false);
		pauseMenuUI.SetActive(false);
		camStart.SetActive(true);
		fightButton.SetActive(false);
		
		//initial player and game stats
		p1Health= playersHealthStart;
		p2Health= playersHealthStart;
		p1Shields = 6;
		p2Shields = 6;
	}

	void Update(){
		if ((p1Health <= 0)&&(thisScene.name != "EndScene")&&(thisScene.name != "EndSceneCoop")){
			p1Health = 0;
			if (isCoop == false){	
				if (p2PlayerChoiceName != ""){
					winner = "Player2: " + p2PlayerChoiceName;
				} else {winner = "Player2: " + p2PrefabNameLast;}
				StartCoroutine(EndGame());
			} else if ((isCoop == true)&&(sendP1death == true)){
				coopPlayer1Dead=true;
				sendP1death = false;
				//Debug.Log("player 1 is deeeeeeaaaad...."); 
				StartCoroutine(CoopEndGame());
			}
		}
		if ((p2Health <= 0)&&(thisScene.name != "EndScene")&&(thisScene.name != "EndSceneCoop")){
			p2Health = 0;
			if (isCoop == false){
				if (p1PlayerChoiceName != ""){
					winner = "Player1: " + p1PlayerChoiceName;
				} else {winner = "Player1: " + p1PrefabNameLast;}
				StartCoroutine(EndGame());
			} else if ((isCoop == true)&&(sendP2death == true)){
				coopPlayer2Dead=true;
				sendP2death = false;
				//Debug.Log("player 2 is deeeeeeaaaad....");
				StartCoroutine(CoopEndGame());
			}
		}

		//Pause Menu 1/2
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (GameisPaused){ Resume(); }
			else{ Pause(); }
		}
		
		if ((p1PrefabName == "")||(p2PrefabName == "")){fightButton.SetActive(false);}
		else {fightButton.SetActive(true);}
		
	}

	void FixedUpdate(){
		if (isGameTime==true){
			gameTimer += 0.01f;
			if (gameTime <= 0){
				gameTime = 0;
				if (isCoop == false){
					winner = "Time's up! \nNo winner. \nP1 Health = " + p1Health + " \nP2 Health = " + p2Health;
					StartCoroutine(EndGame());
				}
			}
			else if ((gameTimer >= 1f)&&(thisScene.name != "EndScene")){
				gameTime -= 1;
				UpdateStats();
				gameTimer = 0;
			}
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
		if (isShowcase == false){
			if (player1Prefab != null){	
				if (p1PlayerChoiceName != ""){
					p1Ntemp.text = "" + p1PlayerChoiceName;
					p1PrefabNameLast = p1PlayerChoiceName;
				}
				else {p1Ntemp.text = "" + p1PrefabNameLast;}
			}
				else { p1Ntemp.text = ""; }
		}
		else {
			if (player1Prefab != null){	p1Ntemp.text = "" + player1Prefab.name;}
			else { p1Ntemp.text = ""; }
		}

		Text p2Htemp = p2HealthText.GetComponent<Text>();
		p2Htemp.text = "P2 Health: " + p2Health;

		Text p2Stemp = p2ShieldsText.GetComponent<Text>();
		p2Stemp.text = "P2 Shields: " + p2Shields;
		
		Text p2Ntemp = p2NameText.GetComponent<Text>();
		if (isShowcase == false){
			if (player2Prefab != null){	
				if (p2PlayerChoiceName != ""){
					p2Ntemp.text = "" + p2PlayerChoiceName;
					p2PrefabNameLast = p2PlayerChoiceName;
				}
				else {p2Ntemp.text = "" + p2PrefabNameLast;}
				
			}
				else { p2Ntemp.text = ""; }
		}
		else {
			if (player2Prefab != null){	p2Ntemp.text = "" + player2Prefab.name;}
			else { p2Ntemp.text = ""; }
		}
		
		Text GTtemp = gameTimerText.GetComponent<Text>();
		GTtemp.text = "" + gameTime;
		
		Text winTemp = winnerText.GetComponent<Text>();
		if (isCoop == false){winTemp.text = "WINNER: \n" + winner;}
		else if (isCoop == true){winTemp.text = "" + winner;}		
	}
	
	public IEnumerator EndGame(){
		notFirstGame = true;
		yield return new WaitForSeconds(0.5f);
		//Debug.Log("Game Over! \n Winner = " + winner);		
		if ((thisScene.name != "EndScene")&&(isShowcase == false)&&(thisScene.name != "MainMenu")){
			SceneManager.LoadScene ("EndScene");
		}
	}
	
	//Co-op game mode end-game functions
	public void CoopUpdateMonster(float monsterHealth){
		Text monHealthTemp = monsterHealthText.GetComponent<Text>();
		monHealthTemp.text = "Monster Health: " + monsterHealth;
		
		if (monsterHealth <= 0){
			coopMonsterDead = true;
			StartCoroutine(CoopEndGame());
		}
	}
	
	public IEnumerator CoopEndGame(){
		notFirstGame = true;
		if ((coopPlayer1Dead == true)&&(coopPlayer2Dead == true)){
			winner = coopDefeatMsg;
			yield return new WaitForSeconds(1.0f);		
			if ((thisScene.name != "EndScene")&&(isShowcase == false)&&(thisScene.name != "MainMenu")&&(thisScene.name != "EndSceneCoop")){
				SceneManager.LoadScene ("EndSceneCoop");
			}
		}
		if (coopMonsterDead == true){
			winner = coopWinMsg;
			yield return new WaitForSeconds(1.0f);
			if ((thisScene.name != "EndScene")&&(isShowcase == false)&&(thisScene.name != "MainMenu")&&(thisScene.name != "EndSceneCoop")){
				SceneManager.LoadScene ("EndSceneCoop");
			}
		}
		
		if (coopMonsterWins == true){
			winner = coopDefeatMsg;
			yield return new WaitForSeconds(1.0f);		
			if ((thisScene.name != "EndScene")&&(isShowcase == false)&&(thisScene.name != "MainMenu")&&(thisScene.name != "EndSceneCoop")){
				SceneManager.LoadScene ("EndSceneCoop");
			}
		}
		if (coopPlayersWin == true){
			winner = coopWinMsg;
			yield return new WaitForSeconds(1.0f);
			if ((thisScene.name != "EndScene")&&(isShowcase == false)&&(thisScene.name != "MainMenu")&&(thisScene.name != "EndSceneCoop")){
				SceneManager.LoadScene ("EndSceneCoop");
			}
		}
		
		
	}
	
	//Pause Menu 2/2
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
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);	
	}
	
	public void Replay(){
		Time.timeScale = 1f;
		//restart the game:
		p1Health = 20f;
		p2Health = 20f;
		p1Shields = 6;
		p2Shields = 6;
		if (notFirstGame==true){ 
			SceneManager.LoadScene(lastPlayScene);
		} 
		else { 
			SceneManager.LoadScene("MainMenu");
		}
	}
	
	//MainMenu buttons
	public void MainMenu(){
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
		}
		
	public void Quit(){
		#if UNITY_EDITOR 
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	public void StartGame(){
		Time.timeScale = 1f;
		//restart the game:
		p1Health = 20f;
		p2Health = 20f;
		p1Shields = 6;
		p2Shields = 6;
		SceneManager.LoadScene("Arena1");
	}
	
	public void StartGameCoop(){
		Time.timeScale = 1f;
		//restart the game:
		p1Health = 20f;
		p2Health = 20f;
		p1Shields = 6;
		p2Shields = 6;
		SceneManager.LoadScene("Coop1");
	}
	
	//hit FIGHT button in player choice menu to start the battle
	public void StartBattle(){
		string gameTimeString = inputFieldGameTime.GetComponentInChildren<Text>().text;
		int.TryParse(gameTimeString, out gameTime);
		if (gameTime == 0){gameTime = 60;}
		
		UpdateStats();
		playerMenuUI.SetActive(false);
		gameHUD.SetActive(true);
		pauseMenuUI.SetActive(false);
		
		if (onBattleStart != null)
			onBattleStart.Invoke();
			
		if (p1PrefabName != ""){	
			if (isP1_NPC == false){	player1Prefab = botDictionary[p1PrefabName];}
			else if (isP1_NPC == true){	player1Prefab = botDictionaryNPC[p1PrefabName];}
			
			if (isP2_NPC == false){	player2Prefab = botDictionary[p2PrefabName];}
			else if (isP2_NPC == true){	player2Prefab = botDictionaryNPC[p2PrefabName];}
		}
		
		//Instantiate players and cameras, and turn off StartCamera:
		GameObject Player1 = Instantiate(player1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
		GameObject Player2 = Instantiate(player2Prefab, Player2Holder.transform.position, Player2Holder.transform.rotation, Player2Holder.transform);
		Instantiate(cam1Prefab, Player1Holder.transform.position, Player1Holder.transform.rotation, Player1Holder.transform);
		Instantiate(cam2Prefab, Player2Holder.transform.position, Player2Holder.transform.rotation, Player2Holder.transform);
		camStart.SetActive(false);
		
		GameObject camTop = Instantiate(camTopPrefab, camTopHolder.position, camTopHolder.rotation);
		camTop.GetComponent<CameraTopDown>().loadPlayers(Player1, Player2);
		
		isGameTime = true;
		
		if ((isCoop==true)&&(coopMonstersWillSpawn == false)){
			GameObject.FindWithTag("CoopNPCMonster").GetComponent<NPC_LoadPlayers>().LoadPlayerTargets();
		}
		
		if (fallDoesdamage == true){
			Player1.GetComponent<BotBasic_FallRespawn>().doesFallDamage = true;
			Player1.GetComponent<BotBasic_FallRespawn>().playerFallDamage = fallDamage;
			Player2.GetComponent<BotBasic_FallRespawn>().doesFallDamage = true;
			Player2.GetComponent<BotBasic_FallRespawn>().playerFallDamage = fallDamage;
		}
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