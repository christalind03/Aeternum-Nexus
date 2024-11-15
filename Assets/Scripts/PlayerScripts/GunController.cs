using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    public float fireRate, timeBetweenShots, range;
    public bool allowButtonHold;

    public bool isShooting, isReadyToFire;

    [Header("References")]
    public Camera cam;
    public Transform attackPoint;
    public RaycastHit raycastHit;
    public LayerMask whatIsEnemy;
    public InputActionAsset playerControls;
    InputAction shootAction;

    [Header("Graphics")]
    public GameObject muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        isReadyToFire = true;
        shootAction = playerControls.FindActionMap("Combat").FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        GunInput();
    }

    void GunInput()
    {
        isShooting = allowButtonHold ? shootAction.IsPressed() : shootAction.WasPressedThisFrame();
        if (isReadyToFire && isShooting)
        {
            Debug.Log("shot gun");
            FireGun();
        }
    }

    void FireGun()
    {
        isReadyToFire = false;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, range, whatIsEnemy))
        {
            Debug.Log(raycastHit.collider.name);
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                // raycastHit.collider.GetComponent<ShootingAI>().TakeDamage(damage);
            }
        }

        // Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        Invoke("ResetShot", timeBetweenShots);
    }
    void ResetShot()
    {
        isReadyToFire = true;
    }
}
