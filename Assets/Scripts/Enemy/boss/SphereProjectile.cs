using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjectile : MonoBehaviour
{
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _maxDistance;

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    public static SphereProjectile CreateProjectile(GameObject objPrefab, Vector3 objPosition, Quaternion objRotation, float attackDamage, float projectileSpeed, float maxDistance)
    {
        GameObject projectileObj = Instantiate(objPrefab, objPosition, objRotation);

        if (!projectileObj.TryGetComponent<SphereProjectile>(out SphereProjectile projectileComponent))
        {
            projectileComponent = projectileObj.AddComponent<SphereProjectile>();
        }

        projectileComponent._attackDamage = attackDamage;
        projectileComponent._projectileSpeed = projectileSpeed;
        projectileComponent._maxDistance = maxDistance;

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

    /*private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            otherCollider.gameObject.GetComponent<Health>().RemoveHealth(_attackDamage);
        }

        Destroy(gameObject);
    }
    */
}

