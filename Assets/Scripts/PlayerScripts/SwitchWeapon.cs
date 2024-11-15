using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchWeapon : MonoBehaviour
{
    [Header("References")]
    public GameObject meleeHolder;
    public GameObject gunHolder;
    public InputActionAsset playerControls;
    PlayerMovement playerMovement;
    InputAction switchWeapon;
    [HideInInspector] public bool isMeleeActive;

    // Start is called before the first frame update
    void Start()
    {
        switchWeapon = playerControls.FindActionMap("Combat").FindAction("Switch Weapon");
        switchWeapon.Enable();

        meleeHolder.SetActive(true);
        gunHolder.SetActive(false);

        isMeleeActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (switchWeapon.WasPressedThisFrame()) // switch between melee and gun when pressing "Q" or scrolling
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