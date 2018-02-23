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
	private PlayerInfo[] savedPlayerData = new PlayerInfo[] {new PlayerInfo (), new PlayerInfo(), new PlayerInfo(), new PlayerInfo(), new PlayerInfo()}; //5 data members because 4+keyboard

	//Store varuables that need to be reloaded here.
	//Player[] players = Player[5];

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
	public void saveData(PlayerInfo data)
	{
		instance.savedPlayerData[data.playerNumber] = data;
	}

	//Players call this method to load data when the object is created initially in each new scene.
	public PlayerInfo loadData(int playerNumber)
	{
		return savedPlayerData[playerNumber];
	}

	//Returns the ID's of all players that created characters and are playing.
	public bool hasCharacter(int playerNumber)
	{
		return savedPlayerData [playerNumber] != null;
	}
		
}
