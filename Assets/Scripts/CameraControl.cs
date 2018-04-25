using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour {

    // This class will be a singleton, keeping track of the game cam.
    public static CameraControl Instance;

    [Tooltip("The speed that camera shake will be reduced if there is any applied")]
    public float shakeDiminishRate = 5f;

    // Reference to the group follow cam
    private CinemachineVirtualCamera cineCam;

    // The amount of cameraShake currently applied to the camera
    private float cameraShake = 0;

	// Use this for initialization
	void Awake () {
		if(Instance == null)
        {
            Instance = this;
            cineCam = GameObject.Find("GroupFollowCam").GetComponent<CinemachineVirtualCamera>();
        }
        else
        {
            Destroy(this);
        }


	}
	
	// Update is called once per frame
	void Update ()
    {
		if(cameraShake > 0)
        {
            cameraShake -= shakeDiminishRate * Time.deltaTime;
        }
        else
        {
            cameraShake = 0;
        }

        if(cineCam)
        cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = cameraShake;
    }

    public void AddCameraShake(float shake)
    {
        cameraShake += shake;
        cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = cameraShake;
    }
}
