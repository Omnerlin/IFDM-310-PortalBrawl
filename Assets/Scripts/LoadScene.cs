using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    /**Scene must be added to the Build to be able to be loaded!*/

    // The prefab used to instantiate a sceneTransitionCanvas if one doesn't exist
    public GameObject sceneTransitionCanvasPrefab;

	public string sceneToLoad;
	public bool hasBeenPressed = false;

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


            SceneTransitionManager.Instance.TransitionToScene(sceneToLoad, SceneTransitionManager.AnimationType.forward);
			hasBeenPressed = true;
		}
	}
}
