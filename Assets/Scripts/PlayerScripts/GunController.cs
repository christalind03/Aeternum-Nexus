using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    public float fireRate, range;
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

    private float _initialDamage;
    private Coroutine _activeDebuff;

    // Start is called before the first frame update
    void Start()
    {
        _initialDamage = damage;

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
            //Debug.Log("shot gun");
            FireGun();
        }
    }

    void FireGun()
    {
        isReadyToFire = false;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, range, whatIsEnemy))
        {
            //Debug.Log(raycastHit.collider.name);
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                //Debug.Log("Bullet Hit!");
                Invoke("ResetShot", fireRate); // calling the function below completely exits this FireGun function and i don't know why

                GameObject enemyObject = raycastHit.collider.gameObject;

                if (enemyObject.TryGetComponent(out EnemyShield enemyShield))
                {
                    Destroy(enemyShield);
                }
                else
                {
                    enemyObject.GetComponent<Health>().RemoveHealth(damage);
                }
            }
        }

        // Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        Invoke("ResetShot", fireRate);
    }
    void ResetShot()
    {
        isReadyToFire = true;
    }

    public void AttackDebuff(float debuffDuration, float debuffPercentage)
    {
        // Ensure we aren't overriding coroutines
        if (_activeDebuff != null)
        {
            StopCoroutine(_activeDebuff);
        }

        _activeDebuff = StartCoroutine(HandleDebuff(debuffDuration, debuffPercentage));
    }

    private IEnumerator HandleDebuff(float debuffDuration, float debuffPercentage)
    {
        if (damage == _initialDamage)
        {
            damage -= (damage * debuffPercentage);
        }

        yield return new WaitForSeconds(debuffDuration);

        damage = _initialDamage;
        _activeDebuff = null;
    }
}
