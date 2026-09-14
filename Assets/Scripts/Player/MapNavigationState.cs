using Manager;
using Map;

namespace Player
{
    public class MapNavigationState: State
    {
        
        private PlayerController _player;
        
        public MapNavigationState(StateMachine fsm, PlayerController player) : base(fsm)
        {
            _player = player;
        }

        public override void Enter()
        {
            _player.SwitchGameplay(false);
            _player.SwitchMapNavigation(true);
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            _player.SwitchMapNavigation(false);
        }
    }
}