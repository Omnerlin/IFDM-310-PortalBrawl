using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSoundPlayer : MonoBehaviour {
    public AudioSource sound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound()
    {
        sound.Play();
    }
}
