using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_LoadPlayers : MonoBehaviour{
	
	//this script should be added to all co-op monsters along with the tag "CoopNPCMonster"
	//it simply allows a main move-and-attack script to know if the players are present so they can be loaded for targeting
	
	public bool playersReady = false;

	public void LoadPlayerTargets(){
		playersReady = true;
		
	}


}
