using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover : MonoBehaviour
{
    [Header("Dash Bar")]
    [SerializeField] private float speed;
    [SerializeField] private float height;
    [SerializeField] private float initialHeight;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float newHeight = Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(pos.x, newHeight + initialHeight, pos.z);
    }
}
