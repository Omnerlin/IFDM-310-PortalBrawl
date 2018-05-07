using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class ControlScreen : MonoBehaviour {

    public IList<Rewired.Player> players { get; set; }
    public AudioSource sounds;

    private void Awake()
    {
        players = ReInput.players.GetPlayers(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Rewired.Player player in players)
        {
            if (player.GetButtonDown("Start") || player.GetButtonDown("XButton"))
            {
                Debug.Log("loading level");
                SceneManager.LoadScene("Alexlevel");
                sounds.Play();
            }
        }


    }
}
