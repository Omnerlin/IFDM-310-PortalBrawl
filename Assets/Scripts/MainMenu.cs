using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Rewired;


public class MainMenu : MonoBehaviour {
    public int index = 0;
    public int totalOptions;
    public float yOffset;

    public Rewired.Player player { get; set;}

    //public bool isStart;

    public void Awake()
    {
        player = ReInput.players.GetPlayer(4);

    }

    private void OnMouseUp()

    {

        /*if (isStart)

        {

            SceneManager.LoadScene("TestScene");

        }

        if (!isStart)

        {

            Application.Quit();

        }*/

    }

    // Use this for initialization
    void Start ()
    {
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        if (player.GetButtonDown("DPadDown"))
        {
            if (index <= (totalOptions - 1))
            {
                index++;
                Vector2 postion = transform.position;
                postion.y -= yOffset;
                transform.position = postion;
            }
        }
        if (player.GetButtonDown("DPadUp"))
        {
            if ((index > 0))
            {
                index--;
                Vector2 postion = transform.position;
                postion.y += yOffset;
                transform.position = postion;
            }
        }

        if (player.GetButton("Start"))
        {
            if(index == 0)
            {
                SceneTransitionManager.Instance.TransitionToScene("CharacterSelection", SceneTransitionManager.AnimationType.forward);
            }
            if(index == 1)
            {
                Debug.Log("Load game was selected, but is not available yet");
            }
            if (index == 2)
            {
                Debug.Log("Options were selected, but is not available yet");
            }
            if (index == 3)
            {
                Application.Quit();
            }
        }
    }
}
