using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{

    private List<int> assignedControllers = new List<int>();
    public GameObject playerPrefab;

    private void Update()
    {
        // We need to test if any players want to join the game
        IList <Rewired.Player> players = Rewired.ReInput.players.GetPlayers(false);
        for(int i = 0; i < players.Count; i++)
        {
            // Assign the player 
            if(!players[i].isPlaying && players[i].GetButtonDown("Start"))
            {
                GameObject pFab = Instantiate(playerPrefab);
                PlayerControl pControl = pFab.GetComponent<PlayerControl>();
                pControl.player = Rewired.ReInput.players.GetPlayer(i);
                pControl.player.isPlaying = true;
            }
        }
    }

    // Add a controller to the list of initialized controls
    private void AddControllerAndPlayer(int ControlID)
    {
        assignedControllers.Add(ControlID);
        GameObject player = Instantiate(playerPrefab);
        playerPrefab.GetComponent<Player>().playerNumber = assignedControllers.Count;
        player.GetComponent<PlayerControl>().controllerNumber = ControlID;
        Debug.Log("Creating Player " + assignedControllers.Count + " using controller ID " + ControlID);
    }

}
