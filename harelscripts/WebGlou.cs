using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGlou : MonoBehaviour
{
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player")
        {
            Debug.Log("player pass throowweb ");
        }
    }
}
