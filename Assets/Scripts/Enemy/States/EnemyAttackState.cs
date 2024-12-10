using System;
using UnityEngine;

[CreateAssetMenu(menuName = "BaseState/Enemy/AttackState")]
public class EnemyAttackState<EState>: EnemyState<EState> where EState : Enum
{
    public void Initalize(EnemyContext context, EState estate)
    {
        Context = context;
        base.Initialize(context, estate);
    }

    public override void EnterState()
    {
        GameObject playerObject = GameObject.Find("Player"); // Contains the Health script

        Context.Animator.SetTrigger("Attack");
        playerObject?.GetComponent<Health>().RemoveHealth(Context.AttackDamage);
    }

    public override void ExitState() { }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }

    public override void UpdateState() { }
}
