using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour {

    // How thick the boxes for the colliders should be.
    public float boxThiccness = 2;

    // Get a reference to the camera component
    private Camera cam;

    // The colliders used for keeping players in view.
    private BoxCollider2D[] colliders;


	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 2.0f * camHalfWidth;
        float camHeight = camHalfHeight * 2;

        colliders = new BoxCollider2D[4]; // Top, Right, Bottom, Left

        // Figure out where these boxes should be put, and make them as thicc as they need to be.
        for(int i = 0; i < colliders.Length; i++)
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            
            switch(i)
            {
                // Top
                case 0:
                    col.size = new Vector2(camWidth, boxThiccness);
                    col.offset = new Vector2(0, camHalfHeight + boxThiccness/2);
                    break;
                // Right
                case 1:
                    col.size = new Vector2(boxThiccness, camHeight);
                    col.offset = new Vector2(camHalfWidth + boxThiccness / 2, 0);
                    break;
                // Bottom
                case 2:
                    col.size = new Vector2(camWidth, boxThiccness);
                    col.offset = new Vector2(0, -camHalfHeight - boxThiccness / 2);
                    break;
                // Left
                case 3:
                    col.size = new Vector2(boxThiccness, camHeight);
                    col.offset = new Vector2(-camHalfWidth - boxThiccness / 2, 0);
                    break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
