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
	public float maxUltimate;
	//public int currentHP;
	private bool hasInitialized = false; //This flag is here because Update was being called before Start. This allows health to be initialized.

	private PlayerInfo myData = new PlayerInfo();

	//The player's color is at the index of the playerNumber
	public static Color[] playerColors = {Color.blue, Color.magenta, Color.green, Color.yellow, Color.black};
	public static Color[] ultimateColors = {new Color(135,0,225,225), new Color(157,358,174,225)}; //Purple for charged, pale purple for charging

	public PlayerStatDisplay myDisplay;

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
			//SpriteRenderer myRenderer = GetComponent<PlayerControl>().<SpriteRenderer> ();
			//myRenderer.sprite = Resources.Load<Sprite> (myData.characterName);
			if (myData.currentHealth == -1) 
			{
				setMaxHP ();
				setMaxUltimate ();
			}
		}
		myData.playerNumber = playerNumber;

		myDisplay.getText().color = playerColors[playerNumber];

		hasInitialized = true;
	}

	void Update()
	{
        if(Input.GetKeyDown(KeyCode.H))
        {
            hurtPlayer(5);
        }

		//Allows the stats to be initialized before assigning and displaying them.
		if (!hasInitialized)
			return;

		rechargeUltimate ();

		updateStatDisplay ();
		if (myData.currentHealth == 0) 
		{
			Debug.Log ("Player " + playerNumber + " playing " + myData.characterName + "has died.");
			//myData.currentHealth = 0;
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
		myDisplay.getHealthBar().GetComponent<StatBar>().updateBar (myData.currentHealth, maxHP);
		// myDisplay.getUltimateBar ().GetComponent<StatBar> ().updateBar (myData.currentUltimate, maxUltimate);
	}

	public string getCharacterName() { return myData.characterName; }

	public void setCharacterName(string name) { myData.characterName = name; }

	public void setDisplay(GameObject display) 
	{
		myDisplay = display.GetComponent<PlayerStatDisplay> ();
		if (myDisplay == null) 
		{
			Debug.LogWarning ("Display passed did not have a PlayerStatDisplay script component.");
			return;
		}
		myDisplay.getUltimateBar ().SetActive (true);
		myDisplay.getHealthBar ().SetActive (true);
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
		if (myData.currentHealth <= 0)
        {
			myData.currentHealth = 0;
            StopAllCoroutines();
            GetComponent<PlayerControl>().characterBody.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(PlayerFlash());
        }

		Debug.Log (myData.characterName + " took " + damage + " damage! "+myData.currentHealth+" health remaining! (Don't die)");
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

	public void die() //TODO: delete this?
	{
		//State classes cause this to happen.
		//Destroy(this.gameObject); 
	}

	public bool isDead()
	{
		return myData.currentHealth <= 0;
	}

	public void revivePlayer(int revive)
	{
	 	//update a local variable that charges up, and decreases when someone stops charging it?
	}

	//Ultimate, notes made by Anna
	//Ultimate: Can be set to max, can be used, recharged over time, and be checked to see if it is fully charged.
	//Call ultimateIsCharged() to check if the Ultimate can be used.
	//Call useUltimate() to set the charge to 0 and begin the recharge process again.

	public void setMaxUltimate(){ myData.currentUltimate = maxUltimate; } 

	public void useUltimate() { 
		myData.currentUltimate = 0;
		myDisplay.getUltimateBar ().GetComponent<Image> ().color = ultimateColors [1]; //Color it uncharged
	}

	public void rechargeUltimate()
	{
		if (ultimateIsCharged())
			return;
		else if (myData.currentUltimate + Time.deltaTime >= maxUltimate) //If one more step will charge it
		{
			myDisplay.getUltimateBar ().GetComponent<Image> ().color = ultimateColors [1]; //Color it uncharged
			myData.currentUltimate += Time.deltaTime;
		}
		else  //(myData.currentUltimate < maxUltimate) 
		{
			myData.currentUltimate += Time.deltaTime;
		}
	}

	public bool ultimateIsCharged() { return myData.currentUltimate == maxUltimate; }


    IEnumerator PlayerFlash()
    {
        bool visible = false;
        float hurtTimer = hurtInvincibilityDuration;
        float timeSinceLastFlash = 0;
        SpriteRenderer renderer = GetComponent<PlayerControl>().characterBody.GetComponent<SpriteRenderer>();
        renderer.enabled = visible;

        while (hurtTimer > 0)
        {
            hurtTimer -= Time.deltaTime;
            timeSinceLastFlash += Time.deltaTime;

            // Swap between visible/invisible if we've reached our interval time
            if(timeSinceLastFlash >= hurtFlashInterval)
            {
                visible = !visible;
                renderer.enabled = visible;
                timeSinceLastFlash = 0;
            }

            yield return null;
        }

        renderer.enabled = true;
    }

}
