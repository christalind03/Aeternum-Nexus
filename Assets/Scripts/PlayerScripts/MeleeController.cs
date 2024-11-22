using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeController : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public GameObject sword;
    public GameObject player;
    public LayerMask whatIsEnemy;
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

    // Start is called before the first frame update
    void Start()
    {
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

                    raycastHit.collider.gameObject.GetComponent<Health>().RemoveHealth(damage);
                }
            }
        }
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
        // AudioSource audioClip = GetComponent<AudioSource>();
        // audioClip.PlayOneShot(attackSound);

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
}
