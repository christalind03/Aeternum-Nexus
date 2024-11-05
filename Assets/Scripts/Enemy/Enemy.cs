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

    protected EnemyContext _context;
    protected bool _canAttack;

    private float _initialAttackDamage;

    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

        _fieldOfView = GetComponent<FieldOfView>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _context = new EnemyContext(_attackCooldown, _attackDamage, _initialPosition, _initialRotation, transform, _fieldOfView, _navMeshAgent);
        _canAttack = true;

        _initialAttackDamage = _attackDamage;

        InitializeStates();
    }

    private void InitializeStates()
    {
        foreach (StateEntry<EState> stateEntry in StateEntries)
        {
            EState stateEntryKey = stateEntry.Key;
            EnemyState<EState> stateEntryValue = (EnemyState<EState>)stateEntry.Value;

            stateEntryValue.Initialize(_context, stateEntryKey);
            States.Add(stateEntryKey, stateEntryValue);
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (_initialAttackDamage == _attackDamage && otherCollider.gameObject.TryGetComponent<SupportDevice>(out SupportDevice supportDevice))
        {
            _attackDamage += _attackDamage * supportDevice.Percentage;
            StartCoroutine(ResetAttackDamage(supportDevice.Duration));
        }
    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    private IEnumerator ResetAttackDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackDamage = _initialAttackDamage;
    }
}
