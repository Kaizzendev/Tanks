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

        public override void Update()
        {
            if (_enemy.distance < _stats.fireRange)
            {
                _enemy._attackController.Attack(_enemy._player.position);
            }

            else if (_enemy.distance > _stats.fireRange && _enemy.distance < _stats.detectionRange)
            {
                _enemy._chaseController.Chase(_enemy._player.position);
            }
            
            else
            {
                _enemy._patrolController.Patrol();
            }
            
            Debug.Log($"is stopped: {_stats.navMeshAgent.isStopped}");
        }

        public override void Exit()
        {
            _enemy.SwitchGameplay(false);
            _stats.navMeshAgent.isStopped = false;
        }
    }
}