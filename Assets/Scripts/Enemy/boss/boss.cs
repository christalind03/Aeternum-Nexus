using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : Enemy<boss.EEnemyState>
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
        if (0 < _fieldOfView.DetectedObjects.Count)//if there are objects in view
        {
            Debug.Log("player detected");
            // Since this is a single-player game and we are only detecting for players, it's safe to assume that the 0th index will always be our player.
            //Transform playerTransform = _fieldOfView.DetectedObjects[0].transform;

            // This is separated from state logic because we always want to look at the player if they're within the enemy's field of view.
            //transform.LookAt(playerTransform.position);
            Vector3 fixedDirection = new Vector3(0, 0, 0);
            transform.LookAt(fixedDirection);

            if (Physics.CheckSphere(transform.position, _fleeRange, _playerMask))
            {
                Debug.Log("Flee State");

                transform.LookAt(fixedDirection);
                TransitionToState(EEnemyState.Flee);

            }
            else
            {
                Debug.Log("Attack1");
                TransitionToState(EEnemyState.Attack);
                Debug.Log("Attack1Done");
            }
        }
        else
        {
            TransitionToState(EEnemyState.Idle);
        }
    }
}