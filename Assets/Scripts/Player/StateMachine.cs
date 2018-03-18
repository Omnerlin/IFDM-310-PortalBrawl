using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only instantiate this class if it is given a playerState as a parameter
public class StateMachine <T> where T : PlayerState {

    // Keep track of the current state that we're in, so we know when to switch
    public T currentState { get; private set; }

	public void UpdateState()
    {
        if(currentState == null)
        {
            Debug.LogWarning("StateMachine doesn't have a current state set");
        }

        T newState = currentState.UpdateState() as T; 
        
        // Update the current state. If it returns a new state, switch to that state instead.
        if(newState != currentState)
        {
            currentState.OnExit();
            newState.OnEnter();
            currentState = newState;
        }
    }
}
