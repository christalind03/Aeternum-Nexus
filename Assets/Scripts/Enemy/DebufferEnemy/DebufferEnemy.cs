using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebufferEnemy : Enemy<DebufferEnemy.EEnemyState>
{
    public enum EEnemyState
    {
        Attack,
        Flee,
        Idle,
    }

    [Header("Flee Properties")]
    [SerializeField] private float _fleeRange;

    private void FixedUpdate()
    {
        if (0 < _fieldOfView.DetectedObjects.Count)
        {
            // Since this is a single-player game and we are only detecting for players, it's safe to assume that the 0th index will always be our player.
            Transform playerTransform = _fieldOfView.DetectedObjects[0].transform;

            // This is separated from state logic because we always want to look at the player if they're within the enemy's field of view.
            transform.LookAt(playerTransform.position);

            if (Physics.CheckSphere(transform.position, _fleeRange, _playerMask))
            {
                TransitionToState(EEnemyState.Flee);
            }
            else
            {
                TransitionToState(EEnemyState.Attack);
            }
        }
        else
        {
            TransitionToState(EEnemyState.Idle);
        }
    }
}
