using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just a helper script used with the animator to spawn a monster with a delay. 
// The monster to be spawned will be supplied by the enemySpawner

public class PortalSpawn : MonoBehaviour {

    [HideInInspector] public GameObject monsterToSpawn;

    private void Awake()
    {
        if(!monsterToSpawn)
        {
            Destroy(this.gameObject);
        }
    }

    public void spawnEnemy()
    {
        Instantiate(monsterToSpawn, this.transform.position, this.transform.rotation);
        GetComponent<Animator>().SetTrigger("Close");
    }

    // Use this function with the animator so that this will automatically be destroyed at the end
    public void DestorySelf()
    {
        Destroy(this.gameObject);
    }
    
}
