using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flee State", menuName = "BaseState/boss/FleeState")]
public class BossFleeState : EnemyFleeState<boss.EEnemyState> 
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float MaxDistance;

    private float _attackTimer = 0f;

    public override void Set(EnemyState<boss.EEnemyState> otherInstance)
    {
        if (otherInstance is BossFleeState otherState)
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

        if (Context.AttackCooldown <= _attackTimer)//if able to attack
        {
            SphereProjectile.CreateProjectile(ProjectilePrefab, Context.Transform.position, Context.Transform.rotation, Context.AttackDamage, ProjectileSpeed, MaxDistance);
            

            _attackTimer = 0f;
        }
    }
}

