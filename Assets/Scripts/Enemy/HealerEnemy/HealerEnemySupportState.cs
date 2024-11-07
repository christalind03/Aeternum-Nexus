using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Support State", menuName = "BaseState/HealerEnemy/SupportState")]
public class HealerSupportState : EnemyState<HealerEnemy.EEnemyState>
{
    [SerializeField] private float _supportRadius;
    [SerializeField] private LayerMask _supportLayers;

    private Collider[] _detectedColliders = new Collider[15];
    
    public void Initalize(EnemyContext context, HealerEnemy.EEnemyState estate)
    {
        Context = context;
        base.Initialize(context, estate);
    }

    public override void EnterState()
    {
        int _colliderCount = Physics.OverlapSphereNonAlloc(Context.Transform.position, _supportRadius, _detectedColliders, _supportLayers, QueryTriggerInteraction.Collide);

        for (int i = 0; i < _colliderCount; ++i)
        {
            GameObject detectedObject = _detectedColliders[i].gameObject;
        
            if (detectedObject.TryGetComponent<Health>(out Health allyHealth))
            {
                float healthDifference = allyHealth.MaximumHealth - allyHealth.CurrentHealth;
                allyHealth.AddHealth(healthDifference);
            }
        }
    }

    public override void ExitState() { }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }
    public override void UpdateState() { }
}
