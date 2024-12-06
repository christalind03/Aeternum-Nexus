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
    EquipAudio equipAudio;

    [HideInInspector] public bool isMeleeActive;

    // Start is called before the first frame update
    void Start()
    {
        switchWeapon = playerControls.FindActionMap("Combat").FindAction("Switch Weapon");
        switchWeapon.Enable();

        meleeHolder.SetActive(false);
        gunHolder.SetActive(true);

        isMeleeActive = false;
        equipAudio = GetComponent<EquipAudio>();
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
        equipAudio.PlayEquipAudio("sword", true);
    }

    void SwitchToGun()
    {
        meleeHolder.SetActive(false);
        gunHolder.SetActive(true);
        equipAudio.PlayEquipAudio("gun", true);
    }
}

// add animations