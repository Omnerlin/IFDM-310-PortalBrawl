using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour {
    public float moveForce;
    public float maxMoveSpeed = 30;

    private Rigidbody2D rb2d;
    private Transform target;
    private GameObject[] players;
    private GameObject player;

    private Animator animator;


	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        player = players[Random.Range(0, players.Length)];
        //if(player)
        Debug.Log(player.transform.root.name + "player is");
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(player && player.GetComponent<Player>().getMyData().currentHealth > 0)
        {

            target = player.GetComponent<Transform>();

            // Get the direction of the player and move in that direction
            Vector3 heading = target.position - transform.position;
            Vector3 direction = heading / heading.magnitude;
            rb2d.AddForce((moveForce * Time.deltaTime) * direction);

            if(rb2d.velocity.magnitude > maxMoveSpeed)
            {
                rb2d.velocity = rb2d.velocity.normalized * maxMoveSpeed;
            }
        }
        else
        {
            player = players[Random.Range(0, players.Length)];
        }

        if(rb2d.velocity.x == 0)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        if (rb2d.velocity.x > 0)
        {
            animator.SetFloat("MoveDirectionX", 1);
        }
        else if(rb2d.velocity.x < 0)
        {
            animator.SetFloat("MoveDirectionX", -1);
        }


    }

    // Update is called once per frame
    void Update ()
    {
        
    }
    
}
