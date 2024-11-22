using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform _teleportTarget;
    [SerializeField] GameObject _playerObject;

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            _playerObject.transform.position = _teleportTarget.position;
            _playerObject.transform.rotation = _teleportTarget.rotation;
        }
    }
}
