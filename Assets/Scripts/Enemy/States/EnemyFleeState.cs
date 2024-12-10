using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFleeState<EState> : EnemyState<EState> where EState : Enum
{
    public float FleeDistance;
    public float FleeSpeed;

    private float _initialSpeed;

    public override void Set(EnemyState<EState> otherInstance)
    {
        if (otherInstance is EnemyFleeState<EState> otherState)
        {
            FleeDistance = otherState.FleeDistance;
            FleeSpeed = otherState.FleeSpeed;
        }
    }

    public void Initalize(EnemyContext context, EState estate)
    {
        Context = context;
        base.Initialize(context, estate);
    }

    public override void EnterState()
    {
        _initialSpeed = Context.NavMeshAgent.acceleration;
        Context.NavMeshAgent.acceleration = FleeSpeed;

        if (Context.Animator != null)
        {
            Context.Animator.SetTrigger("Flee");
        }
    }

    public override void ExitState()
    {
        Context.NavMeshAgent.acceleration = _initialSpeed;
    }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }

    public override void UpdateState()
    {
        // Calculate the direction opposite of the player
        Transform playerTransform = Context.FieldOfView.DetectedObjects[0].transform;

        Vector3 targetDirection = (Context.Transform.position - playerTransform.position).normalized;
        Vector3 targetPosition = Context.Transform.position + targetDirection * FleeDistance;

        Context.NavMeshAgent.SetDestination(targetPosition);
    }
}
