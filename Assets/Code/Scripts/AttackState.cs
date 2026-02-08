using UnityEngine;
namespace Player
{
    public class AttackState: State
    {
        private EnemyTank enemy;
        
        public AttackState(StateMachine fsm, EnemyTank enemy) : base(fsm)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            Debug.Log("Entering Attack State");
            enemy.navMeshAgent.isStopped = false;
            enemy.navMeshAgent.SetDestination(enemy.player.position);
        }

        public override void Update()
        {
            RotateTurretTowards(enemy.player.position);
            Shoot();
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
            if (distance > enemy.fireRange * 1.2f)
            {
                fsm.ChangeState<ChaseState>();
            }
        }

        public override void Exit()
        {
            enemy.navMeshAgent.isStopped = true;
        }

        private void RotateTurretTowards(Vector3 target)
        {
            Vector3 dir = (target - enemy.turret.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            lookRot *= Quaternion.Euler(0, 90, 0);
            enemy.turret.rotation = Quaternion.Lerp(enemy.turret.rotation, lookRot, Time.deltaTime * enemy.turretRotationSpeed);
        }

        private void Shoot()
        {
            enemy.fireTimer += Time.deltaTime;
            if (enemy.fireTimer >= enemy.fireRate)
            {
                enemy.fireTimer = 0;
                Vector3 direction = enemy.firePoint.position - enemy.turret.transform.position;
                GameObject missilePrefab = GameObject.Instantiate(enemy.missile, enemy.firePoint.position, enemy.firePoint.rotation);
                missilePrefab.GetComponent<Missile>().Launch(direction);
                missilePrefab.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        
    }
}