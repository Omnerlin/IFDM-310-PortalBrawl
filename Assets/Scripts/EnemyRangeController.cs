using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeController : MonoBehaviour {

    public float moveForce;
    public float maxMoveSpeed = 30;

    private Rigidbody2D rb2d;
    private Transform target;
    private GameObject[] players;
    GameObject player;

    private Animator animator;

    public GunController theGun;


    // Use this for initialization
    void Start()
    {

        players = GameObject.FindGameObjectsWithTag("Player");
        player = players[Random.Range(0, players.Length)];
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 3 && theGun)
        {
            Vector3 heading = transform.position - target.position;
            Vector3 direction = heading / heading.magnitude;
            float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
            Quaternion look = Quaternion.AngleAxis(angle, Vector3.forward);
            theGun.firePoint.rotation = look;
            rb2d.AddForce((moveForce * Time.deltaTime) * direction);
            rb2d.velocity = rb2d.velocity.normalized * 0;
        }
        else if (player)
        {
            target = player.GetComponent<Transform>();

            // Get the direction of the player and move in that direction
            Vector3 heading = target.position - transform.position;
            Vector3 direction = heading / heading.magnitude;
            float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
            Quaternion look = Quaternion.AngleAxis(angle, Vector3.forward);
            theGun.firePoint.rotation = look;
            rb2d.AddForce((moveForce * Time.deltaTime) * direction);

            if (rb2d.velocity.magnitude > maxMoveSpeed)
            {
                rb2d.velocity = rb2d.velocity.normalized * maxMoveSpeed;
            }
        }
        else
        {
            player = players[Random.Range(0, players.Length)];
        }


        if (rb2d.velocity.x == 0)
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
        else if (rb2d.velocity.x < 0)
        {
            animator.SetFloat("MoveDirectionX", -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 5 && theGun)
        {
            theGun.isFiring = true;
            Vector3 heading = transform.position - target.position;
            Vector3 direction = heading / heading.magnitude;
            float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
            Quaternion look = Quaternion.AngleAxis(angle, Vector3.forward);
            theGun.firePoint.rotation = look;
            rb2d.AddForce((moveForce * Time.deltaTime) * direction);
            rb2d.velocity = rb2d.velocity.normalized * 0;

        }
        else if (theGun)
        {
            theGun.isFiring = false;
        }
        if (!player)
        {
            theGun.isFiring = false;
        }
    }
}
