using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemy : StateManager<MeleeEnemy.EEnemyState>
{
    public enum EEnemyState
    {
        Attack,
        Chase,
        Idle,
    }

    [Header("Attack Parameters")]
    [SerializeField]
    private float _attackCooldown;

    [SerializeField]
    private float _attackDamage;

    [SerializeField]
    private float _attackRange;

    [Header("Layer Masks")]
    [SerializeField]
    private LayerMask _playerMask;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private FieldOfView _fieldOfView;
    private NavMeshAgent _navMeshAgent;

    private MeleeEnemyContext _context;
    private bool _canAttack;

    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _fieldOfView = GetComponent<FieldOfView>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _context = new MeleeEnemyContext(_attackDamage, _initialPosition, _initialRotation, transform, _fieldOfView, _navMeshAgent);
        _canAttack = true;

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EEnemyState.Attack, new MeleeEnemyAttackState(_context, EEnemyState.Attack));
        States.Add(EEnemyState.Chase, new MeleeEnemyChaseState(_context, EEnemyState.Chase));
        States.Add(EEnemyState.Idle, new MeleeEnemyIdleState(_context, EEnemyState.Idle));

        CurrentState = States[EEnemyState.Idle];
    }

    private void FixedUpdate()
    {
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
                StartCoroutine(TriggerAttackCooldown());
            } else
            {
                TransitionToState(EEnemyState.Chase);
            }
        }
        else
        {
            TransitionToState(EEnemyState.Idle);
        }
    }

    private IEnumerator TriggerAttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}
