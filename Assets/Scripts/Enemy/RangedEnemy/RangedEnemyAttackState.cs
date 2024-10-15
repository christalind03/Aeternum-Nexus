using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "BaseState/RangedEnemy/AttackState")]
public class RangedEnemyAttackState : EnemyAttackState<RangedEnemy.EEnemyState>
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
            Projectile.CreateProjectile(_projectilePrefab, Context.Transform.position, Context.Transform.rotation, Context.AttackDamage, _projectileSpeed, _maxDistance);
            _attackTimer = 0f;
        }
    }
}
