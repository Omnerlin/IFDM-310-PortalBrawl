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
        public AudioSource[] soundSounds;
        public DennisState(DennisPlayerController cont, AudioSource[] sources)
        {
            pControl = cont;
            soundSounds = sources;
        }
    }

    // Class that will be execute when dennis can roam freely
    public class WalkState : DennisState
    {
        public WalkState(DennisPlayerController cont, AudioSource[] sources) : base(cont, sources){}

        public override void OnEnter(){}
        public override void OnExit(){}

        public override PlayerState Update()
        {
            if(pControl.GetComponent<Player>().isDead())
            {
                return new DeathState(pControl, soundSounds);
            }

            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();
            pControl.UpdatePlayerMovement(soundSounds);
            
            return this;
        }
    }

    public class DeathState : DennisState
    {
        public DeathState(DennisPlayerController cont, AudioSource[] sources) : base(cont, sources) { }

        public override void OnEnter()
        {
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
            soundSounds[2].Play();
            pControl.aimReticle.SetActive(true);
        }

        public override PlayerState Update()
        {
            // Revive for debugging
            if(Input.GetKeyDown(KeyCode.R))
            {
                pControl.GetComponent<Player>().setMaxHP();
                soundSounds[2].Play();
                return new WalkState(pControl, soundSounds);
            }

            return this;
        }
    }
}