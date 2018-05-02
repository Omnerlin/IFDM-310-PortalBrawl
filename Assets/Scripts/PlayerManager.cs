using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Rewired;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance = null;
    public GameObject cameraFollowGroup;

    public GameObject anixPrefab;
    public GameObject dennisPrefab;
    public GameObject revdiocPrefab;
    public GameObject zerandiPrefab;

    public GameObject portraitPrefab;
    public GameObject playerPortraitGroup;

	// public GameObject[] playerDisplays = new GameObject[4]; //Indexed by PLAYER number, NOT Rewired-ID

    public GameObject playerPrefab;

    // Getting a reference to this so that we can close the portal after players are spawned.
    // We also want to be able to spawn players relative to the portal's position.
    public GameObject playerPortal;

    // Game win and loss screens. Putting them in playerManager for now, since I'm using it
    // to tell whether or not they should be displayed in the first place. Meh.
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    public Image fadeScreen;
    public float fadeTime = 1.5f;


    [HideInInspector] public List<GameObject> activePlayers = new List<GameObject>();

    private void Awake()
    {
        // This will be a singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerPortal.SetActive(true);

        // Add the portal to the cameraFollow
        CinemachineTargetGroup groupComp = cameraFollowGroup.GetComponent<CinemachineTargetGroup>();
        List<CinemachineTargetGroup.Target> group = new List<CinemachineTargetGroup.Target>(groupComp.m_Targets);
        CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target();
        newTarget.target = playerPortal.transform;
        newTarget.weight = 1.0f;
        group.Add(newTarget);
    }

    public void LoadPlayers()
    {
        Debug.Log("Trying to load players");

        // Instaniate characters based on their character name
        if (!GlobalControl.instance)
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
                Debug.Log("Instanciating " + GlobalControl.instance.savedPlayerData[i].characterName + " prefab from PlayerManager");
                switch (GlobalControl.instance.savedPlayerData[i].characterName)
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
                if (!playerObject)
                {
                    Debug.LogError("Player object name could not find a prefab to spawn!");
                    return;
                }

                GameObject portrait = Instantiate(portraitPrefab, playerPortraitGroup.transform);

                // Assign the character portrait image and name
                portrait.transform.Find("Portrait").GetComponent<Image>().sprite = playerObject.GetComponent<Player>().characterPortraitImage;
                portrait.transform.Find("Name").GetComponent<Image>().sprite = playerObject.GetComponent<Player>().characterNameImage;

                //Assign the playerNumber and the text display object to the player for its use.
                playerObject.GetComponent<Player>().playerNumber = GlobalControl.instance.savedPlayerData[i].playerNumber;
                playerObject.GetComponent<Player>().setDisplay(portrait);
                playerObject.GetComponent<PlayerControl>().player = ReInput.players.GetPlayer(GlobalControl.instance.savedPlayerData[i].controllerID);

                // Spawn the players in a row based on their player number
                int playerNum = playerObject.GetComponent<Player>().playerNumber;
                playerObject.transform.position = new Vector2(playerPortal.transform.position.x + playerNum - 2, playerPortal.transform.position.y);

                // We want to be able to keep track of the players that are spawned during this run so that we can do things like check if they are dead.
                activePlayers.Add(playerObject);

                // Add the player to the group of objects to be tracked by the camera
                // while keeping the other targets
                CinemachineTargetGroup groupComp = cameraFollowGroup.GetComponent<CinemachineTargetGroup>();
                List<CinemachineTargetGroup.Target> group = new List<CinemachineTargetGroup.Target>(groupComp.m_Targets);
                CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target();
                newTarget.target = playerObject.transform;
                newTarget.weight = 1.0f;
                group.Add(newTarget);
                
                // Remove the portal if it is there.
                foreach (CinemachineTargetGroup.Target target in group)
                {
                    if(target.target == playerPortal.transform)
                    {
                        group.Remove(target);
                    }
                }

                // Reassign the group as an array
                groupComp.m_Targets = group.ToArray();

                if(playerPortal)
                {
                    playerPortal.GetComponent<Animator>().SetTrigger("Close");
                }
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
            //if(!players[i].isPlaying && players[i].GetButtonDown("Start"))
            //{
            //    AddPlayer(i);                
            //}
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

    // Checks if all players have been killed to death
    public void CheckForGameOver()
    {
        bool killedToDeath = true;
        for (int i = 0; i < activePlayers.Count; i++)
        {
            if (activePlayers[i] != null && !activePlayers[i].GetComponent<Player>().isDead())
            {
                killedToDeath = false;
                break;
            }
        }

        if(killedToDeath)
        {
            // OI, it's game over time. Enable the correct menu. Choose whether or not to reload.
            //Debug.Log("OI, the characters are dead");

            StopAllCoroutines();
            StartCoroutine(FadeGameEnd(killedToDeath));
        }
    }

    public void GameWin()
    {
        StopAllCoroutines();
        StartCoroutine(FadeGameEnd(false));
    }

    void SetPlayerControlsEnabled(bool enabled)
    {
        foreach (GameObject go in activePlayers)
        {
            go.GetComponent<PlayerControl>().player.isPlaying = false;
        }
    }

    public void HealAllPlayersToFull()
    {
        foreach (GameObject go in activePlayers)
        {
            go.GetComponent<Player>().setMaxHP();
        }
    }

    public void SetAllPlayersInactive()
    {
        foreach (GameObject go in activePlayers)
        {
            go.SetActive(false);
        }
    }

    IEnumerator FadeGameEnd(bool gameLoss)
    {
        float timer = 0;

        // Set up initial conditions for coroutine, making sure that the fade image is completely transparent.
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 0);
        fadeScreen.gameObject.SetActive(true);

        // Fade in the black image until it is totally visible, then perform the appropriate actions based on the supplied boolean.
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, timer / fadeTime);
            yield return null;
        }

        timer = fadeTime;
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1);

        if(gameLoss)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            SetAllPlayersInactive();
            gameWinScreen.SetActive(true);
        }

        // Start fading the screen back out
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, timer / fadeTime);
            yield return null;
        }

        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 0);
        fadeScreen.gameObject.SetActive(false);

    }
}
