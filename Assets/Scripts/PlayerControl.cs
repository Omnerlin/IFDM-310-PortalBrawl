using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public GameObject TestCrap;

    // Stats that will affect the player movespeed
    public float maxMoveSpeed;
    public float moveForce;

    // Reticle that will rotate to show aiming direction
    public GameObject aimReticle;

    // Player's rigidbody that will be used for setting velocity
    private Rigidbody2D rb2d;

    // Number of the player's controller (Used to check for separate input)
    public int controllerNumber { get; set; }

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        UpdateReticleRotation(controllerNumber);
    }

    private void FixedUpdate()
    {
        // Don't process input if the controller number isn't set
        if(controllerNumber == 0)
        {
            return;
        }

        // Update the player's movespeed based on input axis (-1 to 1) and normalize it (for diagonal movement)
        Vector2 moveInput = new Vector2(Input.GetAxis("J" + controllerNumber + "LHorizontal"), Input.GetAxis("J" + controllerNumber + "LVertical")).normalized;


        // Immediately set the player's velocity based on the normalized input
        rb2d.velocity = new Vector2(maxMoveSpeed * moveInput.x, maxMoveSpeed * moveInput.y);

        
        // These if statements aren't really necessary at this point (Since we're using normalized input)
        if(Mathf.Abs(rb2d.velocity.x) > maxMoveSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxMoveSpeed, rb2d.velocity.y);
        }
        if (Mathf.Abs(rb2d.velocity.y) > maxMoveSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxMoveSpeed);
        }
    }

    private void UpdateReticleRotation(int controllerNumber)
    {
        if(controllerNumber == 5)
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
            Vector2 axis = new Vector2(Input.GetAxis("J" + controllerNumber + "RHorizontal"), 
                Input.GetAxis("J" + controllerNumber + "RVertical")).normalized;

            if(axis.x != 0 && axis.y != 0)
            {
                Vector2 totalPos = new Vector2(this.gameObject.transform.position.x + axis.x, this.gameObject.transform.position.y + axis.y);

                float aimAngle = Mathf.Atan2((this.gameObject.transform.position.y - totalPos.y), 
                    (this.gameObject.transform.position.x - totalPos.x)) * Mathf.Rad2Deg;

                aimReticle.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            }

        }
    }
}
