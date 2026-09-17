using System;
using DefaultNamespace;
using UnityEngine;

namespace Enemy
{
    public class AttackController: EnemyControllerBase
    {
        
        [Header("Stats")]
        [SerializeField] private EnemyStats _enemyStats;
        
        internal void Attack(Vector3 target)
        {
            RotateTurretTowards(target);
            Shoot();
        }
        
        private void RotateTurretTowards(Vector3 target)
        {
            Vector3 dir = (target - _enemyStats.turret.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            lookRot *= Quaternion.Euler(0, 90, 0);
            _enemyStats.turret.rotation = Quaternion.Lerp(_enemyStats.turret.rotation, lookRot, Time.deltaTime * _enemyStats.turretRotationSpeed);
        }
        
        private void Shoot()
        {
            _enemyStats.fireTimer += Time.deltaTime;
            if (_enemyStats.fireTimer >= _enemyStats.fireRate)
            {
                _enemyStats.fireTimer = 0;
                Vector3 direction = _enemyStats.firePoint.position - _enemyStats.turret.transform.position;
                GameObject missilePrefab = Instantiate(_enemyStats.missile, _enemyStats.firePoint.position, _enemyStats.firePoint.rotation);
                missilePrefab.GetComponent<Missile>().Launch(direction);
                missilePrefab.GetComponent<Missile>().team = EnumTeam.Enemy;
            }
        }
    }
}