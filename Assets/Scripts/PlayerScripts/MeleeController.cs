using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MeleeController : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public GameObject sword;
    public GameObject player;
    public LayerMask whatIsEnemy;
    WeaponAudio weaponAudio;
    public InputActionAsset playerControls;
    InputAction swingAction;

    [Header("Conditions")]
    public bool canAttack = true;
    public bool isAttacking = false;

    [Header("Modifiers")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float damage = 15;
    // public AudioClip attackSound;

    private RaycastHit raycastHit;
    private float _initialDamage;
    private Coroutine _activeDebuff;

    // Start is called before the first frame update
    void Start()
    {
        _initialDamage = damage;

        weaponAudio = GetComponent<WeaponAudio>();


        swingAction = playerControls.FindActionMap("Combat").FindAction("Attack");
        swingAction.Enable(); // ????
    }


    // Update is called once per frame
    void Update()
    {
        if (swingAction.WasPressedThisFrame() && canAttack)
        {
            SwordAttack();
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, attackRange, whatIsEnemy))
            {
                //Debug.Log(raycastHit.collider.name);
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    //Debug.Log("Bullet Hit!");
                    //Invoke("ResetShot", fireRate); // calling the function below completely exits this FireGun function and i don't know why
                    //raycastHit.collider.GetComponent<Health>().RemoveHealth(damage);
                    //SwordAttack();
                    //GameObject enemyObject = raycastHit.collider.gameObject;
                    GameObject enemyObject = raycastHit.collider.gameObject;

                    if (enemyObject.TryGetComponent(out EnemyShield enemyShield))
                    {
                        Destroy(enemyShield);
                    }
                    else
                    {
                        enemyObject.GetComponent<Health>().RemoveHealth(damage);
                    }
                    raycastHit.collider.gameObject.GetComponent<Health>().RemoveHealth(damage);
                    StartCoroutine(PlayHitAudioWithDelay(0.25f));

                }
            }
        }
    }
    
    IEnumerator PlayHitAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        weaponAudio.PlayWeaponAudio("meleeHit");
    }

    void OnEnable()
    {
        canAttack = true;
    }

    public void SwordAttack()
    {
        isAttacking = true;
        canAttack = false; // currently attacking
        Animator animatorObj = sword.GetComponent<Animator>();
        animatorObj.SetTrigger("Attack");
        weaponAudio.PlayWeaponAudio("meleeSwing");

        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttack());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
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
