using UnityEngine;

public class StateContext : MonoBehaviour
{
    private readonly PlayerController Controller;
    private State _currentState;
    public State GetCurrentState => _currentState;

    public StateContext(PlayerController _controller)
    {
        Controller = _controller;
    }

    public void Transition(State state)
    {
        _currentState = state;
    }
}