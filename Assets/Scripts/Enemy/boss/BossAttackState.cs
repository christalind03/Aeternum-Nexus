using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "BaseState/boss/AttackState")]
public class BossAttackState : EnemyAttackState<boss.EEnemyState>
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _maxDistance;
    

    private float _attackTimer = 0f;

    public override void EnterState()
    {
        // Immediately attack the player after entering the attack state
        _attackTimer = Context.AttackCooldown;

    }

    public override void UpdateState()
    {
        _attackTimer += Time.deltaTime;

        if (Context.AttackCooldown <= _attackTimer)
        {
            Quaternion R45 = Context.Transform.rotation * Quaternion.Euler(0, 45, 0);
            Quaternion L45 = Context.Transform.rotation * Quaternion.Euler(0, -45, 0);
            Quaternion R90 = Context.Transform.rotation * Quaternion.Euler(0, 90, 0);
            Quaternion L90 = Context.Transform.rotation * Quaternion.Euler(0, -90, 0);
            Quaternion R135 = Context.Transform.rotation * Quaternion.Euler(0, 135, 0);
            Quaternion L135 = Context.Transform.rotation * Quaternion.Euler(0, -135, 0);
            Quaternion R180 = Context.Transform.rotation * Quaternion.Euler(0, 180, 0);
            

            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, Context.Transform.rotation, Context.AttackDamage, _projectileSpeed, _maxDistance);
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, R45, Context.AttackDamage, _projectileSpeed, _maxDistance);
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, R90, Context.AttackDamage, _projectileSpeed, _maxDistance);
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, R135, Context.AttackDamage, _projectileSpeed, _maxDistance);
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, L45, Context.AttackDamage, _projectileSpeed, _maxDistance);
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, L90, Context.AttackDamage, _projectileSpeed, _maxDistance);
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, L135, Context.AttackDamage, _projectileSpeed, _maxDistance);
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, R180, Context.AttackDamage, _projectileSpeed, _maxDistance);


            _attackTimer = 0f;
        }
    }
}
