using UnityEngine;

namespace Enemy
{
    public class DeadState: State
    {
        private EnemyController _enemy;
        private EnemyStats _stats;
        
        public DeadState(StateMachine fsm, EnemyController enemy, EnemyStats stats) : base(fsm)
        {
            _enemy = enemy;
            _stats = stats;

        }

        public override void Enter()
        {
            _enemy.SwitchGameplay(false);
            _stats.navMeshAgent.velocity = Vector3.zero;
            _stats.navMeshAgent.isStopped = true;
            _enemy.enabled = false;
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