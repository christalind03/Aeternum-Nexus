using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : Enemy<HealerEnemy.EEnemyState>
{
    public enum EEnemyState
    {
        Attack,
        Chase,
        Idle,
        Support,
    }

    [Header("Support Parameters")]
    [SerializeField] private float _supportCooldown;
    private bool _canSupport = true;

    private void FixedUpdate()
    {
        if (_canSupport)
        {
            StartCoroutine(SupportAllies());
        }

        if (0 < _fieldOfView.DetectedObjects.Count)
        {
            // Since this is a single-player game and we are only detecting for players, it's safe to assume that the 0th index will always be our player.
            Transform playerTransform = _fieldOfView.DetectedObjects[0].transform;

            // This is separated from state logic because we always want to look at the player if they're within the enemy's field of view.
            transform.LookAt(playerTransform.position);

            if (_canAttack && CurrentState.StateKey.Equals(EEnemyState.Chase) && Physics.CheckSphere(transform.position, _attackRange, _playerMask))
            {
                TransitionToState(EEnemyState.Attack);

                _canAttack = false;
                StartCoroutine(AttackCooldown());
            }
            else
            {
                TransitionToState(EEnemyState.Chase);
            }
        }
        else
        {
            TransitionToState(EEnemyState.Idle);
        }
    }

    private IEnumerator SupportAllies()
    {
        _canSupport = false;
        TransitionToState(EEnemyState.Support);

        yield return new WaitForSeconds(_supportCooldown);

        _canSupport = true;
    }
}
