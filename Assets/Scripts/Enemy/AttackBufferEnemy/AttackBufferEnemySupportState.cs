using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Support State", menuName = "BaseState/AttackBufferEnemy/SupportState")]
public class AttackBufferEnemySupportState : EnemyState<AttackBufferEnemy.EEnemyState>
{
    public float SupportDuration;
    public float SupportPercentage;
    public float SupportRadius;

    public override void Set(EnemyState<AttackBufferEnemy.EEnemyState> otherInstance)
    {
        if (otherInstance is AttackBufferEnemySupportState otherState)
        {
            SupportDuration = otherState.SupportDuration;
            SupportPercentage = otherState.SupportPercentage;
            SupportRadius = otherState.SupportRadius;
        }
    }

    public void Initalize(EnemyContext context, AttackBufferEnemy.EEnemyState estate)
    {
        Context = context;
        base.Initialize(context, estate);
    }

    public override void EnterState()
    {
        GameObject supportDevice = new GameObject("Support Device");
        supportDevice.transform.position = Context.Transform.position;

        AttackBufferDevice attackBufferDevice = supportDevice.AddComponent<AttackBufferDevice>();
        attackBufferDevice.Duration = SupportDuration;
        attackBufferDevice.Percentage = SupportPercentage;

        // In order for the support device to work correctly, we need a collider and rigidbody for enemies to register triggers.
        SphereCollider supportCollider = supportDevice.AddComponent<SphereCollider>();
        supportCollider.isTrigger = true;
        supportCollider.radius = SupportRadius;

        Rigidbody supportRigidbody = supportDevice.AddComponent<Rigidbody>();
        supportRigidbody.isKinematic = true;

        // Immediately destroy the object since we don't want it to be in the scene forever.
        Destroy(supportDevice);
    }

    public override void ExitState() { }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }
    public override void UpdateState() { }
}
