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

    public void Awake()
    {
        player = ReInput.players.GetPlayer(4);
        totalOptions = menuButtons.Count - 1;
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
            }
        }

        if (player.GetButton("Start"))
        {
            menuButtons[index].onClick.Invoke();
        }
    }
}
