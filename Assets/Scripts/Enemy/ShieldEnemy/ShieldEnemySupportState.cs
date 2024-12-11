using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Support State", menuName = "BaseState/ShieldEnemy/SupportState")]
public class ShieldEnemySupportState : EnemyState<ShieldEnemy.EEnemyState>
{
    public LayerMask AllyMask;
    public float SupportRadius;

    private Collider[] _detectedAllies = new Collider[25];

    public override void Set(EnemyState<ShieldEnemy.EEnemyState> otherInstance)
    {
        if (otherInstance is ShieldEnemySupportState otherState)
        {
            AllyMask = otherState.AllyMask;
            SupportRadius = otherState.SupportRadius;
        }
    }

    public void Initalize(EnemyContext context, ShieldEnemy.EEnemyState estate)
    {
        Context = context;
        base.Initialize(context, estate);
    }

    public override void EnterState()
    {
        if (Context.Animator != null)
        {
            Context.Animator.SetTrigger("Support");
        }

        int totalColliders = Physics.OverlapSphereNonAlloc(Context.Transform.position, SupportRadius, _detectedAllies, AllyMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < totalColliders; i++)
        {
            GameObject allyObject = _detectedAllies[i].gameObject;

            if (!allyObject.TryGetComponent(out EnemyShield enemyShield))
            {
                _detectedAllies[i].gameObject.AddComponent<EnemyShield>();
            }
        }
    }

    public override void ExitState() { }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }
    public override void UpdateState() { }
}
