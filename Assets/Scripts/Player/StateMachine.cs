using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only instantiate this class if it is given a playerState as a parameter
public class StateMachine <T> where T : PlayerState {

    // Keep track of the current state that we're in, so we know when to switch
    public T currentState { get; private set; }

    // Has onEnter been called for the first state yet?
    private bool hasStartedUp = false;

    // Constructor that supplies what state the machine should start in
    public StateMachine (PlayerState startingState)
    { currentState = startingState as T; }

	public void UpdateState()
    {
        if(!hasStartedUp)
        {
            currentState.OnEnter();
            hasStartedUp = true;
        }

        if(currentState == null)
        {
            Debug.LogWarning("StateMachine doesn't have a current state set");
            return;
        }

        // Update the current state. If it returns a new state, switch to that state.
        T newState = currentState.Update() as T; 
        if(newState != currentState)
        {
            currentState.OnExit();
            newState.OnEnter();
            currentState = newState;
        }
    }
}
