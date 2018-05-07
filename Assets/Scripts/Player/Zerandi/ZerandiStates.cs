using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public partial class ZerandiPlayerController : PlayerControl
{
    // Extended class of PlayerState that has a reference to the DennisPlayerController
    public abstract class ZerandiState : PlayerState
    {
        public ZerandiPlayerController pControl;
        public AudioSource reviveSound;
        public ZerandiState(ZerandiPlayerController cont,AudioSource sound)
        {
            pControl = cont;
            reviveSound = sound;
        }
    }

    // Class that will be execute when Zerandi can roam freely
    public class WalkState : ZerandiState
    {
        public WalkState(ZerandiPlayerController cont, AudioSource sound) : base(cont, sound) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        

        public override PlayerState Update()
        {
            if(pControl.GetComponent<Player>().isDead())
            {
                return new DeathState(pControl, reviveSound);
            }

			if(pControl.player.GetButtonDown("TriangleButton"))
			{
                //(We don't need another state for this because it's a rather quick thing.)
				pControl.attemptToRevive ();
			}

            pControl.UpdateAttack();
            pControl.UpdateReticleOrientation();
            pControl.UpdatePlayerMovement();

            return this;
        }
    }

    public class StunState : ZerandiState
    {
        public StunState(ZerandiPlayerController cont, AudioSource sound) : base(cont, sound) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState Update()
        {
            pControl.UpdateAttack();
            pControl.UpdateReticleOrientation();

            return this;
        }
    }

    public class DeathState : ZerandiState
    {
        public DeathState(ZerandiPlayerController cont, AudioSource sound) : base(cont, sound) { }

        public override void OnEnter()
        {
            PlayerManager.instance.CheckForGameOver();
            MessageBroadcaster.Instance.BroadcastAnnouncement("Zerandi is Down!");

            pControl.animator.SetTrigger("Die");
            pControl.deathVisuals.SetActive(true);
            pControl.rb2d.isKinematic = true;
            pControl.rb2d.velocity = Vector2.zero;
        }

        public override void OnExit()
        {
            pControl.animator.SetTrigger("Revive");
            pControl.deathVisuals.SetActive(false);
            pControl.rb2d.isKinematic = false;
            pControl.rb2d.velocity = Vector2.zero;
            reviveSound.Play();
        }

        public override PlayerState Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                pControl.GetComponent<Player>().setMaxHP();
                return new WalkState(pControl, reviveSound);
            }
			if (!pControl.GetComponent<Player> ().isDead ()) //Someone gave them health by reviving them or otherwise healing them
			{ 
				return new WalkState(pControl, reviveSound);
			}
            return this;
        }
    }
}
