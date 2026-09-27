using Manager;
using Player;
using UnityEngine;

public class AliveState : State
{
    private PlayerController _player;
    
    public AliveState(StateMachine fsm, PlayerController player) : base(fsm)
    {
        _player = player;
    }

    public override void Enter()
    {
        GameEvents.onPlayerSpawn?.Invoke(_player.transform);
        _player.SwitchGameplay(true);
        _player.SwitchMapNavigation(false);
    }

    public override void Update()
    {

    }
    
    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        
    }
    
   
    
}
