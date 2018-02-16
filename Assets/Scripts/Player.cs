using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Transform startPosition;
	public string characterName { get; set; }
    public int playerNumber { get; set; } //Numbers have colors associated with them.

	public PlayerInfo myInfo;

	//The player's color is at the index of the playerNumber
	public static Color[] playerColors = {Color.blue, Color.magenta, Color.green, Color.yellow, Color.black};

	public Color getColor()
	{
		return playerColors [playerNumber];
	}

	// Use this for initialization
	void Start () 
	{
		//GlobalControl.load (playerNumber);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Register yourself with Global so your info can be reloaded next time.
	public void registerGlobal()
	{
		//GlobalControl.instance.register (myInfo);
	}
}
