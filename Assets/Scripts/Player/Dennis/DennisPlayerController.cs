using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DennisPlayerController : PlayerControl {

    // Player's rigidbody that will be used for setting velocity
    public GunController theGun;

    // Keep track of our current Playerstate
    private PlayerState currentState;

    // Reticle that will rotate to show aiming direction
    public GameObject aimReticle;

    // A statemachine made for dennisStates
    private StateMachine<DennisStates.DennisState> sMachine;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        sMachine.UpdateState();
	}

    public void UpdateAttack()
    {

    }

    public void UpdateReticleRotation()
    {
        // Return if the player isn't, well, playing
        if (!player.isPlaying)
        {
            return;
        }

        // If using keyboard, get the mouse position for aiming, otherwise use controller axis
        if (player.controllers.hasKeyboard)
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

            if (Mathf.Abs(axis.x) > 0.3 || Mathf.Abs(axis.y) > 0.3)
            {
                Vector2 totalPos = new Vector2(this.gameObject.transform.position.x + axis.x, this.gameObject.transform.position.y + axis.y);

                float aimAngle = Mathf.Atan2((this.gameObject.transform.position.y - totalPos.y),
                    (this.gameObject.transform.position.x - totalPos.x)) * Mathf.Rad2Deg;


                aimReticle.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            }
        }
    }
}
