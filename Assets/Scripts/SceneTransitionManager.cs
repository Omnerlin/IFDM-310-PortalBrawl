using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;
using System;

/* This class was made to be used for transitioning between scenes so that they all
 * flow together more nicely. The gameObject that holds this script will persist between scenes,
 * meaning that there should only ever be one active at a time (singleton)
 */

public class SceneTransitionManager : MonoBehaviour {

    // This will persist as a singleton between scenes.
    public static SceneTransitionManager Instance;

    [Tooltip("How long it should take for swipe animations to complete (In seconds)")]
    public float swipeDuration = 0.2f;

    // Get a reference to the input manager so that we can disable input in between scenes.
    public GameObject rewiredManager;

    [Tooltip("Should controller input be disabled while transitioning?")]
    public bool disableInputWhileTransitioning = true;

    // Elements that we'll be animating
    public Image swipeImage;
    public Image whiteFadeImage;

    // We'll use this to know what animation to play in between scenes
    private AnimationType animToPlayOnNextLoad = AnimationType.none;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);            
        }
        else
        {
            Destroy(this);
        }

        // Subscribe to the scene manager loading scenes so that
        // we know when to play entrance animations and look for the rewired input thing
        SceneManager.sceneLoaded += PlaySceneEnterAnimation;
        SceneManager.sceneLoaded += FindRewiredInput;

        // Look for rewird input in awake because it's the only time it's called.
        rewiredManager = GameObject.Find("Rewired Input Manager");
    }

    private void FindRewiredInput(Scene scene, LoadSceneMode mode)
    {
        rewiredManager = GameObject.Find("Rewired Input Manager");
    }

    // Transition to the next scene going forward.
    public void TransitionToScene(string sceneName, AnimationType animType)
    {
        // Disable the rewiredInputManager if that's what we wanted, and transition to the next scene
        rewiredManager.SetActive(disableInputWhileTransitioning);

        // Play animation based on type we gave
        switch (animType)
        {
            case AnimationType.forward:
                StopAllCoroutines();
                StartCoroutine(PlaySwipeInFromLeft(sceneName));
                animToPlayOnNextLoad = AnimationType.forward;
                break;
            case AnimationType.backward:
                swipeImage.GetComponent<Animator>().Play("SwipeInFromRight");
                break;
        }        
    }

    private void PlaySceneEnterAnimation(Scene scene, LoadSceneMode mode)
    {
        switch(animToPlayOnNextLoad)
        {
            case AnimationType.forward:
                StopAllCoroutines();
                StartCoroutine(PlaySwipeOutToRight());
                break;
            case AnimationType.backward: // Still needs to be implemented
                break;
            default:
                if(swipeImage)
                swipeImage.enabled = false;
                break;
        }
    }


    private IEnumerator PlaySwipeInFromLeft(string SceneName)
    {
        // Place the swipeImage to the very left of the canvas, then animate it back toward the center

        rewiredManager.SetActive(disableInputWhileTransitioning);
        swipeImage.rectTransform.anchoredPosition = new Vector2(-swipeImage.GetComponentInParent<RectTransform>().rect.width, 0);
        swipeImage.enabled = true;

        while (swipeImage.rectTransform.anchoredPosition.x < 0)
        {
            // Move the image close to the center
            swipeImage.rectTransform.anchoredPosition = new Vector2(swipeImage.rectTransform.anchoredPosition.x + swipeDuration * Time.deltaTime, 0);

            // Make sure the image doesn't overshoot for a frame
            if(swipeImage.rectTransform.anchoredPosition.x >= 0)
            {
                swipeImage.rectTransform.anchoredPosition = new Vector2(0, 0);
            }
            yield return null;
        }

        // Load up the scene once the animation is done.
        SceneManager.LoadScene(SceneName);
    }

    private IEnumerator PlaySwipeOutToRight()
    {
        // Place the swipeImage to the very left of the canvas, then animate it back toward the center
        swipeImage.rectTransform.anchoredPosition = new Vector2(0, 0);

        while (swipeImage.rectTransform.anchoredPosition.x < swipeImage.GetComponentInParent<RectTransform>().rect.width)
        {
            // Move the image close to the center
            swipeImage.rectTransform.anchoredPosition = new Vector2(swipeImage.rectTransform.anchoredPosition.x + swipeDuration * Time.deltaTime, 0);

            // Make sure the image doesn't overshoot for a frame
            if (swipeImage.rectTransform.anchoredPosition.x >= swipeImage.GetComponentInParent<RectTransform>().rect.width)
            {
                swipeImage.rectTransform.anchoredPosition = new Vector2(swipeImage.GetComponentInParent<RectTransform>().rect.width, 0);
            }
            yield return null;
        }

        EnableRewiredInput();
        swipeImage.enabled = false;
    }

    // Functions to disable all controls when transitioning, just in case it's possible 
    // something weird could happen if someone starts pressing buttons while the transition happens
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
