using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public int index = 0;
    public float yOffset;
    public float xOffset;

    public enum NavigationType { TopToBottom, LeftToRight };

    public NavigationType navigationType;

    public IList<Rewired.Player> players { get; set;}

    public List<Button> menuButtons;

    private int totalOptions;

    public AudioSource[] sounds = new AudioSource[2];

    private void Awake()
    {
        players = ReInput.players.GetPlayers(false);
        totalOptions = menuButtons.Count - 1;
    }

    private void Start()
    {
        menuButtons[index].Select();
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        foreach (Rewired.Player player in players)
        {
            if (player.GetButtonDown(navigationType == NavigationType.TopToBottom ? "DPadUp" : "DPadLeft"))
            {
                if (index > 0)
                {
                    index--;
                    menuButtons[index].Select();
                    Vector2 postion = transform.position;
                    postion.y += yOffset;
                    postion.x -= xOffset;
                    transform.position = postion;
                    sounds[0].Play();
                }
            }
            if (player.GetButtonDown(navigationType == NavigationType.TopToBottom ? "DPadDown" : "DPadRight"))
            {
                if ((index < totalOptions))
                {
                    index++;
                    menuButtons[index].Select();
                    Vector2 postion = transform.position;
                    postion.y -= yOffset;
                    postion.x += xOffset;
                    transform.position = postion;
                    sounds[0].Play();
                }
            }

            if (player.GetButtonDown("Start") || player.GetButtonDown("XButton"))
            {
                menuButtons[index].onClick.Invoke();
                sounds[1].Play();
            }
        }
    }
}
