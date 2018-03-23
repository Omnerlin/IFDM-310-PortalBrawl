using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DennisPlayerController : PlayerControl
{
    // Extended class of PlayerState that has a reference to the DennisPlayerController
    public abstract class DennisState : PlayerState {
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

            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();
            pControl.UpdatePlayerMovement();
            
            return this;
        }
    }

    public class StunState : DennisState
    {
        public StunState(DennisPlayerController cont) : base(cont) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState Update()
        {
            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();

            return this;
        }
    }
}