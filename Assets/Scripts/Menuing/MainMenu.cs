using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public int index = 0;
    public float yOffset;

    public Rewired.Player player { get; set;}

    public List<Button> menuButtons;

    private int totalOptions;

    public AudioSource[] sounds = new AudioSource[2];

    private void Awake()
    {
        player = ReInput.players.GetPlayer(4);
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
        if (player.GetButtonDown("DPadUp"))
        {
            if (index > 0)
            {
                index--;
                menuButtons[index].Select();
                Vector2 postion = transform.position;
                postion.y += yOffset;
                transform.position = postion;
                sounds[0].Play();
            }
        }
        if (player.GetButtonDown("DPadDown"))
        {
            if ((index < totalOptions))
            {
                index++;
                menuButtons[index].Select();
                Vector2 postion = transform.position;
                postion.y -= yOffset;
                transform.position = postion;
                sounds[0].Play();
            }
        }

        if (player.GetButton("Start"))
        {
            menuButtons[index].onClick.Invoke();
            sounds[1].Play();
        }
    }
}
