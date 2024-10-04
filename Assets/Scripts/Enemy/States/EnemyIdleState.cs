using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyContext context, EnemyStateMachine.EEnemyState estate) : base(context, estate)
    {
        EnemyContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering the idle state...");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting the idle state...");
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }

    public override void UpdateState()
    {
        Debug.Log("Updating the idle state...");
    }
}
