using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class characterButtonScript : MonoBehaviour {

	public bool selected;
	private SpriteRenderer myRenderer;
	public Text statInfo;

	// Use this for initialization
	void Start () 
	{
		selected = false;
		myRenderer = GetComponent<SpriteRenderer> ();
		statInfo.enabled = false;
	}

	//When the cursor is hovering over the spot
	void OnTriggerEnter2D (Collider2D other)
	{
		if(!selected) myRenderer.color = other.GetComponent<SpriteRenderer> ().color;
		statInfo.enabled = true;
	}

	//When the cursor leaves your area
	void OnTriggerExit2D (Collider2D other)
	{
		if (!selected) 
		{
			myRenderer.color = Color.white;
			statInfo.enabled = false;
		}
	}

	/**
	 * Causes the button to be "selected." It reflects the cursor's color that selected it.
	 * other Collider2D component that has selected this button. */
	public string select(Collider2D other)
	{
		selected = true;
		myRenderer.color = other.GetComponent<SpriteRenderer> ().color;
		statInfo.enabled = true;
		return name;
	}

	//todo: add deselect (selected = false; color = white;)
	public void deselect()
	{
		selected = false;
		myRenderer.color = Color.white;
		statInfo.enabled = false;
	}
}
