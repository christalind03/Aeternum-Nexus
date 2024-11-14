using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    [Header("References")]
    public GameObject meleeHolder;
    public GameObject gunHolder;
    bool isMeleeActive;

    // Start is called before the first frame update
    void Start()
    {
        meleeHolder.SetActive(true);
        gunHolder.SetActive(false);

        isMeleeActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch between melee and gun when pressing "1" or "2"
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (isMeleeActive)
            {
                SwitchToGun();
                isMeleeActive = false;
            }
            else
            {
                SwitchToMelee();
                isMeleeActive = true;
            }
        }
    }

    void SwitchToMelee()
    {
        meleeHolder.SetActive(true);
        gunHolder.SetActive(false);
    }

    void SwitchToGun()
    {
        meleeHolder.SetActive(false);
        gunHolder.SetActive(true);
    }
}

// add animations