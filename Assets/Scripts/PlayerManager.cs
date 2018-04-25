using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    public GameObject portraitPrefab;
    public GameObject playerPortraitGroup;

	public GameObject[] playerDisplays = new GameObject[4]; //Indexed by PLAYER number, NOT Rewired-ID

    //private List<int> assignedControllers = new List<int>();
    public GameObject playerPrefab;

    private void Awake()
    {
        // This will be a singleton
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }

        Debug.Log("Trying to load players");

        // Instaniate characters based on their character name
        if(!GlobalControl.instance)
        {
            return;
        }

        for (int i = 0; i < GlobalControl.instance.savedPlayerData.Length; i++)
        {
            if (GlobalControl.instance.savedPlayerData[i] != null && !string.IsNullOrEmpty(GlobalControl.instance.savedPlayerData[i].characterName))
            {
                // Run through the list of character names and instaniate the prefab that matches
                // Doing this since we only have 4 characters. Otherwise we could do something more generic 
                Debug.Log("Iteration " + i);
                GameObject playerObject = null;
				Debug.Log ("Instanciating " + GlobalControl.instance.savedPlayerData[i].characterName + " prefab from PlayerManager");
                switch(GlobalControl.instance.savedPlayerData[i].characterName)
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

                //gameObject yaboi = 
                //playerObject.GetComponent<Player>().characterNameImage
                GameObject portrait = Instantiate(portraitPrefab, playerPortraitGroup.transform);
                //portrait.transform.localScale = new Vector3(1, 1, 1);

                // Assign the character portrait image and name
                portrait.transform.Find("Portrait").GetComponent<Image>().sprite = playerObject.GetComponent<Player>().characterPortraitImage;
                portrait.transform.Find("Name").GetComponent<Image>().sprite = playerObject.GetComponent<Player>().characterNameImage;

                //Assign the playerNumber and the text display object to the player for its use.
                playerObject.GetComponent<Player>().playerNumber = GlobalControl.instance.savedPlayerData[i].playerNumber;
				playerObject.GetComponent<Player> ().setDisplay( portrait );
                playerObject.GetComponent<PlayerControl>().player = ReInput.players.GetPlayer(GlobalControl.instance.savedPlayerData[i].controllerID);

                // Add the player to the group of objects to be tracked by the camera
                // while keeping the other targets
                CinemachineTargetGroup groupComp = cameraFollowGroup.GetComponent<CinemachineTargetGroup>();
                List<CinemachineTargetGroup.Target> group = new List<CinemachineTargetGroup.Target>(groupComp.m_Targets);
                CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target();
                newTarget.target = playerObject.transform;
                newTarget.weight = 1.0f;
                group.Add(newTarget);

                groupComp.m_Targets = group.ToArray();
            }
        }
    }

    private void Update()
    {
        //TODO: Delete this. Characters should only be spawned from the Start.
        // We need to test if any players want to join the game

        if (ReInput.players == null)
            return;

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
		//playerObject.GetComponent<Player> ().myDisplay = playerDisplays [info.playerNumber];

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
