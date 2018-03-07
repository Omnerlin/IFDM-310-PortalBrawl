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
public class CursorControl : MonoBehaviour {

	// Rewired player object
	public Rewired.Player rewiredPlayer { get; set; }

	private Player playerScript;

	// Stats that will affect the player movespeed
	public float movementScale;

	// Number of the player's controller (Used to check for separate input)
	public int controllerNumber { get; set; }

	private Transform myTransform;
	private Collider2D myCollider;

	//Used to constrain the cursor's movement to inside the camera
	private Rect cameraRect;

	private void Awake()
	{
		// Just set the player to the zero index
		rewiredPlayer = Rewired.ReInput.players.GetPlayer(0);
		playerScript = gameObject.AddComponent<Player>();
	}

	// Use this for initialization
	void Start () 
	{
		myTransform = GetComponent<Transform>();
		myCollider = GetComponent<Collider2D>();

		//Every controller should load player info on start?
		//playerScript = this.gameObject.GetComponent<Player>();
		playerScript.setPlayerNumber(rewiredPlayer.id);
		GetComponent<SpriteRenderer>().color = playerScript.getColor();

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
            //If the player has already selected someone, return.


            Collider2D[] results = new Collider2D[1];
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.NoFilter();
            int numColliding = myCollider.OverlapCollider(contactFilter, results);
            if (numColliding > 0)
            {
                GameObject selected = (results[0]).gameObject;
                Debug.Log("Player " + playerScript.getPlayerNumber() + " tried selecting " + selected.name + "!");
                //Run a script on the button that says you've selected it so it changes its display
                if (selected.GetComponent<characterButtonScript>() != null &&
                    !selected.GetComponent<characterButtonScript>().selected) //If this character hasn't already been selected **
                {
                    // We want the player to be able to switch characters after they've already selected, so see if they already have a name assigned.
                    // We should probably offer a "back" button to undo character select. Otherwise, nobody could change their character if everyone
                    // has picked one, ya see?
                    if(playerScript.getCharacterName() == selected.name)
                    {
                        Debug.Log("Player " + playerScript.getPlayerNumber() + "already selected " + selected.name);
                        return;
                    }
                    if (playerScript.getCharacterName() != "")
                    {
                        GameObject oldSelection = GameObject.Find(playerScript.getCharacterName());

                        if (!oldSelection)
                        {
                            Debug.Log("uhhh.... what? Name must have been changed.");
                            return;
                        }

                        // Set our old selection back to normal, and assign our player to the new selection
                        oldSelection.GetComponent<characterButtonScript>().selected = false;
                        oldSelection.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    Debug.Log("Assigning " + selected.name + " to Player " + rewiredPlayer.id + ".");
                    string name = selected.name;
                    playerScript.setCharacterName(name);
                    selected.GetComponent<SpriteRenderer>().color = playerScript.getColor();
                    selected.GetComponent<characterButtonScript>().selected = true;
                }
                //If it wasn't a character button, it must be the LoadScene button
                else if (selected.GetComponent<LoadScene>() != null)
                    selected.GetComponent<LoadScene>().loadScene();
            }
        }

        //TODO: add deselection button
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
