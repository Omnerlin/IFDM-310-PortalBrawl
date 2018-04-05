using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/* Created by Anna Carey based on PlayerControl. Should be stored
 * on the Cursor prefab object.
 * Saves the camera's frame and prevents the cursor from
 * moving outside that frame. Also stores the Rewired.Player object
 * and responds to the player's button presses.
 * If the cursor is over another box with a collider, it will assign
 * that collider's name to playerInfo.characterName. */

// Alex's notes:
// Sorry for kinda making things all different and stuff. 
// I know that it's weird that we're saving data from the cursors, but
// I think that it should be fine since we're only using these cursors for 
// chararcter select. I think Player scripts should just be put on players
// to avoid having to manage two scripts conflicting with each other, ya know?

// We can just have the player data be saved once the start button is pressed since we 
// to the next scene we're going to the gameplay state from there.

// These seems reasonable since we're getting all of our input and stuff from this object anyways.
// I think it'll be easier to manage just having interactions between this, cursormanager and the global control.
public class CursorControl : MonoBehaviour {

    // Let's just store the player's information here.
    [HideInInspector] public PlayerInfo info = new PlayerInfo();

	// Rewired player object
	public Rewired.Player rewiredPlayer { get; set; }

	// Stats that will affect the player movespeed
	public float movementScale;

	private Transform myTransform;
	private Collider2D myCollider;

    //Used to constrain the cursor's movement to inside the camera
    private Rect cameraRect;

    // Color that we're going to use for the player
    public Color playerColor;

    // Variables for accessing the data that we want to save for this player.
    public int playerNumber
    {
        get { return info.playerNumber; }
        set { info.playerNumber = value; }
    }

    public int controllerID
    {
        get { return info.controllerID; }
        set { info.controllerID = value; }
    }

    public int health
    {
        get { return info.currentHealth; }
        set { info.currentHealth = value; }
    }

    public string characterName
    {
        get { return info.characterName; }
        set { info.characterName = value; }
    }


    public void SaveData()
    {
        GlobalControl.instance.SaveData(info.playerNumber, info);
    }

	private void Awake()
	{
		// Just set the player to the zero index
		rewiredPlayer = Rewired.ReInput.players.GetPlayer(0);
	}

	// Use this for initialization
	void Start () 
	{
		myTransform = GetComponent<Transform>();
		myCollider = GetComponent<Collider2D>();

		//Setup the cameraRect bounds.
		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		Vector3 bottomLeft = camera.ScreenToWorldPoint(Vector3.zero);
		Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(
			camera.pixelWidth, camera.pixelHeight));

		cameraRect = new Rect (bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
	}

    // Update is called once per frame
    void Update()
    {
        if (rewiredPlayer == null)
        {
            return;
        }

        //Check if cursor is selecting a button. If this collider is colliding with another, get that
        //collider's button, assign its name to this character, and set that button to "selected."
        if (rewiredPlayer.GetButtonDown("Select"))
        {
            Collider2D[] results = new Collider2D[1];
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.NoFilter();
            int numColliding = myCollider.OverlapCollider(contactFilter, results);
            if (numColliding > 0)
            {
                GameObject selected = (results[0]).gameObject;
                Debug.Log("Player " + info.playerNumber + " tried selecting " + selected.name + "!");
                //Run a script on the button that says you've selected it so it changes its display
                if (selected.GetComponent<characterButtonScript>() != null &&
                    !selected.GetComponent<characterButtonScript>().selected) //If this character hasn't already been selected **
                {
                    // TODO: We want the player to be able to switch characters after they've already selected, so see if they already have a name assigned.
                    // We should probably offer a "back" button to undo character select. Otherwise, nobody could change their character if everyone
                    // has picked one, ya see?
                    if(characterName == selected.name)
                    {
                        Debug.Log("Player " + playerNumber + "already selected " + selected.name);
                        return;
                    }
                    if (characterName != "")
                    {
                        // Set our old selection back to normal, and assign our player to the new selection
                        GameObject oldSelection = GameObject.Find(characterName);

                        // Make sure that we can find our old selection based off of name.... If we can't
                        // something went wrong. Maybe changed the name of the selection box or the player.
                        if (!oldSelection)
                        {
                            Debug.Log("uhhh.... what? Name must have been changed.");
                            return;
                        }

						oldSelection.GetComponent<characterButtonScript>().deselect();
                    }

                    Debug.Log("Assigning " + selected.name + " to Player " + playerNumber + ".");
					characterName = selected.name;
					selected.GetComponent<characterButtonScript>().select(myCollider);
                }
                //If it wasn't a character button, it must be the LoadScene button
                else if (selected.GetComponent<LoadScene>() != null)
                {
                    selected.GetComponent<LoadScene>().loadScene();
                }
            }
        }

		if (rewiredPlayer.GetButtonDown ("Back")) 
		{
			GameObject oldSelection = GameObject.Find(characterName);
			Debug.Log ("Player " + playerNumber + " deselected " + oldSelection.name + ".");
			oldSelection.GetComponent<characterButtonScript>().deselect();
			characterName = "";
		}

    }

    private void OnDestroy()
    {
        SaveData();
    }

    private void FixedUpdate()
	{
		UpdatePlayerMovement();
	}

	private void UpdatePlayerMovement()
	{
		// Return if the player isn't, well, playing
		if (!rewiredPlayer.isPlaying)
		{
			return;
		}

		//Constrain the movement to within the camera's bounds
		myTransform.position = new Vector3 (
			Mathf.Clamp (myTransform.position.x, cameraRect.xMin, cameraRect.xMax),
			Mathf.Clamp (myTransform.position.y, cameraRect.yMin, cameraRect.yMax),
			myTransform.position.z);

		// Update the player's movespeed based on input axis (-1 to 1) and normalize it (for diagonal movement)
		Vector2 moveInput = new Vector2(rewiredPlayer.GetAxis("Move Horizontal"), rewiredPlayer.GetAxis("Move Vertical"));
		if(moveInput.magnitude > 1)
		{
			moveInput = moveInput.normalized;
		}

		// Immediately set the cursor velocity based on the normalized input
		myTransform.Translate(moveInput.x*movementScale, moveInput.y*movementScale, 0);
	}
}
