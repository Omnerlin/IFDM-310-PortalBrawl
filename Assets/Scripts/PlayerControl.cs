using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerControl : MonoBehaviour {

    // Rewired player object
    public Rewired.Player player { get; set; }
	public Player playerInfo;

    // Stats that will affect the player movespeed
    public float maxMoveSpeed;
    public float moveForce;

    // Number of the player's controller (Used to check for separate input)
    public int controllerNumber { get; set; }

    // Reticle that will rotate to show aiming direction
    public GameObject aimReticle;

    // Player's rigidbody that will be used for setting velocity
    private Rigidbody2D rb2d;

    public GunController theGun;

    private void Awake()
    {
        // Just set the player to the zero index
        player = Rewired.ReInput.players.GetPlayer(0);
    }

    // Use this for initialization
    void Start () 
	{
		playerInfo = gameObject.GetComponent<Player>();
		playerInfo.setPlayerNumber (player.id);
        rb2d = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        UpdateReticleRotation(controllerNumber);
        if (player.GetButton("RightBumper"))
        {
            theGun.isFiring = true;
        }
        else
        {
            theGun.isFiring = false;
        }
    }

    private void FixedUpdate()
    {
        UpdatePlayerMovement();
    }

    private void UpdateReticleRotation(int controllerNumber)
    {
        // Return if the player isn't, well, playing
        if (!player.isPlaying)
        {
            return;
        }

        // If using keyboard, get the mouse position for aiming, otherwise use controller axis
        if(player.controllers.hasKeyboard)
        {
            // Get the position of the mouse in screen coordinates, and convert it to world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // We'll use this to decide what direction that the player is aiming
            float aimAngle = Mathf.Atan2((transform.position.y - mousePos.y), (transform.position.x - mousePos.x)) * Mathf.Rad2Deg;
            aimReticle.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        }
        else
        {
            // Get reticle rotation based on controller analog
            Vector2 axis = new Vector2(player.GetAxis("Rotate Horizontal"), 
                player.GetAxis("Rotate Vertical"));

            if(Mathf.Abs(axis.x) > 0.3 || Mathf.Abs(axis.y) > 0.3)
            {
                Vector2 totalPos = new Vector2(this.gameObject.transform.position.x + axis.x, this.gameObject.transform.position.y + axis.y);

                float aimAngle = Mathf.Atan2((this.gameObject.transform.position.y - totalPos.y), 
                    (this.gameObject.transform.position.x - totalPos.x)) * Mathf.Rad2Deg;

                
                aimReticle.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            }
        }
    }

    private void UpdatePlayerMovement()
    {
        // Return if the player isn't, well, playing
        if (!player.isPlaying)
        {
            return;
        }

        // Update the player's movespeed based on input axis (-1 to 1) and normalize it (for diagonal movement)
        Vector2 moveInput = new Vector2(player.GetAxis("Move Horizontal"), player.GetAxis("Move Vertical"));
        if(moveInput.magnitude > 1)
        {
            moveInput = moveInput.normalized;
        }

        // Immediately set the player's velocity based on the normalized input
        rb2d.velocity = new Vector2(maxMoveSpeed * moveInput.x, maxMoveSpeed * moveInput.y);


        // These if statements aren't really necessary at this point (Since we're using normalized input)
        if (Mathf.Abs(rb2d.velocity.x) > maxMoveSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxMoveSpeed, rb2d.velocity.y);
        }
        if (Mathf.Abs(rb2d.velocity.y) > maxMoveSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxMoveSpeed);
        }
    }
}
