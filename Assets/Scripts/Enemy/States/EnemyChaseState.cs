using System;
using UnityEngine;

public class EnemyChaseState<EState> : EnemyState<EState> where EState : Enum
{
    public void Initalize(EnemyContext context, EState estate)
    {
        Context = context;
        base.Initialize(context, estate);
    }

    public override void EnterState()
    {
        if (Context.Animator != null)
        {
            Context.Animator.SetTrigger("Chase");
        }
    }

    public override void ExitState() { }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }

    public override void UpdateState()
    {
        // Since we need to index the DetectedObjects property, check to see if there's anything in it first
        if (Context.FieldOfView.DetectedObjects.Count > 0)
        {
            // Since this is a single-player game and we are only detecting for players, it's safe to assume that the 0th index will always be our player.
            Transform playerTransform = Context.FieldOfView.DetectedObjects[0].transform;

            // This is separated from state logic because we always want to look at the player if they're within the enemy's field of view.
            Context.Transform.LookAt(playerTransform.position);
            Context.NavMeshAgent.SetDestination(playerTransform.position);
        }
    }
}
