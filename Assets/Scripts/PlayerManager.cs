using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    public GameObject cameraFollowGroup;

    //private List<int> assignedControllers = new List<int>();
    public GameObject playerPrefab;

    private void Awake()
    {
        Debug.Log("Trying to load players");
        // This will be a singleton
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        //Check all of the RewiredPlayers. If they selected a character and have saved data, make them an instance
        //of the player by calling AddPlayer

        //IList<Rewired.Player> players = ReInput.players.GetPlayers(false);
        //for (int i = 0; i < players.Count; i++)
        //{
        //    // Assign the player 
        //    if (players[i].isPlaying && GlobalControl.instance.hasPlayer(players[i].id))
        //    {
        //        AddPlayer(i);
        //    }
        //}

        // Instaniate characters based 
        foreach(PlayerInfo info in GlobalControl.instance.savedPlayerData)
        {
            if (info != null && !string.IsNullOrEmpty(info.characterName))
            {
                GameObject go = Instantiate(playerPrefab);
               
                go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(info.characterName);
                go.GetComponent<Player>().playerNumber = info.playerNumber;
                go.GetComponent<PlayerControl>().player = ReInput.players.GetPlayer(info.controllerID);
            }
        }
    }

    private void Update()
    {
		//TODO: Delete this. Characters should only be spawned from the Start.
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
			
    }

    // Add a controller to the list of initialized controls
	//TODO: Some of this may be reduntant to happen in every scene.
    private void AddPlayer(int playerID)
    {
        GameObject pFab = Instantiate(playerPrefab);
        PlayerControl pControl = pFab.GetComponent<PlayerControl>();
        pControl.player = ReInput.players.GetPlayer(playerID);
        pControl.player.isPlaying = true;

        // Add the player to the group of objects to be tracked by the camera
        // while keeping the other targets
        CinemachineTargetGroup groupComp = cameraFollowGroup.GetComponent<CinemachineTargetGroup>();
        List<CinemachineTargetGroup.Target> group = new List<CinemachineTargetGroup.Target>(groupComp.m_Targets);
        CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target();
        newTarget.target = pFab.transform;
        newTarget.weight = 1.0f;
        group.Add(newTarget);

        groupComp.m_Targets = group.ToArray();
    }

}
