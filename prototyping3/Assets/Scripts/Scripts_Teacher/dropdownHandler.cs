using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dropdownHandler : MonoBehaviour{

	private GameHandler gameHandlerObj;
	private string playerChoice;
	private string playerChoiceName;
	private int pNum;
	public bool isPlayer1 = false;

	void Awake(){
		if (GameObject.FindWithTag("GameHandler") != null){
				gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
			}
			
		if (isPlayer1 == true){pNum = 1;}
		else {pNum = 2;}
	}

    void Start(){
        var dropdown = transform.GetComponent<Dropdown>();
		dropdown.options.Clear();
		
		List<string> items1 = new List<string>();
		items1.Add("");
		items1.Add("BotA00");
		items1.Add("BotA01 Oussama Khalaf");
		items1.Add("BotA02 Ryan Heath");
		items1.Add("BotA03 Jiwon Jung");
		items1.Add("BotA04 Akshat Madan");
		items1.Add("BotA05 Alora Newbury");
		items1.Add("BotA06 Ryan Garvan");
		items1.Add("BotA07 Matthew Klingman");
		items1.Add("BotA08 Lorenzo DeMaine");
		items1.Add("BotA09 Mark Culp");
		items1.Add("BotA10 David Dasky");
		items1.Add("BotA11 Amogh Subhedar");
		items1.Add("BotA12 Mars Jurich");
		items1.Add("BotA13 Devon Peterson");
		items1.Add("BotA14 Keanu Cendejas");
		items1.Add("BotA15 Amy Stoltz");
		items1.Add("BotA16 Keegan Tompkins");
		items1.Add("BotA17 Devin Cavness");
		items1.Add("BotA18 Aaron Johnson");
		items1.Add("BotA19 Zacary Brown");
		items1.Add("BotA20 Adam Doolittle");
		items1.Add("BotB01 Robert Seiver");
		items1.Add("BotB02 Will Pritz");
		items1.Add("BotB03 John Paul Abides");
		items1.Add("BotB04 Austin Stead");
		items1.Add("BotB05 Carlos Garcia-Perez");
		items1.Add("BotB06 Ethan Villasin");
		items1.Add("BotB07 Dustin Keplinger");
		items1.Add("BotB08 Rhianna Pinkerton");
		items1.Add("BotB09 Will Mielke");
		items1.Add("BotB10 Lowell Novitch");
		items1.Add("BotB11");
		items1.Add("BotB12 Jacob Presley");
		items1.Add("BotB13 Skyler Powers");
		items1.Add("BotB14 Dieter Voegels");
		items1.Add("BotB15 Jacob Wymer");
		items1.Add("BotB16 Chase Graves");
		items1.Add("BotB17 Dillon Hahn");
		items1.Add("BotB18 Quinn Beierle");
		items1.Add("BotB19");
		items1.Add("BotB20");
		
		List<string> items2 = new List<string>();
		items2.Add("");
		items2.Add("BotB00");
		items2.Add("BotB01 Robert Seiver");
		items2.Add("BotB02 Will Pritz");
		items2.Add("BotB03 John Paul Abides");
		items2.Add("BotB04 Austin Stead");
		items2.Add("BotB05 Carlos Garcia-Perez");
		items2.Add("BotB06 Ethan Villasin");
		items2.Add("BotB07 Dustin Keplinger");
		items2.Add("BotB08 Rhianna Pinkerton");
		items2.Add("BotB09 Will Mielke");
		items2.Add("BotB10 Lowell Novitch");
		items2.Add("BotB11");
		items2.Add("BotB12 Jacob Presley");
		items2.Add("BotB13 Skyler Powers");
		items2.Add("BotB14 Dieter Voegels");
		items2.Add("BotB15 Jacob Wymer");
		items2.Add("BotB16 Chase Graves");
		items2.Add("BotB17 Dillon Hahn");
		items2.Add("BotB18 Quinn Beierle");
		items2.Add("BotB19");
		items2.Add("BotB20");
		items2.Add("BotA01 Oussama Khalaf");
		items2.Add("BotA02 Ryan Heath");
		items2.Add("BotA03 Jiwon Jung");
		items2.Add("BotA04 Akshat Madan");
		items2.Add("BotA05 Alora Newbury");
		items2.Add("BotA06 Ryan Garvan");
		items2.Add("BotA07 Matthew Klingman");
		items2.Add("BotA08 Lorenzo DeMaine");
		items2.Add("BotA09 Mark Culp");
		items2.Add("BotA10 David Dasky");
		items2.Add("BotA11 Amogh Subhedar");
		items2.Add("BotA12 Mars Jurich");
		items2.Add("BotA13 Devon Peterson");
		items2.Add("BotA14 Keanu Cendejas");
		items2.Add("BotA15 Amy Stoltz");
		items2.Add("BotA16 Keegan Tompkins");
		items2.Add("BotA17 Devin Cavness");
		items2.Add("BotA18 Aaron Johnson");
		items2.Add("BotA19 Zacary Brown");
		items2.Add("BotA20 Adam Doolittle");

		//fill dropdown with items
		if (pNum == 1){
			foreach(var item in items1){
				dropdown.options.Add(new Dropdown.OptionData(){text = item});
			}
		}
		else if (pNum == 2){
			foreach(var item in items2){
				dropdown.options.Add(new Dropdown.OptionData(){text = item});
			}
		}
		
		DropdownItemSelected(dropdown);
		dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
		
		 //This script should be attached to Item
		 Toggle toggle = gameObject.GetComponent<Toggle>();
		 Debug.Log(toggle);
		 if (toggle != null && toggle.name == "Item 1: Option B"){
		 		toggle.interactable = false;
		 }
    }

    void DropdownItemSelected(Dropdown dropdown){
        int index = dropdown.value;
		//string firstWord = item.Split(' ')[0]; //the .Split part IDs the first word, so names can be added
		playerChoice = dropdown.options[index].text.ToString().Split(' ')[0];
		playerChoiceName = dropdown.options[index].text.ToString();
		
		Debug.Log("Player " + pNum + " Choice: " + playerChoiceName);
		
		if (pNum == 1){
			gameHandlerObj.p1PrefabName = playerChoice;
			gameHandlerObj.p1PlayerChoiceName = playerChoiceName;
		}
		else if (pNum == 2){
			gameHandlerObj.p2PrefabName = playerChoice;
			gameHandlerObj.p2PlayerChoiceName = playerChoiceName;
		}	
    }
}
