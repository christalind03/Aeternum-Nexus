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
    private LayerMask _playerMask;

    private MeleeEnemyContext _context;

    private FieldOfView _fieldOfView;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _fieldOfView = GetComponent<FieldOfView>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _context = new MeleeEnemyContext(_navMeshAgent);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EEnemyState.Attack, new MeleeEnemyAttackState(_context, EEnemyState.Attack));
        States.Add(EEnemyState.Chase, new MeleeEnemyChaseState(_context, EEnemyState.Chase));
        States.Add(EEnemyState.Idle, new MeleeEnemyIdleState(_context, EEnemyState.Idle));

        CurrentState = States[EEnemyState.Attack];
    }

    private void FixedUpdate()
    {
        if (_fieldOfView.DetectedObjects.Count > 0)
        {
            // Since this is a single-player game and we are only detecting for players, it's safe to assume that the 0th index will always be our player.
            Transform playerTransform = _fieldOfView.DetectedObjects[0].transform;

            transform.LookAt(playerTransform.position);
            _navMeshAgent.SetDestination(playerTransform.position);
        }
    }
}
