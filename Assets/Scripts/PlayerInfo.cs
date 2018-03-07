
public class PlayerInfo {

	public string characterName = "";
	public int playerNumber=1;
	public int health = 100;
	//Health, etc

	public string toString()
	{
		return "PlayerNumber: " + playerNumber + "\tCharacterName: " + characterName + "\tHealth: " + health;
	}
}
