using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AnixPlayerController : PlayerControl
{
    // Extended class of PlayerState that has a reference to the DennisPlayerController
    public abstract class AnixState : PlayerState
    {
        public AnixPlayerController pControl; public AnixState(AnixPlayerController cont) { pControl = cont; }
    }

    // Class that will be execute when dennis can roam freely
    public class WalkState : AnixState
    {

        public WalkState(AnixPlayerController cont) : base(cont) { }


        public override void OnEnter() { }
        public override void OnExit() { }

        public override PlayerState Update()
        {
            // If the player hits the rightbumper, return the attack state
            if(pControl.GetComponent<Player>().isDead())
            {
                return new DeathState(pControl);
            }

            if (pControl.player.GetButtonDown("RightBumper"))
            {
                return new AttackState(pControl);
            }

			//Player can revive someone else
			if(pControl.player.GetButtonDown("TriangleButton"))
			{
				attemptToRevive ();
			}

            pControl.UpdateReticleRotation();
            pControl.UpdatePlayerMovement();

            return this;
        }

		public void attemptToRevive()
		{
			Debug.Log ("Attempting to revive...");
			ContactFilter2D filter = new ContactFilter2D ();
			filter.NoFilter ();
			GameObject walkbox = pControl.gameObject.GetComponent<Transform>().Find("Walkbox").gameObject;
			if(walkbox == null) 
			{
				Debug.LogWarning("Was not able to find "+pControl.name+"'s walkbox");
				return;
			}

			Collider2D[] collideWithMe = new Collider2D[20];
			//If the player's collider is overlapping with another player's collider who is dead
			Physics2D.OverlapCollider(walkbox.GetComponent<BoxCollider2D> (), filter, collideWithMe);
			foreach (Collider2D col in collideWithMe)
			{
				if (col.gameObject.name == "ReviveBox") 
				{
					Player otherPlayer = col.gameObject.transform.parent.transform.parent.GetComponent<Player> (); //...It's two layers deep. >.<
					pControl.GetComponent<Player> ().reviveOtherPlayer (otherPlayer);
				}
				//(We don't need another state for this because it's a rather quick thing.)
			}
		}
    }

    public class AttackState : AnixState
    {
        public AttackState(AnixPlayerController cont) : base(cont)
        {
            staffHitbox = pControl.staffHitbox;
            timeActive = pControl.hitboxTimeActive; filter = pControl.hitboxFilter; cooldown = pControl.attackCooldown;
        }

        private GameObject staffHitbox;
        private float timeActive; // in seconds
        private float cooldown;
        private Collider2D[] hitColliders = new Collider2D[20];
        private List<GameObject> hitEnemies = new List<GameObject>();
        private ContactFilter2D filter;

        public override void OnEnter()
        {
            // Remove the hitbox from revdioc's transform.... we don't actually want it there. It was just there for organizational purposes.
            // Enable the hitbox, and check collision with it.
            staffHitbox.transform.position = pControl.staff.transform.position;
            staffHitbox.transform.rotation = pControl.staff.transform.rotation;

            staffHitbox.transform.parent = null;
            staffHitbox.SetActive(true);

            pControl.staffVisuals.GetComponent<Animator>().SetTrigger("Swing");

            CameraControl.Instance.AddCameraShake(5);
        }

        public override void OnExit()
        {
            staffHitbox.SetActive(false);
        }

        public override PlayerState Update()
        {
            timeActive -= Time.deltaTime;
            if (timeActive <= 0) { cooldown -= Time.deltaTime; staffHitbox.SetActive(false); } // Return walk state if we can attack again
            if (cooldown <= 0) { return new WalkState(pControl); }

            if (pControl.GetComponent<Player>().isDead())
            {
                return new DeathState(pControl);
            }

            CheckHitCollisions();
            pControl.UpdatePlayerMovement();
            pControl.UpdateReticleRotation();
            return this;
        }

        public void CheckHitCollisions()
        {
            Physics2D.OverlapCollider(staffHitbox.GetComponent<Collider2D>(), filter, hitColliders);
            foreach (Collider2D col in hitColliders)
            {
                if (col != null && col.tag == "Enemy" && !hitEnemies.Contains(col.gameObject))
                {
                    col.gameObject.GetComponent<EnemyHp>().HurtEnemy(1);
                    Vector3 heading = col.transform.position - pControl.transform.position;
                    Vector3 direction = heading / heading.sqrMagnitude;
                    col.gameObject.GetComponent<Rigidbody2D>().AddForce(pControl.hitForce * direction);
                    hitEnemies.Add(col.gameObject);
                }
            }
        }
    }

    public class DeathState : AnixState
    {

        public DeathState(AnixPlayerController cont) : base(cont) { }

        public override void OnEnter()
        {
            pControl.animator.SetTrigger("Die");
            pControl.deathVisuals.SetActive(true);
            pControl.rb2d.velocity = Vector2.zero;
            pControl.rb2d.isKinematic = true;
            pControl.staffVisuals.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        public override void OnExit()
        {
            pControl.rb2d.isKinematic = false;
            pControl.deathVisuals.SetActive(false);
            pControl.animator.SetTrigger("Revive");
            pControl.staffVisuals.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }

        public override PlayerState Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                pControl.GetComponent<Player>().setMaxHP();
                return new WalkState(pControl);
            }
			if (!pControl.GetComponent<Player> ().isDead ()) //Someone gave them health by reviving them or otherwise healing them
			{ 
				return new WalkState(pControl);
			}
            return this;
        }
    }
}
