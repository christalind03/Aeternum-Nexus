using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing : MonoBehaviour
{
    public GameObject player;
    public float fanForce;
    Rigidbody playerBody;
    // Start is called before the first frame update

    //TEMP
    private Health playerHealth;
    void Start()
    {
        playerBody = player.GetComponent<Rigidbody>();
        playerHealth = player.GetComponent <Health>();
        if (playerHealth == null)
        {
            Debug.LogError("No Health component found on the player GameObject.");
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.name == "PlayerBody")
        {
            if (gameObject.name == "BigFan")
            {
                playerBody.AddForce(Vector3.up * fanForce, ForceMode.Force);
            }
            else
            {
                playerBody.AddForce(Vector3.forward * fanForce, ForceMode.Force);
            }
        }
    }
}
