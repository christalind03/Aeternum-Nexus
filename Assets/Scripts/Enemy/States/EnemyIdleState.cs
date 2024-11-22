using System;
using UnityEngine;

[System.Serializable]
public class EnemyIdleState<EState> : EnemyState<EState> where EState : Enum
{
    public float OffsetAngle;
    public float RotationSpeed;

    private Quaternion _targetRotation;
    private bool _hasTargetRotation; 
    private bool _isReset;

    public override void Set(EnemyState<EState> otherInstance)
    {
        if (otherInstance is EnemyIdleState<EState> otherState)
        {
            OffsetAngle = otherState.OffsetAngle;
            RotationSpeed = otherState.RotationSpeed;
        }
    }
    public void Initalize(EnemyContext context, EState estate)
    {
        Context = context;
        base.Initialize(context, estate);

        _hasTargetRotation = false;
        _isReset = true;
    }

    public override void EnterState()
    {
        bool hasInitialPosition = Context.InitialPosition == Context.Transform.position;
        bool hasInitialRotation = Context.InitialRotation == Context.Transform.rotation;

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
            if (Vector3.Distance(Context.InitialPosition, Context.Transform.position) < 0.1f)
            {
                // Rotate the enemy back to its original rotation
                Context.Transform.rotation = Quaternion.Slerp(Context.Transform.rotation, Context.InitialRotation, RotationSpeed * Time.deltaTime);
            }

            // We would set this to be Context.InitialRotation == Context.CurrentTransform.rotation, but due to floating point values we have to calculate its distance instead.
            if (Context.InitialRotation == Context.Transform.rotation)
            {
                _isReset = true;
            }
        }
        else
        {
            if (_hasTargetRotation)
            {
                // Rotate the enemy back to its original rotation
                Context.Transform.rotation = Quaternion.Slerp(Context.Transform.rotation, _targetRotation, RotationSpeed * Time.deltaTime);

                // We would set this to be _targetRotation == Context.CurrentTransform.rotation, but due to floating point values we have to calculate its distance instead.
                if (Quaternion.Angle(_targetRotation, Context.Transform.rotation) < 0.1f)
                {
                    _hasTargetRotation = false;
                }
            }
            else
            {
                Vector3 eulerAngle = Context.InitialRotation.eulerAngles;

                float targetAngle = UnityEngine.Random.Range(eulerAngle.y - OffsetAngle, eulerAngle.y + OffsetAngle);
                _targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

                _hasTargetRotation = true;
            }
        }
    }
}
