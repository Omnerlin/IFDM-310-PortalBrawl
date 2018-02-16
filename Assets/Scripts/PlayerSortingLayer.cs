using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSortingLayer : MonoBehaviour {

	Transform myTransform;
	SpriteRenderer myRenderer;

	// Use this for initialization
	void Start () 
	{
		myTransform = GetComponent<Transform>();
		myRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		myRenderer.sortingOrder	= (int) myTransform.position.y;
	}
}
