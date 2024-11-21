using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flee State", menuName = "BaseState/boss/FleeState")]
public class BossFleeState : EnemyFleeState<boss.EEnemyState> 
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _projectilePrefab2;
    [SerializeField] private GameObject _projectilePrefab3;
    [SerializeField] private GameObject _projectilePrefab4;
    [SerializeField] private GameObject _projectilePrefab5;
    [SerializeField] private GameObject _projectilePrefab6;
    [SerializeField] private GameObject _projectilePrefab7;
    [SerializeField] private GameObject _projectilePrefab8;

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
            string NR180 = "Fan_R180";
            string NL135 = "Fan_L135";
            string NL90 = "Fan_L90";
            string NL45 = "Fan_L45";
            string NR135 = "Fan_R135";
            string NR90 = "Fan_R90"; 
            string NR45 = "Fan_R45";
            string NR0 = "Fan_R0";

            Quaternion R0 = Context.Transform.rotation * Quaternion.Euler(0, 0, 0);
            Quaternion R45 = Context.Transform.rotation * Quaternion.Euler(0, 45, 0);
            Quaternion L45 = Context.Transform.rotation * Quaternion.Euler(0, -45, 0);
            Quaternion R90 = Context.Transform.rotation * Quaternion.Euler(0, 90, 0);
            Quaternion L90 = Context.Transform.rotation * Quaternion.Euler(0, -90, 0);
            Quaternion R135 = Context.Transform.rotation * Quaternion.Euler(0, 135, 0);
            Quaternion L135 = Context.Transform.rotation * Quaternion.Euler(0, -135, 0);
            Quaternion R180 = Context.Transform.rotation * Quaternion.Euler(0, 180, 0);
            Debug.Log("actually flee");

            fanProjectile.CreateProjectile(_projectilePrefab, Context.Transform.position, R0, Context.AttackDamage, _projectileSpeed, _maxDistance, NR0);
            fanProjectile.CreateProjectile(_projectilePrefab2, Context.Transform.position, R45, Context.AttackDamage, _projectileSpeed, _maxDistance, NR45);
            fanProjectile.CreateProjectile(_projectilePrefab3, Context.Transform.position, R90, Context.AttackDamage, _projectileSpeed, _maxDistance, NR90);
            fanProjectile.CreateProjectile(_projectilePrefab4, Context.Transform.position, R135, Context.AttackDamage, _projectileSpeed, _maxDistance,NR135);
            fanProjectile.CreateProjectile(_projectilePrefab5, Context.Transform.position, L45, Context.AttackDamage, _projectileSpeed, _maxDistance, NL45);
            fanProjectile.CreateProjectile(_projectilePrefab6, Context.Transform.position, L90, Context.AttackDamage, _projectileSpeed, _maxDistance, NL90);
            fanProjectile.CreateProjectile(_projectilePrefab7, Context.Transform.position, L135, Context.AttackDamage, _projectileSpeed, _maxDistance, NL135);
            fanProjectile.CreateProjectile(_projectilePrefab8, Context.Transform.position, R180, Context.AttackDamage, _projectileSpeed, _maxDistance, NR180);


            _attackTimer = 0f;
        }
    }
}

