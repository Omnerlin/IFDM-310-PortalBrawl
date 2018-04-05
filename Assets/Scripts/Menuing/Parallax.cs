using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {


    [Tooltip("How smoothing should be applied to the movement (Lerping)")]
    public float smoothing = 1f;

    // How much camera movement will affect this object's position. 
    // this will be set based on the objects Z position
    private float parallaxScale;

    // Keep a reference to the transform of the camera
    private Transform camTransform;

    // Keep track of the camera's previous position for delta
    private Vector3 prevCameraPostition;

    // Use this for initialization
    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

    private void Start()
    {
        prevCameraPostition = camTransform.position;
        parallaxScale = -gameObject.transform.position.z * 5;
    }

    // Update is called once per frame
    void Update () {

        // Set this every frame just in case it gets updated
        parallaxScale = -gameObject.transform.position.z * 5;


        float parallaxX = (prevCameraPostition.x - camTransform.position.x) * parallaxScale;
        float parallaxY = (prevCameraPostition.y - camTransform.position.y) * parallaxScale;

        float targetPositionX = gameObject.transform.position.x + parallaxX;
        float targetPositionY = gameObject.transform.position.y + parallaxY;


        Vector3 targetPosition = new Vector3(targetPositionX, targetPositionY, gameObject.transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

        prevCameraPostition = camTransform.position;
	}
}
