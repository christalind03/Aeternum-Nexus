using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maximumHealth;
    [SerializeField] private float _currentHealth; // Temporarily serialize the field to ensure health-related functionalities work properly

    [SerializeField] private Image _HealthBarFIll;
    public string sceneToReload;
    public float MaximumHealth { get { return _maximumHealth; } }
    public float CurrentHealth { get { return _currentHealth; } }
    
    private void Start()
    {
        _currentHealth = _maximumHealth;
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
        _currentHealth -= healthPoints;

        // Zero means that the current object has no more health.
        if (_currentHealth <= 0)
        {
            Debug.Log($"{this.name} had 0HP remaining and died.");
            Destroy(gameObject);
            if (gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene(sceneToReload);
            }

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
        if (transform.position.y < -50)
        {
            KillPlayer();//fall out of world
        }
        //Debug.Log(_currentHealth);
    }

    void KillPlayer()
    {
        Debug.Log("dead");
        if (gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToReload);
        }
        if (gameObject.name=="boss1")
        {
            SceneManager.LoadScene(sceneToReload);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
