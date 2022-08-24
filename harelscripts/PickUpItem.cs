using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    RobotOneMovement robotOneMovement;
   public GameObject frlLeg_chaintarget;
   [SerializeField] LayerMask whatispickable;
   Collider nearCollider;
   Collider farCollider;
   float maxDisCollider=float.MinValue;
   float minDisCollider=float.MaxValue;
   bool togler; 
   int i;
   public bool isholding;
    void Start()
    {
        robotOneMovement=GetComponent<RobotOneMovement>();
        i=0;
    }

    // Update is called once per frame
    void Update()
    {
        //onclick
         if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            
           findnearestPickAble();
           if(nearCollider!=null)
           {
            togler=!togler;
            if(togler)
            {

                robotOneMovement.ShouldPickUpaAnimationFRL=true;
                nearCollider.GetComponent<ItemHarel>().PickedUp=true;
                
                
            }else
            {
                Invoke(nameof(SetPickedup),1f);
                 robotOneMovement.ShouldPickUpaAnimationFRL=false;
                   isholding =false;
                   nearCollider.gameObject.transform.position =new Vector3(nearCollider.transform.position.x+1,nearCollider.transform.position.y+2,nearCollider.transform.position.z+1);
            }

           }
        }
        if(nearCollider!=null)
        {
            if(robotOneMovement)
            if(robotOneMovement.ShouldPickUpaAnimationFRL&&!isholding)
        {
            frlLeg_chaintarget.transform.position=Vector3.Lerp(frlLeg_chaintarget.transform.position,nearCollider.transform.position,Time.deltaTime*4f);
          float dis=Vector3.Distance(frlLeg_chaintarget.transform.position,nearCollider.transform.position);
            if(dis<0.1f)
            {
                isholding=true;
            }
        }else if(isholding&&robotOneMovement.ShouldPickUpaAnimationFRL)
        {
             frlLeg_chaintarget.transform.position=transform.position+(transform.right*2)+(transform.forward*2);
        }
        }

        
        
    }
            public void SetPickedup()
         {
                nearCollider.GetComponent<ItemHarel>().PickedUp=false;

         }
    public void  findnearestPickAble()
    {
       Collider[] pickablesInRange= Physics.OverlapSphere(transform.position,5f,whatispickable);
       if(pickablesInRange.Length>0)
       {
         minDisCollider=float.MaxValue;
        foreach (var item in pickablesInRange)
        {
            float dis=Vector3.Distance(transform.position,item.transform.position);
           
             if(dis<minDisCollider)
            {
                nearCollider=item;
            }
            
        
        }
        
       
        
       }
       else if(! isholding)
       {
        nearCollider=null;
       }
    }
}
