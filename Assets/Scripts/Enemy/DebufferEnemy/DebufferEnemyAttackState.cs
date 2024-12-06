using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "BaseState/DebufferEnemy/AttackState")]
public class DebufferEnemyAttackState : EnemyAttackState<DebufferEnemy.EEnemyState>
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float MaxDistance;
    public float DebuffDuration;
    public float DebuffPercentage;

    private float _attackTimer = 0f;

    public override void Set(EnemyState<DebufferEnemy.EEnemyState> otherInstance)
    {
        if (otherInstance is DebufferEnemyAttackState otherState)
        {
            ProjectilePrefab = otherState.ProjectilePrefab;
            ProjectileSpeed = otherState.ProjectileSpeed;
            MaxDistance = otherState.MaxDistance;
            DebuffDuration = otherState.DebuffDuration;
            DebuffPercentage = otherState.DebuffPercentage;
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
            DebuffProjectile.CreateProjectile(ProjectilePrefab, Context.Transform.position, Context.Transform.rotation, Context.AttackDamage, ProjectileSpeed, MaxDistance, DebuffDuration, DebuffPercentage);
            _attackTimer = 0f;
        }
    }
}
