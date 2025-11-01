using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    public Transform player;
    
    [Header("Paramaters")]
    public float moveSpeed;
    public float rotateSpeed;
    public float detectionRange;
    public float fireRange;
    public float fireRate;
    public float turretRotationSpeed;

    private float fireTimer;

    [Header("References")] 
    public Transform turret;
    public Transform firePoint;
    public GameObject missile;

    private EnemyHealth _enemyHealth;
    
    private enum State {Idle, Chase, Attack, Dead }
    private State currentState = State.Idle;
    
    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        _enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (_enemyHealth == null || _enemyHealth.isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);
        print(currentState);
        switch (currentState)
        {
            case State.Idle:
                if (distance < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;
            
            case State.Chase:
                MoveTowards(player.position);
                RotateTowards(player.position);
                if (distance < fireRange)
                {
                    currentState = State.Attack;
                }

                if (distance > detectionRange)
                {
                    currentState = State.Idle;
                }
                break;
            
            case State.Attack:
                RotateTurretTowards(player.position);
                Shoot();
                if (distance > fireRange * 1.2f)
                {
                    currentState = State.Chase;
                }
                break;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    
    private void RotateTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        lookRot *= Quaternion.Euler(0, 90, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
    }

    private void RotateTurretTowards(Vector3 target)
    {
        Vector3 dir = (target - turret.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        lookRot *= Quaternion.Euler(0, 90, 0);
        turret.rotation = Quaternion.Lerp(turret.rotation, lookRot, Time.deltaTime * turretRotationSpeed);
    }

    private void Shoot()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0;
            Vector3 direction = firePoint.position - transform.position;
            GameObject missilePrefab = Instantiate(missile, firePoint.position, firePoint.rotation); 
            missilePrefab.GetComponent<Missile>().Launch(direction);
            missilePrefab.transform.rotation = Quaternion.Euler(0,180,0);
        }
    }
}
