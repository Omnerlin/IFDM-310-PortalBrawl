using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteractionObject : MonoBehaviour {

    [Tooltip("What a user's action UI text will say after approaching this object")]
    public string actionName;

    public UnityEvent onInteract;

    [HideInInspector] public bool interactable = true;

}
