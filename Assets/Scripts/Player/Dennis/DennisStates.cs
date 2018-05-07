using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DennisPlayerController : PlayerControl
{
    // Extended class of PlayerState that has a reference to the DennisPlayerController
    public abstract class DennisState : PlayerState 
	{
        public DennisPlayerController pControl;
        public AudioSource reviveSound;
        public DennisState(DennisPlayerController cont,AudioSource sound) {
            pControl = cont;
            reviveSound = sound;
        }
    }

    // Class that will be execute when dennis can roam freely
    public class WalkState : DennisState
    {
        public WalkState(DennisPlayerController cont, AudioSource sound) : base(cont, sound){}

        public override void OnEnter(){}
        public override void OnExit(){}

        public override PlayerState Update()
        {
            if(pControl.GetComponent<Player>().isDead())
            {
                return new DeathState(pControl,reviveSound);
            }


            if (pControl.player.GetButtonDown("TriangleButton"))
            {
                //(We don't need another state for this because it's a rather quick thing.)
                pControl.attemptToRevive();
            }

            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();
            pControl.UpdatePlayerMovement();
            
            return this;
        }
    }

    public class DeathState : DennisState
    {
        public DeathState(DennisPlayerController cont, AudioSource sound) : base(cont,sound) { }

        public override void OnEnter()
        {
            PlayerManager.instance.CheckForGameOver();
            MessageBroadcaster.Instance.BroadcastAnnouncement("Dennis is Down!");

            pControl.animator.SetTrigger("Die");
            pControl.deathVisuals.SetActive(true);
            pControl.rb2d.velocity = Vector2.zero;
            pControl.rb2d.isKinematic = true;
            pControl.aimReticle.SetActive(false);

        }
        public override void OnExit()
        {
            pControl.rb2d.isKinematic = false;
            pControl.deathVisuals.SetActive(false);
            pControl.animator.SetTrigger("Revive");
            pControl.aimReticle.SetActive(true);
            reviveSound.Play();
        }

        public override PlayerState Update()
        {
            // Revive for debugging
            if(Input.GetKeyDown(KeyCode.R))
            {
                pControl.GetComponent<Player>().setMaxHP();
                return new WalkState(pControl,reviveSound);
            }
			if (!pControl.GetComponent<Player> ().isDead ()) //Someone gave them health by reviving them or otherwise healing them
			{ 
				return new WalkState(pControl,reviveSound);
			}

            return this;
        }
    }
}