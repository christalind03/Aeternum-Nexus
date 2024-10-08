using UnityEngine;

public class MeleeEnemyIdleState : MeleeEnemyState
{
    public MeleeEnemyIdleState(MeleeEnemyContext context, MeleeEnemy.EEnemyState estate) : base(context, estate)
    {
        MeleeEnemyContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering the idle state...");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting the idle state...");
    }

    public override MeleeEnemy.EEnemyState GetNextState()
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
