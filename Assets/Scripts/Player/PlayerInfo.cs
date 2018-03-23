﻿
public class PlayerInfo {

	public string characterName = "";
	public int playerNumber = 1;
    public int controllerID = 0;
	public int currentHealth = -1; //-1 on initialization so it becomes maxHealth at the VERY beginning
	//Health, etc

	public string toString()
	{
		return "PlayerNumber: " + playerNumber + "ControllerID " + controllerID + "\tCharacterName: " + characterName + "\tHealth: " + currentHealth;
	}
}
