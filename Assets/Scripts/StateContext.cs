using System;
using UnityEngine;

public class StateContext : Singleton<StateContext>
{
    [SerializeField] private State _currentState;
    public State GetCurrentState => _currentState;

    public void Transition(State state)
    {
        _currentState = state;
    }
}