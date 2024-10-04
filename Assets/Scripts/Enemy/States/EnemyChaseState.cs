using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyContext context, EnemyStateMachine.EEnemyState estate) : base(context, estate)
    {
        EnemyContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering the chase state...");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting the chase state...");
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
        Debug.Log("Updating the chase state...");
    }
}
