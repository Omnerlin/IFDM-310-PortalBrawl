using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RevdiocPlayerController : PlayerControl
{
    // Extended class of PlayerState that has a reference to the DennisPlayerController
    public abstract class RevdiocState : PlayerState
    {
        public RevdiocPlayerController pControl; public RevdiocState(RevdiocPlayerController cont) { pControl = cont; }
    }

    // Class that will be execute when dennis can roam freely
    public class WalkState : RevdiocState
    {
        public WalkState(RevdiocPlayerController cont) : base(cont) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState UpdateState()
        {

            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();
            pControl.UpdatePlayerMovement();

            return this;
        }
    }

    public class AttackState : RevdiocState
    {
        public AttackState(RevdiocPlayerController cont) : base(cont) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState UpdateState()
        {
            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();

            return this;
        }
    }

    public class StunState : RevdiocState
    {
        public StunState(RevdiocPlayerController cont) : base(cont) { }

        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState UpdateState()
        {
            pControl.UpdateAttack();
            pControl.UpdateReticleRotation();

            return this;
        }
    }
}
