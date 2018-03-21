using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState {

    // Function to execute when this state is entered
    abstract public void OnEnter();

    // Function to execute when this state is being exited
    abstract public void OnExit();

    // Function to execute when updateing this state
    abstract public PlayerState Update();

}
