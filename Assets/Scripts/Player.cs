using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* *
 * This class holds all the info for a player character and is responsible for the loading and saving of said data.
 * When this class is created, it must be given a playerNumber that equals the corrisponding character's Rewired.Player ID.
 * This number is used to load and save data from the Global Control object as an ID.
 * */

public class Player : MonoBehaviour {

    public Transform startPosition;
	public int playerNumber; //Numbers have colors associated with them.

	private PlayerInfo localPlayerData = new PlayerInfo();

	//The player's color is at the index of the playerNumber
	public static Color[] playerColors = {Color.blue, Color.magenta, Color.green, Color.yellow, Color.black};
	//public static string pathToLoadSprites = "Assets/Sprites/Placeholder/Characters/Resources/";

	public Color getColor()
	{
		return playerColors [playerNumber];
	}

	// Use this for initialization
	void Awake () 
	{
		//setPlayerNumber (playerNumber);
	}
	
	// Update is called once per frame
	void Start () 
	{
		loadPlayerData ();
		if (!String.IsNullOrEmpty(localPlayerData.characterName)) //If they have a character assigned
		{
			Debug.Log ("Player "+ playerNumber +" loading data of character "+localPlayerData.characterName);
			SpriteRenderer myRenderer = GetComponent<SpriteRenderer> ();
			myRenderer.sprite = Resources.Load<Sprite> (localPlayerData.characterName);
		}
		localPlayerData.playerNumber = playerNumber;
	}

	void OnDestroy()
	{
		savePlayerData ();
	}

	public string getCharacterName() { return localPlayerData.characterName; }

	public void setCharacterName(string name) { localPlayerData.characterName = name; }

	public int getPlayerNumber(){ return playerNumber; }

	public void setPlayerNumber(int num)
	{
		playerNumber = num;
		localPlayerData.playerNumber = num;
	}

	//Save info with the GlobalControl object so that it can be reloaded in the next scene
	public void savePlayerData()
	{
		Debug.Log ("Player " + playerNumber + " saving data of character " + localPlayerData.characterName);
		GlobalControl.instance.saveData(playerNumber, localPlayerData);
	}

	public void loadPlayerData()
	{
		localPlayerData = GlobalControl.instance.loadData(playerNumber);
	}

}
