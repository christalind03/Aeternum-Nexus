using UnityEngine;

public class MeleeEnemyAttackState: MeleeEnemyState
{
    public MeleeEnemyAttackState(MeleeEnemyContext context, MeleeEnemy.EEnemyState estate) : base(context, estate)
    {
        MeleeEnemyContext Context = context;
    }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }

    public override void UpdateState()
    {
        Debug.Log($"Dealt {Context.AttackDamage} damage!");
        //Debug.Log("Updating the attack state...");
    }
}
