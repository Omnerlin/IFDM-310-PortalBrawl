using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    private Rigidbody2D myRB2D;
    public float moveSpeed;
    private bool isMoving;
    public float timeBetweenMove;
    private float timeBetweenMoveCount;
    public float timeToMove;
    private float timeToMoveCount;
    private Vector3 moveDirection;

	// Use this for initialization
	void Start () {
        myRB2D = GetComponent<Rigidbody2D>();
        timeBetweenMoveCount = timeBetweenMove;
        timeToMoveCount = timeToMove;
	}
    void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update () {
        if (isMoving)
        {
            timeToMoveCount -= Time.deltaTime;
            myRB2D.velocity = moveDirection;
            if(timeToMoveCount < 0f)
            {
                isMoving = false;
                timeBetweenMoveCount = timeBetweenMove;
            }
        }
        else
        {
            timeBetweenMoveCount -= Time.deltaTime;
            myRB2D.velocity = Vector2.zero;
            if(timeBetweenMoveCount < 0f)
            {
                isMoving = true;
                timeToMoveCount = timeToMove;
                moveDirection = new Vector3(Random.Range(-1f, 1f)*moveSpeed, Random.Range(-1f, 1f)*moveSpeed, 0f);
            }
        }
    }
}
