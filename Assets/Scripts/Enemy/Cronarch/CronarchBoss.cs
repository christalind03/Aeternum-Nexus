using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CronarchBoss : Enemy<CronarchBoss.EEnemyState>
{
    public enum EEnemyState
    {
        Attack,
        Chase,
        Idle
    }

    [Header("Boss Parameters")]
    [Tooltip("The value in which the self-heal occurs")]
    [SerializeField] private float _selfHeal;
    [SerializeField] private float _clonesCooldown;
    [SerializeField] private Transform[] _cloneTransforms = new Transform[5];
    [SerializeField] private GameObject _clonePrefab;

    private bool _hasHealed;
    private bool _clonesEnabled;
    private GameObject[] _currentClones = new GameObject[5];

    private Collider _collider;
    private Health _health;
    private MeshRenderer _meshRenderer;

    private float _clonesTimer;

    protected override void Awake()
    {
        base.Awake();

        _clonesTimer = 0;
        _collider = GetComponent<Collider>();
        _health = GetComponent<Health>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        if (!_hasHealed && _health.CurrentHealth <= _selfHeal)
        {
            _health.AddHealth(_health.MaximumHealth - _health.CurrentHealth);
            _hasHealed = true;
        }

        if (_clonesEnabled)
        {
            if (_currentClones.All(thisClone => thisClone == null))
            {
                // Activate the object used to render the enemy
                transform.GetChild(0).gameObject.SetActive(true);

                _collider.enabled = true;
                _navMeshAgent.enabled = true;
                _fieldOfView.enabled = true;

                _clonesEnabled = false;

                _clonesTimer = _clonesCooldown;
            }
        }
        else
        {
            if (0 < _fieldOfView.DetectedObjects.Count)
            {
                if (!_clonesEnabled && _clonesTimer == 0)
                {
                    EnableClones();
                }

                if (!_clonesEnabled)
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
            }
            else
            {
                TransitionToState(EEnemyState.Idle);
            }
        }

        if (0 < _clonesCooldown)
        {
            _clonesCooldown -= Time.deltaTime;
        }
    }

    private void EnableClones()
    {
        // Disable the object used to render the enemy
        transform.GetChild(0).gameObject.SetActive(false);

        _collider.enabled = false;
        _navMeshAgent.enabled = false;
        _fieldOfView.enabled = false;
        
        _clonesEnabled = true;

        for (int i = 0; i < _currentClones.Length; i++)
        {
            _currentClones[i] = Instantiate(_clonePrefab, _cloneTransforms[i].position, _cloneTransforms[i].rotation);
        }
    }
}
