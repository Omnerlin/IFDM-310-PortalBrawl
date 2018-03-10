using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	/**Scene must be added to the Build to be able to be loaded!*/
	public string sceneToLoad;
	public bool hasBeenPressed = false;

	public void loadScene()
	{
		if (!hasBeenPressed) 
		{
            //TODO: Change to actual scene
            SceneTransitionManager.Instance.TransitionToScene(sceneToLoad, SceneTransitionManager.AnimationType.forward);
			hasBeenPressed = true;
		}
	}
}
