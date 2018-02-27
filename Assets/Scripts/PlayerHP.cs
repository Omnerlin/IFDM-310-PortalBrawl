using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour {

    public int maxHP;
    public int currentHP;
	// Use this for initialization
	void Start () {
        currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void HurtPlayer(int damage)
    {
        currentHP -= damage;
    }

    public void setMaxHP()
    {
        currentHP = maxHP;
    }
}
