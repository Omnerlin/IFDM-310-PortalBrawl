using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour {

	public static GlobalControl instance;
	private PlayerInfo savedPlayerData = new PlayerInfo ();

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

	public void savePlayers()
	{
		
	}

	//Registers the player so that their info will be saved when the new scene is loaded.
	public bool register(PlayerInfo into)
	{
		return true;
	}

	//Call this method to save your data as a player
	//todo: add an ID to this.
	public void saveData(PlayerInfo data)
	{
		instance.savedPlayerData = data;
	}

	//Players call this method to load data when the object is created initially in each new scene.
	public PlayerInfo loadData()
	{
		return savedPlayerData;
	}
		
}
