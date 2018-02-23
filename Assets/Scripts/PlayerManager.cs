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

	private void Start()
	{
		if (GlobalControl.instance.loadData ().characterName != null) 
		{
			AddPlayer (GlobalControl.instance.loadData ().playerNumber);
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
			
    }

    // Add a controller to the list of initialized controls
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
