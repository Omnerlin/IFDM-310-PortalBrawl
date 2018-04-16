using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    // Object we are focused on
    private GameObject focusedObject = null;

    // Collider to use for interaction triggers
    public Collider2D interactionCollider;

    // List of objects that the player is currently intersecting with
    private List<GameObject> objectsInRange;

    // Reference to the player's actionCanvas
    private GameObject actionCanvas;

    private void Awake()
    {
        objectsInRange = new List<GameObject>();
        actionCanvas = GetComponentInChildren<ActionCanvas>(true).gameObject;
    }

    private void Update()
    {
        UpdateFocusedObject();
    }

    private void UpdateFocusedObject()
    {
        GameObject prevFocusedObject = focusedObject;
        focusedObject = null;

        // Compare distances and focus on the closest interactable
        float minDistance = float.MaxValue;

        foreach (GameObject go in objectsInRange)
        {
            float dist = Vector3.Distance(go.transform.position, transform.position);
            if (dist < minDistance && go.GetComponent<InteractionObject>().interactable)
            {
                minDistance = dist;
                focusedObject = go;
            }
        }

        ActionCanvas canvas = actionCanvas.GetComponent<ActionCanvas>();

        if(focusedObject)
        {
            canvas.gameObject.SetActive(true);
            canvas.actionText.text = focusedObject.GetComponent<InteractionObject>().actionName;
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure our InteractionCollider is the one that is overlapping
        if (other.IsTouching(interactionCollider) && other.GetComponent<InteractionObject>())
        {
            if (!objectsInRange.Contains(other.gameObject))
            {
                objectsInRange.Add(other.gameObject);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        // Make sure our interactionCollider is the collider that left
        if (!other.IsTouching(interactionCollider))
        {
            objectsInRange.Remove(other.gameObject);
        }
    }

    public void Interact()
    {
        if (focusedObject)
        {
            InteractionObject interObj = focusedObject.GetComponent<InteractionObject>();
            interObj.onInteract.Invoke();
        }
    }
}