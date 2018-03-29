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
	private bool hasInitialized = false; //This flag is here because Update was being called before Start. This allows health to be initialized.

	private PlayerInfo myData = new PlayerInfo();

	//The player's color is at the index of the playerNumber
	public static Color[] playerColors = {Color.blue, Color.magenta, Color.green, Color.yellow, Color.black};

	public PlayerStatDisplay myDisplay; //This may get bigger.

    [Tooltip("// Interval speed at which the player will become invisible/visible while hurt")]
    public float hurtFlashInterval = 0.1f;

    [Tooltip("How long the player will be invincible after taking damage")]
    public float hurtInvincibilityDuration = 1f;


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

		myDisplay.getText().color = playerColors[playerNumber];

		hasInitialized = true;
	}

	void Update()
	{
        if(Input.GetKeyDown(KeyCode.H))
        {
            hurtPlayer(0);
        }

		if (!hasInitialized)
			return;
		
		updateStatDisplay ();
		if (myData.currentHealth <= 0) 
		{
			Debug.Log ("Player " + playerNumber + " playing " + myData.characterName + "has died.");
			myData.currentHealth = 0;
			die();
		}
	}

	void OnDestroy()
	{
		savePlayerData ();
	}

	public void updateStatDisplay()
	{
		myDisplay.getText().text = myData.toString ();
		myDisplay.getHealthBar().GetComponent<healthBar>().updateBar (myData.currentHealth, maxHP);
	}

	public string getCharacterName() { return myData.characterName; }

	public void setCharacterName(string name) { myData.characterName = name; }

	public void setDisplay(GameObject display) 
	{
		myDisplay = display.GetComponent<PlayerStatDisplay> ();
		if (myDisplay == null)
			Debug.LogWarning("Display passed did not have a PlayerStatDisplay script component.");
	}

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

	//Takes damage out of myData.currentHealth. Cannot drop below 0.
	//Dying is in Update.
	public void hurtPlayer(int damage)
	{
		myData.currentHealth -= damage;
		if (myData.currentHealth < 0)
			myData.currentHealth = 0;
        else
        {
            StopAllCoroutines();
            StartCoroutine(PlayerFlash());
        }
	}

	//Adds heal to myData.currentHeath. Cannot go above maxHP.
	public void healPlayer(int heal)
	{
		myData.currentHealth += heal;
		if (myData.currentHealth > maxHP)
			setMaxHP ();
	}

	public void setMaxHP()
	{
		myData.currentHealth = maxHP;
	}

	public void die()
	{
		Destroy(gameObject);
	}

    IEnumerator PlayerFlash()
    {
        bool visible = false;
        float hurtTimer = hurtInvincibilityDuration;
        float timeSinceLastFlash = 0;
        GetComponent<SpriteRenderer>().enabled = visible;

        while (hurtTimer > 0)
        {
            hurtTimer -= Time.deltaTime;
            timeSinceLastFlash += Time.deltaTime;

            // Swap between visible/invisible if we've reached our interval time
            if(timeSinceLastFlash >= hurtFlashInterval)
            {
                visible = !visible;
                GetComponent<SpriteRenderer>().enabled = visible;
                timeSinceLastFlash = 0;
            }

            yield return null;
        }

        GetComponent<SpriteRenderer>().enabled = true;
    }

}
