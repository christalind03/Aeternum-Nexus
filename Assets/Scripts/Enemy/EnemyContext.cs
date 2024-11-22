using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public readonly float AttackCooldown;
    public readonly float AttackDamage;

    public readonly Vector3 InitialPosition;
    public readonly Quaternion InitialRotation;
    public readonly Transform Transform;

    public readonly FieldOfView FieldOfView;
    public readonly NavMeshAgent NavMeshAgent;

    public EnemyContext(float attackCooldown, float attackDamage, Vector3 initialPosition, Quaternion initialRotation, Transform transform, FieldOfView fieldOfView, NavMeshAgent navMeshAgent)
    {
        AttackCooldown = attackCooldown;
        AttackDamage = attackDamage;
        
        InitialPosition = initialPosition;
        InitialRotation = initialRotation;
        Transform = transform;

        FieldOfView = fieldOfView;
        NavMeshAgent = navMeshAgent;
    }
}
