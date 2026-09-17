namespace Enemy
{
    public class DeadState: State
    {
        private EnemyController _enemy;
        
        public DeadState(StateMachine fsm, EnemyController enemy) : base(fsm)
        {
            _enemy = enemy;

        }

        public override void Enter()
        {
            _enemy.SwitchGameplay(false);
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