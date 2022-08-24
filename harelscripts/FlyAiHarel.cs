using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyAiHarel : MonoBehaviour
{
  
   public  GameObject CubeAreaForFly;
   public float randNum;
   [SerializeField] float speed;
    [SerializeField] bool walkpointSet =false;
   public Vector3 walkpoint;
  Animator animator; 
  [SerializeField] LayerMask whatisPicable;
  Collider nearColiderEat;
   

  bool nearFood;

    void Start()
    {
        animator=GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    nearFood=Physics.CheckSphere(transform.position,5f,whatisPicable);
    if(!nearFood)
    {
        Fly();
    }   else{
       float minNum=float.MaxValue;
       
       Collider[] eatablesnearFly=Physics.OverlapSphere(transform.position,5,whatisPicable);
       if(eatablesnearFly.Length>0)
       {
        foreach (var item in eatablesnearFly)
        {
            float dis= Vector3.Distance(item.transform.position,transform.position);
            if(dis<minNum)
            {
                nearColiderEat=item;
                minNum=dis;
            } 
        } 
        float Lerp =Mathf.MoveTowards(0,1,Time.deltaTime);
        transform.position=Vector3.Lerp(transform.position,nearColiderEat.transform.position,Lerp); transform.LookAt(nearColiderEat.transform.position);
      transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(nearColiderEat.transform.position-transform.position,Vector3.up),Time.deltaTime*2f); ;

       }
       
        

    }

    }
   public void Fly()
   {
    if(!walkpointSet)
    {
        
        float randz=Random.Range(-randNum,randNum);
    float randx=Random.Range(-randNum,randNum);
    float randy=Random.Range(-randNum/2,randNum/2);
     walkpoint=new Vector3( transform.position.x+ randx,transform.position.y+randy,transform.position.z+randz);
    Collider[]collidersNearWalkPoint =Physics.OverlapSphere(walkpoint,1f);
    if(collidersNearWalkPoint.Length>1)
    {
        walkpointSet=false;
    }else
    {
        walkpointSet=true;
    }


    }else
    {
       
        float dis=Vector3.Distance(walkpoint,transform.position);
        if(dis<=0.1f)
        {
            transform.position=transform.position;
            setWalkset();
            
        }else
        {
            transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(walkpoint-transform.position,Vector3.up),Time.deltaTime*2f); ;
            
             transform.position=Vector3.Lerp(transform.position,walkpoint,Time.deltaTime*speed);
              animator.SetBool("Fly",true);
             animator.SetBool("Idle",false);
        }

    }



    
   }
   public void setWalkset()
   {
walkpointSet=false;
   }
}
