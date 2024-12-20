using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class sheild : MonoBehaviour
{
    [Header("References")]
    //public GameObject meleeHolder;
    //public GameObject gunHolder;
    //public GameObject playerCamera;
    public GameObject cube;
    // MeleeController meleeControl;
    // GunController gunControl;
    //SwitchWeapon switchWeapon;
    [SerializeField] private Image _clockFill;


    Health playerControl;
    Health playerBodyControl;

    public GameObject player;
    public GameObject playerbody;


    [Header("Modifiers")]
    //public float damageModifier;
    public float timeWithShield;
    bool shieldIsActive;

    float originalMeleeDamage;
    float originalGunDamage;

   


    // Start is called before the first frame update
    void Start()
    {
        //meleeControl = meleeHolder.GetComponent<MeleeController>();
       // gunControl = gunHolder.GetComponent<GunController>();
        //switchWeapon = playerCamera.GetComponent<SwitchWeapon>();

        playerControl = player.GetComponent<Health>();
        playerBodyControl = playerbody.GetComponent<Health>();

        _clockFill.color = new Color32(1, 1, 1, 0);



        // originalMeleeDamage = meleeControl.damage;
        //originalGunDamage = gunControl.damage;

        //empty
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldIsActive)
        {
            timeWithShield -= Time.deltaTime;

            _clockFill.color = new Color32(136, 209, 255, 255);
            _clockFill.fillAmount = timeWithShield / 30;
            if (timeWithShield <= 0)
            {
                //meleeControl.damage = originalMeleeDamage;
                //gunControl.damage = originalGunDamage;
                playerControl.canTakeDamage = true;
                playerBodyControl.canTakeDamage = true;
                shieldIsActive = false;
                Destroy(transform.parent.gameObject);

                


            }
            Debug.Log("sheild is active == true");
        }
        else
        {
            
            Debug.Log("sheild is active == false");

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "PlayerBody" && !shieldIsActive)
        {

            playerControl.canTakeDamage = false;
            playerBodyControl.canTakeDamage = false;


            shieldIsActive = true;
            //removes visual from scene
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            cube.GetComponent<MeshRenderer>().enabled = false;

            

        }
    }
}
