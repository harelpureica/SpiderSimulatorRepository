using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHarel : MonoBehaviour
{
   public bool PickedUp;
   PickUpItem pickUpItem; 
   FRLtarget fRLtarget;
   Transform player;
   Rigidbody rb;
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        fRLtarget=FindObjectOfType<FRLtarget>();
        pickUpItem=FindObjectOfType<PickUpItem>();
       PickedUp=false; 
       player=FindObjectOfType<RobotOneMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    { 
      
        if(PickedUp&&pickUpItem.isholding)
        {
             gameObject.GetComponent<MeshCollider>().enabled=false;
           rb.useGravity=true;
            transform.position=player.transform.position+(player.transform.forward*2)+(player.transform.right*2);
        } else if(PickedUp&&!pickUpItem.isholding)
        {
                rb.useGravity=true; 
                gameObject.GetComponent<MeshCollider>().enabled=true;
        }
        else if(!PickedUp&&!pickUpItem.isholding)
        {
            rb.useGravity=true; 
                gameObject.GetComponent<MeshCollider>().enabled=true;
        }
    }
}
