using UnityEngine;

public class MeleeEnemyAttackState: MeleeEnemyState
{
    public MeleeEnemyAttackState(MeleeEnemyContext context, MeleeEnemy.EEnemyState estate) : base(context, estate)
    {
        MeleeEnemyContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering the attack state...");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting the attack state...");
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
        Debug.Log("Updating the attack state...");
    }
}
