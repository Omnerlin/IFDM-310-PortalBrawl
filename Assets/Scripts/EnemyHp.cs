using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour {

    public int hP;
    public int currentHP;

	// Use this for initialization
	void Start () {
        currentHP = hP;
	}
	
	// Update is called once per frame
	void Update () {
        if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
		
	}
    public void HurtEnemy(int damage)
    {
        currentHP -= damage;
    }
}
