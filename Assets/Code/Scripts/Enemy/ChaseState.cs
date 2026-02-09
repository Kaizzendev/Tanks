using UnityEngine;

namespace Player
{
    public class ChaseState : State
    {
        private EnemyTank enemy;

        public ChaseState(StateMachine fsm, EnemyTank enemy) : base(fsm)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            Debug.Log("Entering Chase State");
            enemy.navMeshAgent.isStopped = false;
            enemy.navMeshAgent.SetDestination(enemy.player.position);
        }

        public override void Update()
        {
            if (enemy.distance > enemy.detectionRange)
            {
                fsm.ChangeState<PatrolState>();
            }
            if (enemy.distance < enemy.fireRange)
            {
                fsm.ChangeState<AttackState>();
            }
            
            enemy.navMeshAgent.SetDestination(enemy.player.position);
        }

        public override void Exit()
        {
            enemy.navMeshAgent.isStopped = true;
        }
    }
}