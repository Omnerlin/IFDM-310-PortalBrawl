using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class characterButtonScript : MonoBehaviour {

	public bool selected;
	private SpriteRenderer myRenderer;
	public Text statInfo;
	public Sprite baseImage;
	public Sprite hoverImage;
	public Sprite selectedImage;
	public Sprite tokenSprite;
    //TODO DELETE public GameObject smallPortrait;

    public GameObject startButton;
    public GameObject playImg;

	private bool usingStats = false; //Turn to true if you want character descriptions to show up when you hover over the button

	// Use this for initialization
	void Start () 
	{
		selected = false;
		myRenderer = GetComponent<SpriteRenderer> ();
		statInfo.enabled = false;
		myRenderer.sprite = baseImage;
	}

	//When the cursor is hovering over the spot
	void OnTriggerEnter2D (Collider2D other)
	{
		if(!selected)
		{
			//myRenderer.color = other.GetComponent<SpriteRenderer> ().color;
			myRenderer.sprite = hoverImage;
		}
		//statInfo.enabled = true;
	}

	//When the cursor leaves your area
	void OnTriggerExit2D (Collider2D other)
	{
		if (!selected) 
		{
			//myRenderer.color = Color.white;
			myRenderer.sprite = baseImage;
			if(usingStats) statInfo.enabled = false;
		}
	}

	/**
	 * Causes the button to be "selected." It reflects the cursor's color that selected it.
	 * other Collider2D component that has selected this button. */
	public string select(Collider2D other)
	{
		selected = true;
		//myRenderer.color = other.GetComponent<SpriteRenderer> ().color;
		myRenderer.sprite = selectedImage;
		//TODO: Make the character radicle appear
		if(usingStats) statInfo.enabled = true;
        //TODO DELETE smallPortrait.GetComponent<SpriteRenderer>().enabled = true;

        startButton.SetActive(true);
        playImg.SetActive(true);

		return name;
	}

	//todo: add deselect (selected = false; color = white;)
	public void deselect()
	{
		selected = false;
		//myRenderer.color = Color.white;
		myRenderer.sprite = baseImage;
		if(usingStats) statInfo.enabled = false;
        //TODO DELETE smallPortrait.GetComponent<SpriteRenderer>().enabled = false;

        startButton.SetActive(false);
        playImg.SetActive(false);
    }

	public Sprite getTokenSprite()
	{
		return tokenSprite;
	}
}
