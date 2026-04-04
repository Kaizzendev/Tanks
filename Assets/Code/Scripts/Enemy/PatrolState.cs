using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class PatrolState : State
    {
        private int currentIndex = 0;
        private bool waiting;
        private float waitingTime;
        private EnemyTank enemy;
        
        public PatrolState(StateMachine fsm, EnemyTank enemy) : base(fsm)
        {
            this.enemy = enemy;
        }
        
        public override void Enter()
        {
            currentIndex = 0;
            waiting = false;
            MoveToNextPoint();
        }
        
        public override void Update()
        {
            if (enemy.player != null)
            {
                if (enemy.distance < enemy.detectionRange)
                {
                    fsm.ChangeState<ChaseState>();
                }
            }

            if (waiting)
            {
                waitingTime += Time.deltaTime;
                if (waitingTime >= enemy.waitTimeAtPoint)
                {
                    waiting = false;
                    MoveToNextPoint();
                }
            }

            if (!enemy.navMeshAgent.pathPending && enemy.navMeshAgent.remainingDistance < 0.5f)
            {
                StartWaiting();
            }
        }

        public override void Exit()
        {
            enemy.navMeshAgent.isStopped = true; 
            waiting = false;               
            waitingTime = 0f;
        }

        private void StartWaiting()
        {
            waiting = true;
            waitingTime = 0f;
            enemy.navMeshAgent.isStopped = true;
        }

        private void MoveToNextPoint()
        {
            if (enemy.patrolPoints.Length == 0) return;

            enemy.navMeshAgent.isStopped = false;
            enemy.navMeshAgent.SetDestination(enemy.patrolPoints[currentIndex].position);
            currentIndex = (currentIndex + 1) % enemy.patrolPoints.Length; // recorre en bucle
        }
    }
}