using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FieldOfView), typeof(NavMeshAgent))]
public class Enemy<EState> : StateManager<EState> where EState : Enum
{
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
    protected Animator _animator;

    protected EnemyContext _context;
    protected bool _canAttack;

    protected virtual void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

        //Debug.Log($"{gameObject.name} initialized at {_initialPosition}");

        _fieldOfView = GetComponent<FieldOfView>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _context = new EnemyContext(_attackCooldown, _attackDamage, _initialPosition, _initialRotation, transform, _fieldOfView, _navMeshAgent, _animator);
        _canAttack = true;

        InitializeStates();
    }

    private void InitializeStates()
    {
        foreach (StateEntry<EState> stateEntry in StateEntries)
        {
            EState stateEntryKey = stateEntry.Key;
            EnemyState<EState> stateEntryValue = (EnemyState<EState>)stateEntry.Value;
            EnemyState<EState> personalInstance = (EnemyState<EState>)ScriptableObject.CreateInstance(stateEntryValue.GetType());

            personalInstance.Set(stateEntryValue);
            personalInstance.Initialize(_context, stateEntryKey);
            States.Add(stateEntryKey, personalInstance);
        }
    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}
