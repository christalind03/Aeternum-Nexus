using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePush : MonoBehaviour
{
    public Transform center;
    public float radius = 15f;
    public float pushForce = 50f;

    public GameObject player;
    Rigidbody playerBody;
    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerBody = player.GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Log("activated");
         
    Vector3 directionToCenter = playerBody.position - center.position;
        Debug.Log(transform.position);
        Debug.Log(center.position);

        // Check if the object is inside the sphere
        if (directionToCenter.magnitude<radius)
        {
            Debug.Log("Object in sphere");
            // Push the object outward if it isnt null
            if (playerBody != null)
            {
                Debug.Log("Trying to push Object");
                
                Vector3 pushDirection = directionToCenter.normalized;
                Debug.Log(pushDirection);
                playerBody.AddForce(pushDirection* pushForce, ForceMode.Force);
            }
        }
    }
}
