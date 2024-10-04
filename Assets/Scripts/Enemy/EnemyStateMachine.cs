using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateMachine : StateManager<EnemyStateMachine.EEnemyState>
{
    public enum EEnemyState
    {
        Attack,
        Chase,
        Idle,
    }

    private EnemyContext _context;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _context = new EnemyContext(_navMeshAgent);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EEnemyState.Attack, new EnemyAttackState(_context, EEnemyState.Attack));
        States.Add(EEnemyState.Chase, new EnemyChaseState(_context, EEnemyState.Chase));
        States.Add(EEnemyState.Idle, new EnemyIdleState(_context, EEnemyState.Idle));

        CurrentState = States[EEnemyState.Attack];
    }
}
