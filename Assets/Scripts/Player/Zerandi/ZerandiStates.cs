using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public partial class ZerandiPlayerController : PlayerControl
{
    // Extended class of PlayerState that has a reference to the DennisPlayerController
    public abstract class ZerandiState : PlayerState
    {
        public ZerandiPlayerController pControl; public ZerandiState(ZerandiPlayerController cont) { pControl = cont; }
    }

    // Class that will be execute when dennis can roam freely
    public class WalkState : ZerandiState
    {
        public WalkState(ZerandiPlayerController cont) : base(cont) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState Update()
        {

            pControl.UpdateAttack();
            pControl.UpdateReticleOrientation();
            pControl.UpdatePlayerMovement();

            return this;
        }
    }

    public class StunState : ZerandiState
    {
        public StunState(ZerandiPlayerController cont) : base(cont) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState Update()
        {
            pControl.UpdateAttack();
            pControl.UpdateReticleOrientation();

            return this;
        }
    }
}
