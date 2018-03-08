using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMelee : MonoBehaviour {


    private List<GameObject> enemiesAlreadyHit;

	// Use this for initialization
	void Start () {
		
        if(Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<Animator>().SetTrigger("Attack");
            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
