﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler_LastBot_V2 : MonoBehaviour
{
	public GameHandler_Version2 gameHandlerObj;
	public GameObject MenuLastBots;
	public GameObject lastBotsP1Text;
	public GameObject lastBotsP2Text;
	public static GameObject lastPlayer1;
	public static GameObject lastPlayer2;

	void Start()
	{
		gameHandlerObj = gameObject.GetComponent<GameHandler_Version2>();
		MenuLastBots.SetActive(false);
	}

	public void UpdateLastBots(GameObject p1, GameObject p2)
	{
		MenuLastBots.SetActive(true);
		lastPlayer1 = p1;
		lastPlayer2 = p2;

		Text lastBotsP1TextB = lastBotsP1Text.GetComponent<Text>();
		lastBotsP1TextB.text = ("" + p1.name);

		Text lastBotsP2TextB = lastBotsP2Text.GetComponent<Text>();
		lastBotsP2TextB.text = ("" + p2.name);
	}

}
