using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionInfo : MonoBehaviour {

    [HideInInspector] public PlayerInfo info = new PlayerInfo();

    private int playerNumber;
    private int controllerID;
    private string charaterName;



	public void UpdateSaveData()
    {
        GlobalControl.instance.SaveData(1, info);
    }
}
