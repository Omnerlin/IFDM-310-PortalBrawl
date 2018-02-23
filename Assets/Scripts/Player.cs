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
    public int playerNumber { get; set; } //Numbers have colors associated with them.

	private PlayerInfo localPlayerData = new PlayerInfo ();

	//The player's color is at the index of the playerNumber
	public Color[] playerColors = {Color.blue, Color.magenta, Color.green, Color.yellow, Color.black};
	//public static string pathToLoadSprites = "Assets/Sprites/Placeholder/Characters/Resources/";

	public Color getColor()
	{
		return playerColors [playerNumber];
	}

	// Use this for initialization
	void Start () 
	{
		loadPlayerData ();
		if (localPlayerData.characterName != null) //If they have a character assigned
		{
			Debug.Log ("Player loading data from "+localPlayerData.characterName);
			SpriteRenderer myRenderer = GetComponent<SpriteRenderer> ();
			myRenderer.sprite = Resources.Load<Sprite> (localPlayerData.characterName);
			//I was going to do the switch-case, but then I realized that localPlayerData.characterName was the exact string we needed. --Anna
			//switch (localPlayerData.characterName) 
			//{
				//case "Anix":
				//	myRenderer.sprite = Resources.Load<Sprite> (pathToLoadSprites+"Anix");
				//	break;
			//}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
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

	//Register yourself with Global so your info can be reloaded next time.
	public void registerGlobal()
	{
		//GlobalControl.instance.register (myInfo);
	}

	//Save info with the GlobalControl object so that it can be reloaded in the next scene
	public void savePlayerData()
	{
		GlobalControl.instance.saveData(localPlayerData);
	}

	public void loadPlayerData()
	{
		//GlobalControl global = (GlobalControl)FindObjectOfType (GlobalControl);
		localPlayerData = GlobalControl.instance.loadData(playerNumber);
	}

}
