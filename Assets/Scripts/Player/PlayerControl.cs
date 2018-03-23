using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerControl : MonoBehaviour
{
    // Rewired player object
    public Rewired.Player player { get; set; }

    // Info about the player that will be saved to the global store
    [HideInInspector] public Player playerInfo;

    // Stats that will affect the player movespeed
    public float maxMoveSpeed;
    public float moveForce;

    // Number of the player's controller (Used to check for separate input)
    public int controllerNumber { get; set; }

    protected Rigidbody2D rb2d;


    private void Awake()
    {
        // Just set the player to the zero index
        player = Rewired.ReInput.players.GetPlayer(0);
    }

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInfo = gameObject.GetComponent<Player>();
        playerInfo.setPlayerNumber(player.id);
    }

    private void FixedUpdate()
    {
        Animator animator = GetComponent<Animator>();

        animator.SetFloat("MoveX", rb2d.velocity.x);
        animator.SetFloat("MoveY", rb2d.velocity.y);

        if (Mathf.Abs(rb2d.velocity.sqrMagnitude) > 0.01)
        {
            if (rb2d.velocity.x < 0)
            {
                animator.SetFloat("DirectionX", -1);
            }
            else if (rb2d.velocity.x > 0)
            {
                animator.SetFloat("DirectionX", 1);
            }


            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    protected void UpdatePlayerMovement()
    {
        // Return if the player isn't, well, playing
        if (!player.isPlaying)
        {
            return;
        }

        // Update the player's movespeed based on input axis (-1 to 1) and normalize it (for diagonal movement)
        Vector2 moveInput = new Vector2(player.GetAxis("Move Horizontal"), player.GetAxis("Move Vertical"));
        if (moveInput.magnitude > 1)
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
