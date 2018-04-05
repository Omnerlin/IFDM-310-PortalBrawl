using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverImage : MonoBehaviour {

    Renderer textColor;

    // Use this for initialization

    void Start()

    {

        textColor = GetComponent<Renderer>();

        textColor.material.color = Color.white;

    }

    // Update is called once per frame
    void Update () {
		
	}
    private void OnMouseOver()

    {

        textColor.material.color = Color.blue;

    }

    private void OnMouseExit()

    {

        textColor.material.color = Color.white;

    }
}
