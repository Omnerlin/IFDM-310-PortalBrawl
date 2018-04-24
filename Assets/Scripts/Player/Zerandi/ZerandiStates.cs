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
        public AudioSource[] soundSources;
        public ZerandiState(ZerandiPlayerController cont, AudioSource[] sources)
        {
            pControl = cont;
            soundSources = sources;
        }
    }

    // Class that will be execute when dennis can roam freely
    public class WalkState : ZerandiState
    {
        public WalkState(ZerandiPlayerController cont, AudioSource[] sources) : base(cont, sources) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        

        public override PlayerState Update()
        {
            if(pControl.GetComponent<Player>().isDead())
            {
                return new DeathState(pControl, soundSources);
            }

            pControl.UpdateAttack();
            pControl.UpdateReticleOrientation();
            pControl.UpdatePlayerMovement(soundSources);

            return this;
        }
    }

    public class StunState : ZerandiState
    {
        public StunState(ZerandiPlayerController cont, AudioSource[] sources) : base(cont, sources) { }

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
        public DeathState(ZerandiPlayerController cont, AudioSource[] sources) : base(cont, sources) { }

        public override void OnEnter()
        {
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
            soundSources[2].Play();
        }

        public override PlayerState Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                pControl.GetComponent<Player>().setMaxHP();
                soundSources[2].Play();
                return new WalkState(pControl, soundSources);

            }
            return this;
        }
    }
}
