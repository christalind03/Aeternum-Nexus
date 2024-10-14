using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FieldOfView), typeof(NavMeshAgent))]
public class Enemy : StateManager<Enemy.EEnemyState>
{
    public enum EEnemyState
    {
        Attack,
        Chase,
        Idle
    }

    [Header("Attack Parameters")]
    [SerializeField] protected float _attackCooldown;
    [SerializeField] protected float _attackDamage;
    [SerializeField] protected float _attackRange;

    [Header("Layer Masks")]
    [SerializeField] protected LayerMask _playerMask;

    protected Vector3 _initialPosition;
    protected Quaternion _initialRotation;

    protected FieldOfView _fieldOfView;
    protected NavMeshAgent _navMeshAgent;

    protected EnemyContext _context;
    protected bool _canAttack;

    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

        _fieldOfView = GetComponent<FieldOfView>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _context = new EnemyContext(_attackDamage, _initialPosition, _initialRotation, transform, _fieldOfView, _navMeshAgent);
        _canAttack = true;

        InitializeStates();
    }

    private void InitializeStates()
    {
        foreach (StateEntry<Enemy.EEnemyState> stateEntry in StateEntries)
        {
            Enemy.EEnemyState stateEntryKey = stateEntry.Key;
            BaseState<Enemy.EEnemyState> stateEntryValue = stateEntry.Value;

            var stateInstance = (EnemyState)ScriptableObject.CreateInstance(stateEntryValue.GetType());
            stateInstance.Initialize(_context, stateEntryKey);

            States.Add(stateEntryKey, stateInstance);
        }
    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}
