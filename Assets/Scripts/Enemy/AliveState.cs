using UnityEngine;

namespace Enemy
{
    public class AliveState: State
    {
        private EnemyController _enemy;
        
        private EnemyStats _stats;
        public AliveState(StateMachine fsm, EnemyController enemy, EnemyStats stats) : base(fsm)
        {
            _enemy = enemy;
            _stats = stats;
        }

        public override void Enter()
        {
            _enemy.SwitchGameplay(true);
        }


        public override void Exit()
        {
            _enemy.SwitchGameplay(false);
            _stats.navMeshAgent.isStopped = false;
        }
    }
}