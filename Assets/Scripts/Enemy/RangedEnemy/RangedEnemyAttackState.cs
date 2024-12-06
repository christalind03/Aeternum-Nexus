using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "BaseState/RangedEnemy/AttackState")]
public class RangedEnemyAttackState : EnemyAttackState<RangedEnemy.EEnemyState>
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float MaxDistance;

    private float _attackTimer = 0f;

    public override void Set(EnemyState<RangedEnemy.EEnemyState> otherInstance)
    {
        if (otherInstance is RangedEnemyAttackState otherState)
        {
            ProjectilePrefab = otherState.ProjectilePrefab;
            ProjectileSpeed = otherState.ProjectileSpeed;
            MaxDistance = otherState.MaxDistance;
        }
    }

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
            Projectile.CreateProjectile(ProjectilePrefab, Context.Transform.position, Context.Transform.rotation, Context.AttackDamage, ProjectileSpeed, MaxDistance);
            _attackTimer = 0f;
        }
    }
}
