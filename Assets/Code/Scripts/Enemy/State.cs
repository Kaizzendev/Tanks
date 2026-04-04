using Player;
using UnityEngine;

public abstract class State
{
    protected StateMachine fsm;
    
    public State(StateMachine fsm)
    {
        this.fsm = fsm;
    }

    public virtual void Enter() {}
    public virtual void Update() {}
    public virtual void Exit() {}
}
