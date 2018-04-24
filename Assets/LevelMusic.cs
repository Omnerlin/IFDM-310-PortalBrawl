using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour {
    public AudioSource[] music = new AudioSource[2];
    private double timeNextMusic;
    private bool running = false;
    public double timeToWait;

	// Use this for initialization
	void Start () {
        music[0].Play();
        timeNextMusic = AudioSettings.dspTime + timeToWait;
        running = true;
        

    }
	
	// Update is called once per frame
	void Update () {
        if (!running)
        {
            return;
        }
        double time = AudioSettings.dspTime;
        if(time + 1.0F > timeNextMusic && music.Length > 1)
        {
            music[1].PlayScheduled(timeNextMusic);
            running = false;
        }
	}
}
