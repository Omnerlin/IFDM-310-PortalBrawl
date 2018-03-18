using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DennisStates
{

    public abstract class DennisState : PlayerState
    {
        public DennisPlayerController pControl; 
    }

    public class WalkState : DennisState
    {
        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override PlayerState UpdateState()
        {
            pControl.gameObject.GetComponent<DennisPlayerController>().UpdateAttack();
            pControl.gameObject.GetComponent<DennisPlayerController>().UpdateReticleRotation();
            return this;
        }
    }

    public class ReloadState : DennisState
    {
        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override PlayerState UpdateState()
        {
            pControl.gameObject.GetComponent<DennisPlayerController>().UpdateAttack();
            pControl.gameObject.GetComponent<DennisPlayerController>().UpdateReticleRotation();
            return this;
        }
    }
}