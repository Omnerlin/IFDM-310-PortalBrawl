using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectScale : MonoBehaviour {

    [Tooltip("Number of vertical pixels for our reference resolution")]
    public int screenVerticalPixels = 270;

    [Tooltip("Whether the screen should have borders or should be cropped to fill the view")]
    public bool preferUncropped = true;

    private float screenPixelsY = 0;

    private bool currentCropped = false;

	// Update is called once per frame
	void Update () {
		if(screenPixelsY != (float)Screen.height || currentCropped != preferUncropped)
        {
            screenPixelsY = (float)Screen.height;
            currentCropped = preferUncropped;

            float screenRatio = screenPixelsY / screenVerticalPixels;
            float ratio;

            if(preferUncropped)
            {
                ratio = Mathf.Floor(screenRatio) / screenRatio;
            }
            else
            {
                ratio = Mathf.Ceil(screenRatio) / screenRatio;
            }

            transform.localScale = Vector3.one * ratio;
        }
	}
}
