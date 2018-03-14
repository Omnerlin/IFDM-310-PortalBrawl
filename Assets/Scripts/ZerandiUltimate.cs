using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZerandiUltimate : MonoBehaviour {
    CircleCollider2D myCollid;
    public bool isActive;
    public float maxUltRange;
    public float maxUltTime;
    private float ultTimeCounter;
    public float ultRangeGrowth;

    // Use this for initialization
    void Start () {
        myCollid = transform.GetComponent<CircleCollider2D>();
        myCollid.radius = .25f;
    }
	
	// Update is called once per frame
	void Update () {
        if(isActive.Equals(true))
        {
            if(ultRangeGrowth <= maxUltTime)
            {
                if (myCollid.radius < maxUltRange)
                {
                    myCollid.radius += ultRangeGrowth * Time.deltaTime;
                }
                else
                {
                    myCollid.radius = 0.2f;
                }
                ultRangeGrowth += Time.deltaTime;
            }
            else
            {
                isActive = false;
                ultRangeGrowth = 0;
            }
            
            
        }
        else
        {
            myCollid.radius = 0.2f;
        }
        
      
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHp>().HurtEnemy(1);
        }
        else if (other.gameObject.tag.Equals("Player"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }
        else if (other.gameObject.tag.Equals("Environment"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }
    }
}
