using UnityEngine;

public class MeleeEnemyIdleState : MeleeEnemyState
{
    private readonly float _offsetAngle = 45f;
    private readonly float _rotationSpeed = 1.5f;

    private Quaternion _targetRotation;
    private bool _hasTargetRotation = false; 
    private bool _isReset = true;

    public MeleeEnemyIdleState(MeleeEnemyContext context, MeleeEnemy.EEnemyState estate) : base(context, estate)
    {
        MeleeEnemyContext Context = context;
    }

    public override void EnterState()
    {
        bool hasInitialPosition = Context.InitialPosition == Context.CurrentTransform.position;
        bool hasInitialRotation = Context.InitialRotation == Context.CurrentTransform.rotation;

        _isReset = hasInitialPosition && hasInitialRotation;

        Context.NavMeshAgent.SetDestination(Context.InitialPosition);
    }

    public override void ExitState() { }

    public override void OnTriggerEnter(Collider otherCollider) { }
    public override void OnTriggerExit(Collider otherCollider) { }
    public override void OnTriggerStay(Collider otherCollider) { }

    public override void UpdateState()
    {
        if (!_isReset)
        {
            // We would set this to be Context.InitialPosition == Context.CurrentTransform.position, but due to floating point values we have to calculate its distance instead.
            if (Vector3.Distance(Context.InitialPosition, Context.CurrentTransform.position) < 0.1f)
            {
                // Rotate the enemy back to its original rotation
                Context.CurrentTransform.rotation = Quaternion.Slerp(Context.CurrentTransform.rotation, Context.InitialRotation, _rotationSpeed * Time.deltaTime);
            }

            // We would set this to be Context.InitialRotation == Context.CurrentTransform.rotation, but due to floating point values we have to calculate its distance instead.
            if (Context.InitialRotation == Context.CurrentTransform.rotation)
            {
                _isReset = true;
            }
        }
        else
        {
            if (_hasTargetRotation)
            {
                // Rotate the enemy back to its original rotation
                Context.CurrentTransform.rotation = Quaternion.Slerp(Context.CurrentTransform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

                // We would set this to be _targetRotation == Context.CurrentTransform.rotation, but due to floating point values we have to calculate its distance instead.
                if (Quaternion.Angle(_targetRotation, Context.CurrentTransform.rotation) < 0.1f)
                {
                    _hasTargetRotation = false;
                }
            }
            else
            {
                Vector3 eulerAngle = Context.InitialRotation.eulerAngles;

                float targetAngle = Random.Range(eulerAngle.y - _offsetAngle, eulerAngle.y + _offsetAngle);
                _targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

                _hasTargetRotation = true;
            }
        }
    }
}
