using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maximumHealth;
    [SerializeField] private float _currentHealth; // Temporarily serialize the field to ensure health-related functionalities work properly

    [SerializeField] private Image _HealthBarFIll;
    [SerializeField] private Image _HealthBarBack;

    [SerializeField] public bool canTakeDamage;

    public string sceneToReload;
    public float MaximumHealth { get { return _maximumHealth; } }
    public float CurrentHealth { get { return _currentHealth; } }
    PlayerAudio playerAudio;
    EnemyAudio enemyAudio;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    
    private void Start()
    {
        _currentHealth = _maximumHealth;

        if(gameObject.name == "boss1")
        {
            _HealthBarFIll.color = new Color32(1, 1, 1, 0);
            _HealthBarBack.color = new Color32(1, 1, 1, 0);
        }

        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("start bool:" + canTakeDamage);
        }

    }

    public void AddHealth(float healthPoints)
    {
        _currentHealth += healthPoints;

        if (_maximumHealth < _currentHealth)
        {
            _currentHealth = _maximumHealth;
        }
        UpdateHealthBar();
    }

    public void RemoveHealth(float healthPoints)
    {

        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Cantakedamagebool:" + canTakeDamage);

        }

        //_currentHealth -= healthPoints;
        if (gameObject.CompareTag("Player") && canTakeDamage == true)
        {
            playerAudio = GetComponent<PlayerAudio>();
            playerAudio.PlayPlayerAudio("hurt");
        }

        if (gameObject.CompareTag("Enemy"))
        {
            enemyAudio = GetComponent<EnemyAudio>();
            enemyAudio.PlayEnemyAudio("hurt");
            if (_animator != null)
            {
                //_animator.SetTrigger("Hit");
                StartCoroutine(HitEnemy());
            }

            _currentHealth -= healthPoints;
            //playerAudio.PlayPlayerAudio("hurt");

        }

        if (canTakeDamage == true)
        {
            _currentHealth -= healthPoints;
        }

        if (gameObject.name == "boss1")
        {
            float fillAmount = _currentHealth / _maximumHealth;
            //_HealthBarFIll.color = new Color32(254,0,0,255);
            //_HealthBarBack.color = new Color32(118, 0, 7, 255);

        }

        // Zero means that the current object has no more health.
        if (_currentHealth <= 0)
        {
            KillPlayer();
            return;
        }
        
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (gameObject.CompareTag("Player"))
        {
            float fillAmount = _currentHealth / _maximumHealth;
            _HealthBarFIll.fillAmount = fillAmount;
        }
        if (gameObject.name == "boss1")
        {
            float fillAmount = _currentHealth / _maximumHealth;
            _HealthBarFIll.fillAmount = fillAmount;
        }
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            KillPlayer(); //fall out of world
        }

        //Debug.Log(_currentHealth);
    }

    void KillPlayer()
    {
        if (gameObject.CompareTag("Player") || gameObject.name == "boss1" || gameObject.name == "Exit")
        {
            SceneManager.LoadScene(sceneToReload);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Enemy"))
        {
            if (_animator != null)
            {
                _animator.ResetTrigger("Attack");
                _animator.ResetTrigger("Hit");
                _animator.ResetTrigger("Idle");

                _animator.SetTrigger("Death");
            }
        }
    }
    private IEnumerator HitEnemy()
    {
        int stateHash = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

        _navMeshAgent.isStopped = true;
        _animator.SetTrigger("Hit");

        yield return new WaitForSeconds(1f); // Arbitrary number to allow the death animation to be played in full.
        
        _navMeshAgent.isStopped = false;
        _animator.Play(stateHash, 0);
    }
}
