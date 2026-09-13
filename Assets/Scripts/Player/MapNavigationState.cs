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
            MapManager.Instance.OnNodeSelected += GoToNode;
        }

        private void GoToNode(MapNode node)
        {
            
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}