
public class PlayerInfo {

	public string characterName = "";
	public int playerNumber = 1;
    public int controllerID = 0;
	public int health = 100;
	//Health, etc

	public string toString()
	{
		return "PlayerNumber: " + playerNumber + "ControllerID " + controllerID + "\tCharacterName: " + characterName + "\tHealth: " + health;
	}
}
