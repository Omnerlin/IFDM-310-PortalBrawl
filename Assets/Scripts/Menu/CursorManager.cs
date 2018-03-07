﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Cinemachine;

/* Created by Anna Carey using PlayerManager
 * Manages the cursors, adding them to the game when they press 'start.'
*/

public class CursorManager : MonoBehaviour {

	public static CursorManager instance;

	private List<Rewired.Player> assignedPlayers = new List<Rewired.Player>();
	public GameObject cursorPrefab;

	private void Awake()
	{
		// This will be a singleton
		if(instance != null)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	private void Update()
	{
		// We need to test if any players want to join the game
		IList <Rewired.Player> players = ReInput.players.GetPlayers(false);
		for(int i = 0; i < players.Count; i++)
		{
			// Assign the player
			if(!players[i].isPlaying && players[i].GetButtonDown("Start"))
			{
				AddPlayer(i);                
			}
		}
		//TODO: Enable 'continue' once everyone who has joined has selected a character.
	}

	// Add a controller to the list of initialized controls
	private void AddPlayer(int playerID)
	{
        // Add the player to the array of assigned players. This is mainly just to make sure that the player number is set based on 
        // the order of who pressed start. The player's number should be treated separately from the player's controller ID.
        assignedPlayers.Add(ReInput.players.GetPlayer(playerID));


		GameObject pFab = Instantiate(cursorPrefab);
		CursorControl pControl = pFab.GetComponent<CursorControl>();
		pControl.rewiredPlayer = ReInput.players.GetPlayer(playerID);

        // Set the variables for the cursor control here. We could possibly call a public method called "Init"
        // on the cursor controll script to initialize all these values. This shouldn't be done in the script because 
        // the start and awake methods will override what we are trying to set here.
		pControl.rewiredPlayer.isPlaying = true;
        pControl.GetComponent<Player>().setPlayerNumber(assignedPlayers.Count - 1);
        pControl.GetComponent<Player>().SetControllerID(playerID);
        pControl.GetComponent<SpriteRenderer>().color = pControl.GetComponent<Player>().getColor();

        //camera tracking removed
    }

}
