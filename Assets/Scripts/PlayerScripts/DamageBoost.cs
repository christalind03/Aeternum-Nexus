using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamageBoost : MonoBehaviour
{
    [Header("References")]
    public GameObject meleeHolder;
    public GameObject gunHolder;
    public GameObject playerCamera;
    public GameObject cube;
    MeleeController meleeControl;
    GunController gunControl;
    SwitchWeapon switchWeapon;
    [SerializeField] private Image _clockFill;

    [Header("Modifiers")]
    public float damageModifier;
    public float timeWithBoost;
    bool boostIsActive;

    float originalMeleeDamage;
    float originalGunDamage;

    // Start is called before the first frame update
    void Start()
    {
        meleeControl = meleeHolder.GetComponent<MeleeController>();
        gunControl = gunHolder.GetComponent<GunController>();
        switchWeapon = playerCamera.GetComponent<SwitchWeapon>();

        originalMeleeDamage = meleeControl.damage;
        originalGunDamage = gunControl.damage;

        _clockFill.color = new Color32(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (boostIsActive)
        {
            timeWithBoost -= Time.deltaTime;

            _clockFill.color = new Color32(136, 209, 255, 255);
            _clockFill.fillAmount = timeWithBoost/30;

            if (timeWithBoost <= 0)
            {
                meleeControl.damage = originalMeleeDamage;
                gunControl.damage = originalGunDamage;

                boostIsActive = false;
                Destroy(transform.parent.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "PlayerBody" && !boostIsActive)
        {
            meleeControl.damage += (damageModifier * meleeControl.damage);
            gunControl.damage += (damageModifier * gunControl.damage);

            boostIsActive = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            cube.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
}
