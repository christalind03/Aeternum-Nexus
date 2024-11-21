using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing : MonoBehaviour
{
    public GameObject player;
    public float fanForce;
    Rigidbody playerBody;
    Vector3 direction;
    // Start is called before the first frame update

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerBody = player.GetComponent<Rigidbody>();


        /*Quaternion R45 = Context.Transform.rotation * Quaternion.Euler(0, 0, 0);
        Quaternion R45 = Context.Transform.rotation * Quaternion.Euler(0, 45, 0);
        Quaternion L45 = Context.Transform.rotation * Quaternion.Euler(0, -45, 0);
        Quaternion R90 = Context.Transform.rotation * Quaternion.Euler(0, 90, 0);
        Quaternion L90 = Context.Transform.rotation * Quaternion.Euler(0, -90, 0);
        Quaternion R135 = Context.Transform.rotation * Quaternion.Euler(0, 135, 0);
        Quaternion L135 = Context.Transform.rotation * Quaternion.Euler(0, -135, 0);
        Quaternion R180 = Context.Transform.rotation * Quaternion.Euler(0, 180, 0);

        R45 = new Vector3(1, 1, 0).normalized; //45 between x and y
        L45 = new Vector3(1, -1, 0).normalized; //45 between x and -y
        R0 = new Vector3(1, 0, 0).normalized; //x
        L45 = new Vector3(1, -1, 0).normalized; //90 between x and -y
        R45 = new Vector3(1, 1, 0).normalized; //90 between x and y
        L45 = new Vector3(1, -1, 0).normalized; //45 between x and -y
        */

        
    }

    void OnTriggerStay(Collider collision)
    {
        string fanName = gameObject.name;
        Debug.Log(gameObject.name);

        switch (fanName)
        {
            case "Fan_R0":
                direction = new Vector3(1, 0, 0).normalized; // foreward
                
                break;
            case "Fan_R45":
                direction = new Vector3(1, 0, 1).normalized; // 45° between x and y
                break;

            case "Fan_L45":
                direction = new Vector3(1, 0, -1).normalized; // 45° between x and -y
                break;

            case "Fan_R90":
                direction = new Vector3(0, 0, 1).normalized; // 90° to the right
                break;

            case "Fan_L90":
                direction = new Vector3(0, 0, -1).normalized; // 90° to the left
                break;

            case "Fan_R135":
                direction = new Vector3(-1, 0, 1).normalized; // 135° to the right
                break;

            case "Fan_L135":
                direction = new Vector3(-1, 0, -1).normalized; // 135° to the left
                break;

            case "Fan_R180":
                direction = new Vector3(-1, 0, 0).normalized; // 180° (opposite x)
                break;
            default:
                direction = new Vector3(0, 0, 1).normalized;
                Debug.LogWarning("Fan direction of: " + fanName + " is not set");
                break;
        }
        if (collision.name == "PlayerBody")
        {
            Debug.Log("Collison detected");
            if (gameObject.name == "BigFan")
            {
                playerBody.AddForce(Vector3.up * fanForce, ForceMode.Force);
            }
            else
            {
                Debug.Log(direction);
                Debug.Log("Adding x force");
                playerBody.AddForce(direction * fanForce, ForceMode.Force);
            }
        }
    }
}
