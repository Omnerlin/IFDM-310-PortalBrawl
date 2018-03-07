using System.Collections;
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
        Debug.Log("EYYYYYYYYYYYYYYYYYYYYYYY");
        assignedPlayers.Add(ReInput.players.GetPlayer(playerID));
		GameObject pFab = Instantiate(cursorPrefab);
		CursorControl pControl = pFab.GetComponent<CursorControl>();
		pControl.rewiredPlayer = ReInput.players.GetPlayer(playerID);
		pControl.rewiredPlayer.isPlaying = true;
        pControl.GetComponent<Player>().playerNumber = assignedPlayers.Count - 1;
        pControl.GetComponent<SpriteRenderer>().color = pControl.GetComponent<Player>().getColor();

        //camera tracking removed
    }

}
