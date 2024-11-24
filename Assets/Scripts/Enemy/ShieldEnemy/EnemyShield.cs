using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Shield applied!");
    }

    private void OnDisable()
    {
        Debug.Log("Shield removed!");
    }
}
