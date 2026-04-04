using System.Collections.Generic;

public class StateMachine
{
    private State currentState;
    private Dictionary<System.Type, State> states = new Dictionary<System.Type, State>();

    public void RegisterState(State state)
    {
        states[state.GetType()] = state;
    }

    public void ChangeState<T>() where T : State
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = states[typeof(T)];
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
    
}