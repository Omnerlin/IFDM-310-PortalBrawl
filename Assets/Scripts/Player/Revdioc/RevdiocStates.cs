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

        public override PlayerState Update()
        {
			if (pControl.playerInfo.isDead ()) 
			{
				return new DeadState (pControl);
			}
            // If the player hits the rightbumper, return the attack state
            if(pControl.player.GetButtonDown("RightBumper"))
            {
                return new AttackState(pControl);
            }
            pControl.UpdateReticleRotation();
            pControl.UpdatePlayerMovement();

            return this;
        }
    }

    public class AttackState : RevdiocState
    {
        public AttackState(RevdiocPlayerController cont) : base(cont) { hammerHitBox = pControl.hammerHitBox;
            timeActive = pControl.hitboxTimeActive; filter = pControl.hitboxFilter; cooldown = pControl.attackCooldown; }

        private GameObject hammerHitBox;
        private float timeActive; // in seconds
        private float cooldown;
        private Collider2D[] hitColliders = new Collider2D[20];
        private List<GameObject> hitEnemies = new List<GameObject>();
        private ContactFilter2D filter;

        public override void OnEnter()
        {
            // Remove the hitbox from revdioc's transform.... we don't actually want it there. It was just there for organizational purposes.
            // Enable the hitbox, and check collision with it.
            hammerHitBox.transform.position = pControl.hammer.transform.position;
            hammerHitBox.transform.rotation = pControl.hammer.transform.rotation;

            hammerHitBox.transform.parent = null;
            hammerHitBox.SetActive(true);

            CameraControl.Instance.AddCameraShake(4);
        }

        public override void OnExit()
        {
        }

        public override PlayerState Update()
        {
            timeActive -= Time.deltaTime;
            if(timeActive <= 0) { cooldown -= Time.deltaTime; hammerHitBox.SetActive(false);} // Return walk state if we can attack again
            if (cooldown <= 0) { return new WalkState(pControl); }

            Physics2D.OverlapCollider(hammerHitBox.GetComponent<Collider2D>(), filter,  hitColliders);
            foreach (Collider2D col in hitColliders)
            {
                if(col != null && col.tag == "Enemy" && !hitEnemies.Contains(col.gameObject))
                {
                    col.gameObject.GetComponent<EnemyHp>().HurtEnemy(1);
                    Vector3 heading = col.transform.position - pControl.transform.position;
                    Vector3 direction = heading / heading.sqrMagnitude;
                    col.gameObject.GetComponent<Rigidbody2D>().AddForce(pControl.hitForce * direction);
                    hitEnemies.Add(col.gameObject);
                }
            }

            pControl.UpdatePlayerMovement();
            pControl.UpdateReticleRotation();
            return this;
        }
    }

	public class DeadState : RevdiocState
	{
		public DeadState(RevdiocPlayerController cont) : base(cont) { }

		public override void OnEnter() 
		{
			//Trigger Death animation
			pControl.animator.SetTrigger("Die");
            pControl.deathVisuals.SetActive(true);
            pControl.hammer.SetActive(false);
            pControl.rb2d.isKinematic = true;
		}

		public override void OnExit() 
		{
            //Revive? animation, poof?
            pControl.deathVisuals.SetActive(false);
            pControl.hammer.SetActive(true);
            pControl.rb2d.isKinematic = false;
        }

        public override PlayerState Update()
		{
			//TODO: If revived, return walkstate.
			
            if(Input.GetKeyDown(KeyCode.R))
            {
                pControl.GetComponent<Player>().setMaxHP();
                pControl.animator.SetTrigger("Revive");
                return new WalkState(pControl);
            }

			return this; //Sorry, endless loop. You can't do anything while you're down. 
		}
	}
}
