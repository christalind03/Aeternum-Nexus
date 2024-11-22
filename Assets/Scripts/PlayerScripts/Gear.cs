using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing2 : MonoBehaviour
{
    public float spinSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed);
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("IN");
        // Check if the player has entered the platform's trigger area
        if (collision.gameObject.name == "Player")
        {
            // Parent the player to the platform to make them rotate with it
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the player has exited the platform's trigger area
        Debug.Log("OUT");
        if (collision.gameObject.name == "Player")
        {
            // Unparent the player from the platform so they don't rotate with it anymore
            collision.transform.SetParent(null);
        }
    }
}
