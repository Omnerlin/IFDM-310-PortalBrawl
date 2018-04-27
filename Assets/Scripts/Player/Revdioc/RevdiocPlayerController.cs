using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class RevdiocPlayerController : PlayerControl {

    // Reticle that will rotate to show aiming direction
    public GameObject hammer;

    // Object holding visual components of hammer
    public GameObject hammerVisuals;

    // HitBox gameobject to use for telling whether or not we've hit an enemy
    public GameObject hammerHitBox;

    // Force that enemies will be hit with
    public float hitForce = 10;

    // Cooldown for melee attacking
    public float attackCooldown = 1f;
    public float hitboxTimeActive = 0.2f;
    public ContactFilter2D hitboxFilter;

    //Weapon/Revive Sounds
    public AudioSource[] playerSounds;

    // A statemachine made for revdiocStates
    private StateMachine<RevdiocState> sMachine;

    private Camera mainCamera;
    private Camera pixelCam;

    // Used in UpdateReticleRotation
    private float previousRotation;

    private void Awake()
    {
        animator = characterBody.GetComponent<Animator>();

        sMachine = new StateMachine<RevdiocState>(new WalkState(this, playerSounds));

        pixelCam = GameObject.FindGameObjectWithTag("PixelCam").GetComponent<Camera>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        sMachine.UpdateState();
    }

    private void UpdateAttack()
    {
    }

    private void UpdateReticleRotation()
    {
        bool isAiming = true;
        float aimAngle = 0;

        // Return if the player isn't, well, playing
        if (!player.isPlaying)
        {
            return;
        }

        // If using keyboard, get the mouse position for aiming, otherwise use controller axis
        if (player.controllers.hasKeyboard)
        {
            if(player.GetButtonDown("RightBumper"))
            {
                Debug.Log("pressed the bumper");
            }

            isAiming = true;

            // We'll use this to decide what direction that the player is aiming
            Vector3 mousePosf = pixelCam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 mousePos = mainCamera.ViewportToWorldPoint(mousePosf);
            aimAngle = Mathf.Atan2((hammer.transform.position.y - mousePos.y), (hammer.transform.position.x - mousePos.x)) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            hammer.transform.rotation = q;
        }
        else
        {
            // Get reticle rotation based on controller analog
            Vector2 axis = new Vector2(player.GetAxis("Rotate Horizontal"),
                player.GetAxis("Rotate Vertical"));

            // Set up a deadzone for controllers
            if (Mathf.Abs(axis.magnitude) <= 0.3)
            {
                axis = new Vector2(animator.GetFloat("MoveX"),
                animator.GetFloat("MoveY"));
                if (axis.magnitude > 1) { axis = axis.normalized; }
                isAiming = false;

                if (Mathf.Abs(axis.magnitude) > 0)
                {
                    Vector2 totalPos = new Vector2(this.gameObject.transform.position.x + axis.x, this.gameObject.transform.position.y + axis.y);

                    aimAngle = Mathf.Atan2((this.gameObject.transform.position.y - totalPos.y),
                        (this.gameObject.transform.position.x - totalPos.x)) * Mathf.Rad2Deg;

                    Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
                    hammer.transform.rotation = q;
                }
                else
                {
                    aimAngle = previousRotation;
                }
            }
            else
            {
                Vector2 totalPos = new Vector2(this.gameObject.transform.position.x + axis.x, this.gameObject.transform.position.y + axis.y);

                aimAngle = Mathf.Atan2((this.gameObject.transform.position.y - totalPos.y),
                    (this.gameObject.transform.position.x - totalPos.x)) * Mathf.Rad2Deg;

                Quaternion q = Quaternion.AngleAxis(aimAngle, Vector3.forward);
                hammer.transform.rotation = q;
            }
        }

        // Set the direction of the player's animator based on their aim direction.

        if (aimAngle < -110 || aimAngle > 110)
        {
            animator.SetFloat("AimDirection", 1);
            animator.SetFloat("DirectionX", 1);
        }
        else if (aimAngle > -70 && aimAngle < 70)
        {
            animator.SetFloat("AimDirection", -1);
            animator.SetFloat("DirectionX", -1);
        }


        // Decide whether or not the gun should render behind the player based on the angle 
        //if (aimAngle < -15 && aimAngle > -165)
        //{
        //    hammer.GetComponent<SortingGroup>().sortingOrder = -1;
        //}
        //else
        //{
        //    hammer.GetComponent<SortingGroup>().sortingOrder = 0;
        //}

        animator.SetBool("isAiming", isAiming);
        previousRotation = aimAngle;
    }

	public void attemptToRevive()
	{
		//Get walkbox collider for this character
		ContactFilter2D filter = new ContactFilter2D ();
		filter.NoFilter ();
		GameObject walkbox = gameObject.GetComponent<Transform>().Find("Walkbox").gameObject;
		if(walkbox == null) 
		{
			Debug.LogWarning("Was not able to find "+name+"'s walkbox");
			return;
		}


		Collider2D[] collideWithMe = new Collider2D[20];
		//If the player's collider is overlapping with another player's collider who is dead
		Physics2D.OverlapCollider(walkbox.GetComponent<BoxCollider2D> (), filter, collideWithMe);
		foreach (Collider2D col in collideWithMe)
		{
			if (col != null && col.gameObject.name == "ReviveBox") 
			{
				Player otherPlayer = col.gameObject.transform.parent.transform.parent.GetComponent<Player> (); 
				GetComponent<Player> ().reviveOtherPlayer (otherPlayer);
			}

		}
	}
}
