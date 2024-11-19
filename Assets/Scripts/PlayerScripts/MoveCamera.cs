using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// moves the cameraHolder to the position of the player
public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition; // gameobject which indicates where the camera should be

    // Update is called once per frame
    void Update()
    {
        if(cameraPosition != null)
        {
            transform.position = cameraPosition.position; // moves cameraHolder to its intended position
        }
        else
        {
            Debug.LogWarning("Character is deleted");
        }
        
    }
}
