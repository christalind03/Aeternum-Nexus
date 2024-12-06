using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffProjectile : MonoBehaviour
{
    public float _attackDamage;
    public float _projectileSpeed;
    public float _maxDistance;
    public float _debuffDuration;
    public float _debuffPercentage;

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    public static DebuffProjectile CreateProjectile(GameObject objPrefab, Vector3 objPosition, Quaternion objRotation, float attackDamage, float projectileSpeed, float maxDistance, float debuffDuration, float debuffPercentage)
    {
        GameObject projectileObj = Instantiate(objPrefab, objPosition, objRotation);

        if (!projectileObj.TryGetComponent<DebuffProjectile>(out DebuffProjectile projectileComponent))
        {
            projectileComponent = projectileObj.AddComponent<DebuffProjectile>();
        }

        projectileComponent._attackDamage = attackDamage;
        projectileComponent._projectileSpeed = projectileSpeed;
        projectileComponent._maxDistance = maxDistance;
        projectileComponent._debuffDuration = debuffDuration;
        projectileComponent._debuffPercentage = debuffPercentage;
        
        return projectileComponent;
    }

    private void Update()
    {
        transform.position += _projectileSpeed * Time.deltaTime * transform.forward;

        if (_maxDistance <= Vector3.Distance(transform.position, _initialPosition))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            GameObject playerObject = otherCollider.gameObject;

            // Remove the player's health
            if (playerObject.TryGetComponent(out Health playerHealth))
            {
                playerHealth.RemoveHealth(_attackDamage);
            }

            // In order to access the GunController and MeleeController, we'll have to manually traverse the Player GameObject hierarchy
            Transform weaponParent = playerObject.transform.root.GetChild(1).GetChild(0);
            
            GameObject meleeObject = weaponParent.GetChild(0).gameObject;

            if (meleeObject.activeSelf && meleeObject.TryGetComponent(out MeleeController meleeController))
            {
                meleeController.AttackDebuff(_debuffDuration, _debuffPercentage);
            }

            GameObject gunObject = weaponParent.GetChild(1).gameObject;

            if (gunObject.activeSelf && gunObject.TryGetComponent(out GunController gunController))
            {
                gunController.AttackDebuff(_debuffDuration, _debuffPercentage);
            }
        }

        Destroy(gameObject);
    }
}
