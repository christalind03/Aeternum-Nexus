using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeController : MonoBehaviour
{
    [Header("References")]
    public GameObject sword;
    public GameObject player;
    public InputActionAsset playerControls;
    InputAction swingAction;

    [Header("Conditions")]
    public bool canAttack = true;
    public bool isAttacking = false;

    [Header("Modifiers")]
    public float attackCooldown = 1f;
    public float damage = 15;
    // public AudioClip attackSound;

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
