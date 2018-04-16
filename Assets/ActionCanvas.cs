using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCanvas : MonoBehaviour {

    public Text actionText;
    public Image actionImage;

    public Sprite keyboardAction;
    public Sprite playstationAction;
    public Sprite xboxAction;

	// Use this for initialization
	void Start () {
        UpdateActionIcon();
	}


    // Updates what image should be used for the action based on the controller type
    public void UpdateActionIcon()
    {
        Rewired.Player player = GetComponentInParent<PlayerControl>().player;
        
        if(player.controllers.hasKeyboard)
        {
            actionImage.sprite = keyboardAction;
            return;
        }
        
        switch(player.controllers.GetLastActiveController().name)
        {
            case "Sony DualShock 4":
                actionImage.sprite = playstationAction;
                break;
            default:
                actionImage.sprite = xboxAction;
                break;
        }
    }
}
