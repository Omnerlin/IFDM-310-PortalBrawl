using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class serves as a storage location for all of the class Player's data.
 * It uses the array of the struct PlayerInfo called savedPlayerData to store and load that info.
 * Each Player's information is saved in the array index respective to its playerNumber, or Rewired.Player ID.
 * To save, call saveData()
 * To load, call loadData()
 */

public class GlobalControl : MonoBehaviour {

	public static GlobalControl instance;
	public PlayerInfo[] savedPlayerData = new PlayerInfo[5]; //5 data members because 4+keyboard


	void Awake()
	{
		if (instance == null) 
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		} 
		else if (instance != this)	Destroy(gameObject);
	}

	void Start()
	{
		
	}
		

	//Call this method to save your data as a player
	public void saveData(int playerNumber, PlayerInfo data)
	{
		instance.savedPlayerData[playerNumber] = data;
		Debug.Log ("Saved: "+toString());
	}

	//Players call this method to load data when the object is created initially in each new scene.
	public PlayerInfo loadData(int playerNumber)
	{
		Debug.Log ("Loaded: "+ toString());

		if (savedPlayerData [playerNumber] == null) //If this player has not been initialized before, initialize it.
			savedPlayerData [playerNumber] = new PlayerInfo ();
		return savedPlayerData[playerNumber];
	}

	//Returns true if the player has saved any data.
	public bool hasPlayer(int playerNumber)
	{
		return savedPlayerData [playerNumber] != null;
	}

	public string toString()
	{
		string toPrint = "GlobalControl contains:\n";
		for(int i=0; i<savedPlayerData.Length; i++)
		{
			if(savedPlayerData[i] != null && !string.IsNullOrEmpty(savedPlayerData[i].characterName))
				toPrint = toPrint + savedPlayerData[i].toString() +"\n";
			else toPrint = toPrint + "null\n";
		}
		return toPrint;
	}
		
}
