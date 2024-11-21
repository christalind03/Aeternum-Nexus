using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flee State", menuName = "BaseState/boss/FleeState")]
public class BossFleeState : EnemyFleeState<boss.EEnemyState> 
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

        if (Context.AttackCooldown <= _attackTimer)//if able to attack
        {
            SphereProjectile.CreateProjectile(_projectilePrefab, Context.Transform.position, Context.Transform.rotation, Context.AttackDamage, _projectileSpeed, _maxDistance);
            

            _attackTimer = 0f;
        }
    }
}

