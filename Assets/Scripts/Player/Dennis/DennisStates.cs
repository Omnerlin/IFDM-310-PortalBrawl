using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DennisPlayerController : PlayerControl
{
    // Extended class of PlayerState that has a reference to the DennisPlayerController
    public abstract class DennisState : PlayerState 
	{
        public DennisPlayerController pControl; public DennisState(DennisPlayerController cont) {pControl = cont;}
    }

    // Class that will be execute when dennis can roam freely
    public class WalkState : DennisState
    {
        public WalkState(DennisPlayerController cont) : base(cont){}

        public override void OnEnter(){}
        public override void OnExit(){}

        public override PlayerState Update()
        {
            if(pControl.GetComponent<Player>().isDead())
            {
                return new DeathState(pControl);
            }

            pControl.Interact();
            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();
            pControl.UpdatePlayerMovement();
            
            return this;
        }
    }

    public class DeathState : DennisState
    {
        public DeathState(DennisPlayerController cont) : base(cont) { }

        public override void OnEnter()
        {
            pControl.animator.SetTrigger("Die");
            pControl.deathVisuals.SetActive(true);
            pControl.rb2d.velocity = Vector2.zero;
            pControl.rb2d.isKinematic = true;
            pControl.aimReticle.SetActive(false);

            pControl.GetComponent<PlayerInteractionManager>().interactionCollider.enabled = false;

        }
        public override void OnExit()
        {
            pControl.rb2d.isKinematic = false;
            pControl.deathVisuals.SetActive(false);
            pControl.animator.SetTrigger("Revive");
            pControl.aimReticle.SetActive(true);

            pControl.GetComponent<PlayerInteractionManager>().interactionCollider.enabled = true;
            // Let's give them invulnerability when they are rezed
            pControl.GetComponent<Player>().hurtPlayer(0);
        }

        public override PlayerState Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                pControl.GetComponent<Player>().setMaxHP();
                return new WalkState(pControl);
            }

            if (!pControl.GetComponent<Player>().isDead())
            {
                return new WalkState(pControl);
            }

            return this;
        }
    }
}