using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class LoadScene : MonoBehaviour {

    /**Scene must be added to the Build to be able to be loaded!*/

    // The prefab used to instantiate a sceneTransitionCanvas if one doesn't exist
    public GameObject sceneTransitionCanvasPrefab;

	public string sceneToLoad;
	public bool hasBeenPressed = false;
	public SpriteRenderer myRenderer;

	void Start () 
	{
		myRenderer = GetComponent<SpriteRenderer> ();
	}

	public void loadScene()
	{
		if (!hasBeenPressed) 
		{
            //TODO: Change to actual scene

            // If the sceneTransitionManager does not exist in this scene, go ahead and instantiate the transition canvas.
            if(!SceneTransitionManager.Instance)
            {
                Instantiate(sceneTransitionCanvasPrefab);
            }

            // If we're trying to load the main menu, that means that we want to clear all of our saved data.
            // We also want to reset the controllers so that we can still hit start in character select
            if(sceneToLoad == "MainMenu")
            {

                foreach(Rewired.Player player in ReInput.players.GetPlayers(false))
                {
                    player.isPlaying = false;
                }
            }


            SceneTransitionManager.Instance.TransitionToScene(sceneToLoad, SceneTransitionManager.AnimationType.forward);
			hasBeenPressed = true;
		}
	}

    public void ReloadScene()
    {
        SceneTransitionManager.Instance.TransitionToScene(SceneManager.GetActiveScene().name, SceneTransitionManager.AnimationType.forward);
    }

	void OnTriggerEnter2D (Collider2D other)
	{
		myRenderer.enabled = true;
	}

	//When the cursor leaves your area
	void OnTriggerExit2D (Collider2D other)
	{
		myRenderer.enabled = false;
	}
}
