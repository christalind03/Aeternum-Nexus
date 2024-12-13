using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnDisable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
