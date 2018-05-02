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
                case 0:
                    col.offset = transform.InverseTransformDirection(cam.ViewportToWorldPoint(new Vector3(0.5f, 1)));
                    col.size = new Vector2(camWidth, boxThiccness);
                    break;
                case 1:
                    col.offset = cam.ViewportToWorldPoint(new Vector3(1, 0.5f));
                    col.size = new Vector2(boxThiccness, camHeight);
                    break;
                case 2:
                    col.offset = cam.ViewportToWorldPoint(new Vector3(0.5f, 0));
                    col.size = new Vector2(camWidth, boxThiccness);
                    break;
                case 3:
                    col.offset = cam.ViewportToWorldPoint(new Vector3(0, 0.5f));
                    col.size = new Vector2(boxThiccness, camHeight);
                    break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
