using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Support State", menuName = "BaseState/SupportEnemy/SupportState")]
public class SupportEnemySupportState : EnemyState<SupportEnemy.EEnemyState>
{
    [SerializeField] private float _supportDuration;
    [SerializeField] private float _supportPercentage;
    [SerializeField] private float _supportRadius;
    
    public void Initalize(EnemyContext context, SupportEnemy.EEnemyState estate)
    {
        Context = context;
        base.Initialize(context, estate);
    }

    public override void EnterState()
    {
        GameObject supportDevice = new GameObject("Support Device");
        supportDevice.transform.position = Context.Transform.position;

        SupportDevice supportType = supportDevice.AddComponent<SupportDevice>();
        supportType.Duration = _supportDuration;
        supportType.Percentage = _supportPercentage;

        // In order for the support device to work correctly, we need a collider and rigidbody for enemies to register triggers.
        SphereCollider supportCollider = supportDevice.AddComponent<SphereCollider>();
        supportCollider.isTrigger = true;
        supportCollider.radius = _supportRadius;

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
