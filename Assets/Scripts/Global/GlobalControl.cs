using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour {

	public static GlobalControl instance;

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
		
}
