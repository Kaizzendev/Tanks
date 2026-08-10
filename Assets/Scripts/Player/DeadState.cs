namespace Player
{
    public class DeadState: State
    {
        private PlayerController _player;
        public DeadState(StateMachine fsm,PlayerController player) : base(fsm)
        {
            _player = player;
        }
        
        public override void Enter()
        {
        
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
}