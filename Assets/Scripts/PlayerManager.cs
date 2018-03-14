using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    public GameObject cameraFollowGroup;

    public GameObject anixPrefab;
    public GameObject dennisPrefab;
    public GameObject revdiocPrefab;
    public GameObject zerandiPrefab;

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

        // Instaniate characters based on their character name
        foreach(PlayerInfo info in GlobalControl.instance.savedPlayerData)
        {
            if (info != null && !string.IsNullOrEmpty(info.characterName))
            {
                // Run through the list of character names and instaniate the prefab that matches
                // Doing this since we only have 4 characters. Otherwise we could do something more generic 
                GameObject playerObject = null;
                switch(info.characterName)
                {
                    case "Anix":
                        playerObject = Instantiate(anixPrefab);
                        break;
                    case "Dennis":
                        playerObject = Instantiate(dennisPrefab);
                        break;
                    case "Revdioc":
                        playerObject = Instantiate(revdiocPrefab);
                        break;
                    case "Zerandi":
                        playerObject = Instantiate(zerandiPrefab);
                        break;
                }

                // Return with an error if the player name doesn't match any of the cases above
                if(!playerObject)
                {
                    Debug.LogError("Player object name could not find a prefab to spawn!");
                    return;
                }

                playerObject.GetComponent<Player>().playerNumber = info.playerNumber;
                playerObject.GetComponent<PlayerControl>().player = ReInput.players.GetPlayer(info.controllerID);
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
        MeleePlayer pControl = pFab.GetComponent<MeleePlayer>();
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
