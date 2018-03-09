using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

/* This class was made to be used for transitioning between scenes so that they all
 * flow together more nicely. The gamobject that holds this script will persist between scenes,
 * meaning that there should only ever be one active at a time (singleton)
 */

public class SceneTransitionManager : MonoBehaviour {

    // This will persist as a singleton between scenes.
    public static SceneTransitionManager Instance;

    // Get a reference to the input manager so that we can disable input in between scenes.
    public GameObject rewiredManager;

    [Tooltip("Should controller input be disabled while transitioning?")]
    public bool disableInputWhileTransitioning = true;

    // Elements that we'll be animating
    public Image swipeImage;
    public Image whiteFadeImage;

    // We'll use this to know what animation to play in between scenes
    private AnimationType currentAnimType = AnimationType.none;

    private bool playingSceneTransition = false;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            // Subscribe to the scene manager loading scenes so that
            // we know when to play entrance animations
            SceneManager.sceneLoaded += PlaySceneEnterAnimation;
            
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if(playingSceneTransition)
        {

        }
    }

    // Transition to the next scene going forward.
    public void TransitionForwardToScene(string sceneName, AnimationType animType)
    {
        // Disable the rewiredInputManager if that's what we wanted, and transition to the next scene
        rewiredManager.SetActive(disableInputWhileTransitioning);

        // Play animation based on type we gave
        switch (animType)
        {
            case AnimationType.forward:
                swipeImage.GetComponent<Animator>().Play("SwipeInFromLeft");
                break;
            case AnimationType.backward:
                swipeImage.GetComponent<Animator>().Play("SwipeInFromRight");
                break;
        }

        SceneManager.LoadScene(sceneName);
        
    }

    // This will play automatically in the next scene if "TransitionToScene" was called, and the animation will
    // play based on the type supplied in that function as well.
    private void PlaySceneEnterAnimation(Scene scene, LoadSceneMode mode)
    {

    }

    // Functions to disable all controls when transitioning, just in case it's possible 
    // something weird could happen 
    public void DisableRewiredInput()
    {
        rewiredManager.SetActive(false);
    }

    public void EnableRewiredInput()
    {
        rewiredManager.SetActive(true);
    }

    public enum AnimationType
    {
        forward,
        backward,
        none
    }
}
