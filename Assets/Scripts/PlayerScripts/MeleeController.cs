using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeControl : MonoBehaviour
{
    [Header("References")]
    public GameObject sword;
    public GameObject player;
    PlayerMovement playerMovement;

    [Header("Conditions")]
    public bool canAttack = true;
    public bool isAttacking = false;
    public float attackCooldown = 1f;
    // public AudioClip attackSound;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canAttack)
            {
                SwordAttack();
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
