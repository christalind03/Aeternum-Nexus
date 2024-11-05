using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maximumHealth;
    [SerializeField] private float _currentHealth; // Temporarily serialize the field to ensure health-related functionalities work properly

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
    }

    public void RemoveHealth(float healthPoints)
    {
        _currentHealth -= healthPoints;

        // Zero means that the current object has no more health.
        if (_currentHealth <= 0)
        {
            Debug.Log($"{this.name} had 0HP remaining and died.");
            Destroy(gameObject);
        }
    }
}
