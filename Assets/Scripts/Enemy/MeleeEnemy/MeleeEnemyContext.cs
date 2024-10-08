using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyContext
{
    public readonly float AttackDamage;
    public readonly Vector3 InitialPosition;
    public readonly Quaternion InitialRotation;
    public readonly Transform CurrentTransform;
    public readonly FieldOfView FieldOfView;
    public readonly NavMeshAgent NavMeshAgent;

    public MeleeEnemyContext(float attackDamage, Vector3 initialPosition, Quaternion initialRotation, Transform currentTransform, FieldOfView fieldOfView, NavMeshAgent navMeshAgent)
    {
        AttackDamage = attackDamage;
        InitialPosition = initialPosition;
        InitialRotation = initialRotation;
        CurrentTransform = currentTransform;
        FieldOfView = fieldOfView;
        NavMeshAgent = navMeshAgent;
    }
}
