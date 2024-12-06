using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image _HealthBarFIll;
    [SerializeField] private Image _HealthBarBack;
    private void OnTriggerEnter(Collider playerbodies)
    {
        if (playerbodies.tag == "Player")
        {
            Debug.Log("player enter");
            _HealthBarFIll.color = new Color32(254, 0, 0, 255);
            _HealthBarBack.color = new Color32(118, 0, 7, 255);
        }
    }
}
