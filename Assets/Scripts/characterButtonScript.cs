using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterButtonScript : MonoBehaviour {

	public bool selected;
	private SpriteRenderer myRenderer;

	// Use this for initialization
	void Start () 
	{
		selected = false;
		myRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	//When the cursor is hovering over the spot
	void OnTriggerEnter2D (Collider2D other)
	{
		if(!selected) myRenderer.color = other.GetComponent<SpriteRenderer> ().color;
	}

	//When the cursor leaves your area
	void OnTriggerExit2D (Collider2D other)
	{
		if(!selected) myRenderer.color = Color.white;
	}

	/**
	 * Causes the button to be "selected." It reflects the cursor's color that selected it.
	 * other Collider2D component that has selected this button. */
	public string choose(Collider2D other)
	{
		selected = true;
		myRenderer.color = other.GetComponent<SpriteRenderer> ().color;
		return name;
	}

	//todo: add deselect (selected = false; color = white;)
}
