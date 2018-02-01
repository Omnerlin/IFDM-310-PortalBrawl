using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private List<int> assignedControllers = new List<int>();
    public GameObject playerPrefab;

    private void Update()
    {

        if (Input.GetButton("joystick 1 button 0"))
        {
            Debug.Log("joystick 1 pressed");
        }


        for (int i = 1; i < 6; i++)
        {
            if(Input.GetButtonDown("J" + i + "Fire2"))
            {
                if(!assignedControllers.Contains(i))
                {
                    AddControllerAndPlayer(i);
                    break;

                }
            }
        }
    }

    // Add a controller to the list of initialized controls
    private void AddControllerAndPlayer(int ControlID)
    {
        assignedControllers.Add(ControlID);
        GameObject player = Instantiate(playerPrefab);
        playerPrefab.GetComponent<Player>().playerNumber = assignedControllers.Count;
        player.GetComponent<PlayerControl>().controllerNumber = ControlID;
        Debug.Log("Creating Player " + assignedControllers.Count + " using controller ID " + ControlID);
    }

}
