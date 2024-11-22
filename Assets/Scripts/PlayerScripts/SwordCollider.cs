using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    public GameObject melee;
    MeleeController meleeController;

    // Start is called before the first frame update
    void Start()
    {
        meleeController = melee.GetComponent<MeleeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && meleeController.isAttacking) // hits enemy and is actively attacking
        {
            Health enemyHealth = other.GetComponent<Health>();
            Debug.Log("Sword Hit!");

            enemyHealth.RemoveHealth(meleeController.damage);
        }
    }
}
