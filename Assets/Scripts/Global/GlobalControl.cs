﻿using System.Collections;
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
			DontDestroyOnLoad(this.transform.root.gameObject);
			instance = this;
		} 
		else if (instance != this)	Destroy(transform.root.gameObject);
	}
		

	//Call this method to save your data as a player
	public void SaveData(int playerNumber, PlayerInfo data)
	{
		instance.savedPlayerData[playerNumber] = data;
		Debug.Log ("Saved: "+toString());
	}

	//Players call this method to load data when the object is created initially in each new scene.
	public PlayerInfo loadData(int playerNumber)
	{
		Debug.Log ("Loaded: "+ toString());

		if (savedPlayerData [playerNumber] == null) // If this player has not been initialized before, initialize it.
			savedPlayerData [playerNumber] = new PlayerInfo ();
		return savedPlayerData[playerNumber];
	}

	//Returns true if the player has saved any data.
	public bool HasPlayer(int playerNumber)
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public bool AllPlayersDead()
    {
        for(int i = 0; i < savedPlayerData.Length; i++)
        {
            if (savedPlayerData[i] != null && savedPlayerData[i].currentHealth <= 0)
            {
                return true;
            }
        }

        return false;
    }

}
