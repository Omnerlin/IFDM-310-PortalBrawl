using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/* *
 * This class holds all the info for a player character and is responsible for the loading and saving of said data.
 * When this class is created, it must be given a playerNumber that equals the corrisponding character's Rewired.Player ID.
 * This number is used to load and save data from the Global Control object as an ID.
 * */

public class Player : MonoBehaviour {

    public Transform startPosition;
	public int playerNumber; //Numbers have colors associated with them.
    [HideInInspector] public int controllerID;
	public int maxHP; //load start in prefab
	//public int currentHP;

	private PlayerInfo myData = new PlayerInfo();

	//The player's color is at the index of the playerNumber
	public static Color[] playerColors = {Color.blue, Color.magenta, Color.green, Color.yellow, Color.black};

	public Text myDisplay; //This may get bigger.

	public Color getColor()
	{
		return playerColors [playerNumber];
	}

	// Use this for initialization
	void Awake () 
	{
		
	}

	void Start()
	{
		//setPlayerNumber (playerNumber);
		loadPlayerData ();
		if (!String.IsNullOrEmpty(myData.characterName)) //If they have a character assigned
		{
			Debug.Log ("Player " + playerNumber +" loading data of character " + myData.characterName);
			SpriteRenderer myRenderer = GetComponent<SpriteRenderer> ();
			myRenderer.sprite = Resources.Load<Sprite> (myData.characterName);
			if (myData.currentHealth == -1)
				setMaxHP();
		}
		myData.playerNumber = playerNumber;

		myDisplay.color = playerColors[playerNumber];
	}

	void Update()
	{
		updateStatDisplay ();
		if (myData.currentHealth <= 0) {
			myData.currentHealth = 0;
			die();
		}
	}

	void OnDestroy()
	{
		savePlayerData ();
	}

	public void updateStatDisplay()
	{ //todo: Change this so it shows local stats
		myDisplay.text = myData.toString ();
	}

	public string getCharacterName() { return myData.characterName; }

	public void setCharacterName(string name) { myData.characterName = name; }

	public void setDisplay(Text display) { myDisplay = display; }

	public int getPlayerNumber(){ return playerNumber; }

    public void SetControllerID(int id)
    {
        controllerID = id;
        myData.controllerID = id;
    }

	public void setPlayerNumber(int num)
	{
		playerNumber = num;
		myData.playerNumber = num;
	}

	//Save info with the GlobalControl object so that it can be reloaded in the next scene
	public void savePlayerData()
	{
		Debug.Log ("Player " + playerNumber + " saving data of character " + myData.characterName + " with controller ID " + controllerID);

        if(!GlobalControl.instance)
        {
            Debug.LogWarning("Global Control Instance doesn't exist: did not save character data");
            return;
        }

		//myData.currentHealth = currentHP;

		GlobalControl.instance.SaveData(playerNumber, myData);
	}

	public void loadPlayerData()
	{
        if(!GlobalControl.instance)
        {
            Debug.LogWarning("Global Control Instance doesn't exist");
            return;
        }

		myData = GlobalControl.instance.loadData(playerNumber);
	}


	//Health stuff

	public void hurtPlayer(int damage)
	{
		myData.currentHealth -= damage;
	}

	public void healPlayer(int heal)
	{
		myData.currentHealth += heal;
	}

	public void setMaxHP()
	{
		myData.currentHealth = maxHP;
	}

	public void die()
	{
		Destroy(gameObject);
	}

}
