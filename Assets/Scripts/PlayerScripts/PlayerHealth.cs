using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxPlayerHealth;
    public int sceneNumber;
    float playerHealth;


    // Start is called before the first frame update
    void Start()
    {
        maxPlayerHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50)
        {
            KillPlayer();
        }
    }

    void RecoverHealth(float health)
    {
        playerHealth += health;
    }
    void DamagePlayer(float damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            KillPlayer();
        }
    }
    void KillPlayer()
    {
        Debug.Log("dead");
        SceneManager.LoadScene(sceneNumber);
    }
}
