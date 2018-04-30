using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;

    public float lifeTime;
    public int damageForEnemy;

    public bool isEnemyShot;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (isEnemyShot)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                other.gameObject.GetComponent<Player>().hurtPlayer(damageForEnemy);
            }
            else if (other.gameObject.tag.Equals("Enemy"))
            {
                // Ignore contact with the player since they shot it (Will need to change this later)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider);
                return;
            }
        }
        else
        {
            if (other.gameObject.tag.Equals("Enemy"))
            {
                other.gameObject.GetComponent<EnemyHp>().HurtEnemy(damageForEnemy);
            }
            else if (other.gameObject.tag.Equals("Player"))
            {
                // Ignore contact with the player since they shot it (Will need to change this later)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider);
                return;
            }
        }

        // Trigger the explode animation and stop the movement
        GetComponent<Animator>().SetTrigger("Explode");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        speed = 0;
        GetComponent<Collider2D>().enabled = false;

    }

    // Making this function so that the animator can reference it
    public void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}