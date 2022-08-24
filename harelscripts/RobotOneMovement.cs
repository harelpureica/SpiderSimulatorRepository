using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RobotOneMovement : MonoBehaviour
{ 
  public CinemachineVirtualCamera vcam;
  public CinemachineFreeLook freeLookcam;
   Rigidbody rb;
   public float mouseXsens;
   public float mouseYsens;
   public Transform targetzones;
   public float speed;
   public LayerMask whatisground,whatisclimbable;
    float horizontal;
     Collider climablecolider;
    float vertical;
     Vector3 movedir;
     float mouseY;
     float mouseX;
     Vector3 slopdir;
     public bool grounded;
     public LayerMask whatisground2;
     public GameObject metarig;
     // procedural leg animation;
     public float legdis2;
     public Transform BrlTargetZone;
     public Transform BrlTarget;
     Vector3 tempwalkspotBRL;
     public Transform BllTargetZone;
     public Transform BllTarget;
     Vector3 tempwalkspotBlL;
     public Transform FrlTargetZone;
     public Transform FrlTarget;
     Vector3 tempwalkspotfrL;
     public Transform FllTargetZone;
     public Transform FllTarget;
     Vector3 tempwalkspotflL;
     bool zigizag;
     public float timeforzigzag;
     float timer;
     public float armSpeed;
      public Transform highstepcheck;
      public Transform lowstepcheck;
      public float stepJumpHeight;
      public bool onSlop;
      public float jumpForce;
      float StartSpeed;
      float Startarmspeed;
      float sprintarmspeed;
      float Starttimetozigzag;
      float sprinttimetozigzag;
      [SerializeField]  float sprintSpeed; 
      public bool nearclimbable;
      public bool onclimb;
      bool forclimbdownbool;
     public bool ShouldPickUpaAnimationFRL=false;
    void Start()
    {
      Startarmspeed=armSpeed;
      Starttimetozigzag=timeforzigzag;
      StartSpeed=speed;
      sprintSpeed=StartSpeed*1.7f;
      sprintarmspeed=Startarmspeed*1.5f;
      sprinttimetozigzag=Starttimetozigzag*0.9f;
      
    timer=0;
      rb=GetComponent<Rigidbody>();  
      tempwalkspotBlL=BllTargetZone.position;
      tempwalkspotBRL=BrlTargetZone.position;
      tempwalkspotfrL=FrlTargetZone.position;
      tempwalkspotflL=FllTargetZone.position;
    }

    // Update is called once per frame
    void Update()
    {
      //sprint
      if (Input.GetKey(KeyCode.LeftShift))
      {
        speed=sprintSpeed;
        armSpeed=sprintarmspeed;
        timeforzigzag=sprinttimetozigzag;
      }else
      {
        timeforzigzag=Starttimetozigzag;
        armSpeed=Startarmspeed;
        speed=StartSpeed;
      }
      /////////
      timer+=Time.deltaTime;
      if(timer>timeforzigzag)
      {
        zigizag=!zigizag;
        timer=0;
      }
         horizontal =Input.GetAxis("Horizontal");
             vertical=Input.GetAxis("Vertical");
             mouseY+=Input.GetAxis("Mouse X")*mouseYsens;
            mouseX -=Input.GetAxis("Mouse Y")*mouseXsens;
              movedir=(transform.forward*vertical+transform.right*horizontal).normalized;
              
        slopdirhandle();
        if(!nearclimbable)
        {
           Move();
        
       
        AlignWithSurface();
        movetargetzonesforlegs();

        //StepsHandlerrobo();
        }
      Climb();
      
        if(!grounded&&!nearclimbable)
        {
          tempwalkspotBlL=BllTargetZone.position;
      tempwalkspotBRL=BrlTargetZone.position;
      tempwalkspotfrL=FrlTargetZone.position;
      tempwalkspotflL=FllTargetZone.position;
        }
         BrlTarget.position=Vector3.Slerp (BrlTarget.position,tempwalkspotBRL,Time.deltaTime*armSpeed);
         
         BllTarget.position=Vector3.Slerp (BllTarget.position,tempwalkspotBlL,Time.deltaTime*armSpeed);
         if(!ShouldPickUpaAnimationFRL) 
         {
            FrlTarget.position=Vector3.Slerp (FrlTarget.position,tempwalkspotfrL,Time.deltaTime*armSpeed);

         }
         
         FllTarget.position=Vector3.Slerp (FllTarget.position,tempwalkspotflL,Time.deltaTime*armSpeed);
         
         /////////////////////////
         if(grounded||nearclimbable)
         {
         if(zigizag)
         {
          Brl(); 
          Fll(); 
         }else
         {
           Bll(); 
          Frl(); 
         }
         }
        /////////////////////////////
        
         Jump();
         if(!grounded&&!nearclimbable)
         {
          rb.AddForce(Vector3.down,ForceMode.Impulse);
         }
         
         
         
           
       
       
    }
    public void Move()
    {
         
             
             
        if(!grounded&&(horizontal!=0|| vertical!=0))
        {
            
           rb.velocity=Vector3.Lerp(rb.velocity,movedir*speed,Time.deltaTime*2);
          
        }else if(grounded&&(horizontal!=0|| vertical!=0)&&!onSlop)
        {
          //  transform.forward= Vector3.Lerp(transform.forward,slopdir,Time.deltaTime*2) ;
             rb.velocity=Vector3.Lerp(rb.velocity,slopdir*speed,Time.deltaTime*2);
            
        }else if(grounded&&(horizontal!=0|| vertical!=0)&&onSlop)
        {
          //  transform.forward= Vector3.Lerp(transform.forward,slopdir,Time.deltaTime*2) ;
             rb.velocity=Vector3.Lerp(rb.velocity,slopdir*speed*1.3f,Time.deltaTime*2);
            
        }
        
        else
        {
       
            rb.velocity=Vector3.Lerp(rb.velocity,Vector3.zero,Time.deltaTime*2);
        }
        
        
          
          

     
    }
    public void AlignWithSurface()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hitinfo, 4.5f,whatisground)&&!nearclimbable)
        {
              
             
             
             float angle=Vector3.Angle(Vector3.up,hitinfo.normal);
             if(angle>5)
             {
              onSlop=true;
             }else
             {
              onSlop=false;
             }
              
             grounded=true;
              transform.rotation=Quaternion.Euler( new Vector3(Mathf.Clamp(mouseX,-5,15),   mouseY,transform.rotation.z));
            
              
        
        }else if(!nearclimbable)
        {
             transform.rotation=Quaternion.Euler( new Vector3(Mathf.Clamp(mouseX,-5,15) ,   mouseY,transform.rotation.z));
           
          onSlop=false;
          
            grounded=false;
            
        }
    }
    public void slopdirhandle()
    {
         if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hitinfo, 4f,whatisground))
        {
              slopdir=Vector3.ProjectOnPlane(movedir,hitinfo.normal);
              Debug.DrawRay(transform.position,slopdir*speed,Color.green);
        }
        
        
    
    }
    public void Brl()
    {
      
        Vector3 targetZonePos=new Vector3(BrlTargetZone.position.x,BrlTargetZone.position.y+4,BrlTargetZone.position.z);
        
        if(Physics.Raycast(targetZonePos,Vector3.down,out RaycastHit hitinfo,8f,whatisground2))
        {
            float  dis=Vector3 .Distance(tempwalkspotBRL,hitinfo.point);

            if(dis>legdis2)
            {
                 tempwalkspotBRL=hitinfo.point;
                 

                
            }
        
        
       }else if(Physics.Raycast(BrlTargetZone.position+(transform.up*4),-transform.up,out RaycastHit hitinfoclimb,8f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotBRL,hitinfoclimb.point);

            if(dis>legdis2)
            {
                 tempwalkspotBRL=hitinfoclimb.point;
                 

                
            }
       }else if(Physics.Raycast(transform.position+(transform.up*2),-transform.up,out RaycastHit hitinfoclimbstright,8f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotBRL,hitinfoclimbstright.point);

            if(dis>legdis2)
            {
                 tempwalkspotBRL=hitinfoclimbstright.point;
                 

                
            }
       }
       else
       {
        tempwalkspotBRL=BrlTargetZone.position;
       }
    }
     public void Bll()
    {
      
        Vector3 targetZonePos=new Vector3(BllTargetZone.position.x,BllTargetZone.position.y+4,BllTargetZone.position.z);
        if(Physics.Raycast(targetZonePos,Vector3.down,out RaycastHit hitinfo,8f,whatisground2))
        {
            float  dis=Vector3 .Distance(tempwalkspotBlL,hitinfo.point);

            if(dis>legdis2)
            {
                 tempwalkspotBlL=hitinfo.point;
                 

                
            }
           
                
            
        
        
       }else if(Physics.Raycast(BllTargetZone.position+transform.up,-transform.up,out RaycastHit hitinfoclimb,8f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotBlL,hitinfoclimb.point);

            if(dis>legdis2)
            {
                 tempwalkspotBlL=hitinfoclimb.point;
                 

                
            }
       }
       else if(Physics.Raycast(transform.position+(transform.up*3),-transform.up,out RaycastHit hitinfoclimbstright,8f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotBlL,hitinfoclimbstright.point);

            if(dis>legdis2)
            {
                 tempwalkspotBlL=hitinfoclimbstright.point;
                 

                
            }
       }else
       {
       
       
        tempwalkspotBlL=BllTargetZone.position;
       }
    }
     public void Frl()
    {
      if(!ShouldPickUpaAnimationFRL)
      {
        Vector3 targetZonePos=new Vector3(FrlTargetZone.position.x,FrlTargetZone.position.y+4,FrlTargetZone.position.z);
        if(Physics.Raycast(targetZonePos,Vector3.down,out RaycastHit hitinfo,8f,whatisground2))
        {
            float  dis=Vector3 .Distance(tempwalkspotfrL,hitinfo.point);

            if(dis>legdis2)
            {
                 tempwalkspotfrL=hitinfo.point;
                 

                
            }
           
                
            
        
        
       }else if(Physics.Raycast(FrlTargetZone.position+transform.up,-transform.up,out RaycastHit hitinfoclimb,8f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotfrL,hitinfoclimb.point);

            if(dis>legdis2)
            {
                 tempwalkspotfrL=hitinfoclimb.point;
                 

                
            }
       }else if(Physics.Raycast(transform.position+(transform.up*3),-transform.up,out RaycastHit hitinfoclimbstraight,8f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotfrL,hitinfoclimbstraight.point);

            if(dis>legdis2)
            {
                 tempwalkspotfrL=hitinfoclimbstraight.point;
                 

                
            }
       }
       else
       {
        tempwalkspotfrL=FrlTargetZone.position;
       }
      }
    }
     public void Fll()
    {
      
       
        Vector3 targetZonePos=new Vector3(FllTargetZone.position.x,FllTargetZone.position.y+4,FllTargetZone.position.z);
       Debug.DrawRay(FllTargetZone.position,-transform.up,Color.white);
        if(Physics.Raycast(targetZonePos,Vector3.down,out RaycastHit hitinfo,8f,whatisground2)&&!nearclimbable)
        {
            float  dis=Vector3 .Distance(tempwalkspotflL,hitinfo.point);

            if(dis>legdis2)
            {
                 tempwalkspotflL=hitinfo.point;
                 

                
            }
           
                
            
        
        
       }else if(Physics.Raycast(FllTargetZone.position+(transform.up*4),-transform.up,out RaycastHit hitinfoclimb,10f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotflL,hitinfoclimb.point);

            if(dis>legdis2)
            {
                 tempwalkspotflL=hitinfoclimb.point;
                 

                
            }
       }else if(Physics.Raycast(transform.position+(transform.up*4),-transform.up,out RaycastHit hitinfoclimbstright,10f,whatisclimbable))
       {
           float  dis=Vector3 .Distance(tempwalkspotflL,hitinfoclimbstright.point);

            if(dis>legdis2)
            {
                 tempwalkspotflL=hitinfoclimbstright.point;
                 

                
            }
       }
       else
       {
        tempwalkspotflL=FllTargetZone.position;
       }
    }
    public void movetargetzonesforlegs()
    {
      if(horizontal>0.1f)
      {
        targetzones .localPosition=new Vector3(1,targetzones.localPosition.y,targetzones.localPosition.z);
      }else if(horizontal<-0.1f)
      {
         targetzones .localPosition=new Vector3(-1,targetzones.localPosition.y,targetzones.localPosition.z);
      } else
      {
         targetzones .localPosition=new Vector3(0,targetzones.localPosition.y,targetzones.localPosition.z);
      }


      if(vertical>0.1f)
      {
        targetzones .localPosition=new Vector3(targetzones.localPosition.x,targetzones.localPosition.y,1);
      }else if(vertical<-0.1f)
      {
         targetzones .localPosition=new Vector3(targetzones.localPosition.x,targetzones.localPosition.y,-4);
      } else
      {
         targetzones .localPosition=new Vector3(targetzones.localPosition.x,targetzones.localPosition.y,0);
      }
    }
public void StepsHandlerrobo()
     {
      if(!onSlop&&!nearclimbable)
      {
///soulotion for  reducing all raycasts
Vector3 stepdir=Vector3.ProjectOnPlane(transform.forward,Vector3.up);
Vector3 stepdirRight=Vector3.ProjectOnPlane(transform.forward+(transform.right*0.4f),Vector3.up);
Vector3 stepdirLeft=Vector3.ProjectOnPlane(transform.forward-(transform.right*0.4f),Vector3.up);

Debug.DrawRay(lowstepcheck.position,stepdir*5,Color.blue);
if(lowstepcheck.position.y>0.05f)
{


 if( Physics.Raycast(lowstepcheck.position,stepdir,0.2f,whatisground))
      {
       
        if(!Physics.Raycast(highstepcheck.position,stepdir,1,whatisground))
        {
          
          transform.position= new Vector3(transform.position.x,transform.position.y+stepJumpHeight,transform.position.z);
        }
      } 
      Debug.DrawRay(lowstepcheck.position,stepdirRight*5,Color.blue);
 if( Physics.Raycast(lowstepcheck.position,stepdirRight,0.2f,whatisground))
      {
       
        if(!Physics.Raycast(highstepcheck.position,stepdirRight,1,whatisground))
        {
          
          transform.position= new Vector3(transform.position.x,transform.position.y+stepJumpHeight,transform.position.z);
        }
      } 
      Debug.DrawRay(lowstepcheck.position,stepdirLeft*5,Color.blue);
 if( Physics.Raycast(lowstepcheck.position,stepdirLeft,0.2f,whatisground))
      {
       
        if(!Physics.Raycast(highstepcheck.position,stepdirLeft,1,whatisground))
        {
          
          transform.position= new Vector3(transform.position.x,transform.position.y+stepJumpHeight,transform.position.z);
        }
      } 

////
      

      
      
    
        } else
        {
          Vector3 newlowposY=new Vector3(lowstepcheck.position.x,0.1f,lowstepcheck.position.z);
          if( Physics.Raycast(newlowposY,stepdir,0.2f,whatisground))
      {
       
        if(!Physics.Raycast(newlowposY,stepdir,1,whatisground))
        {
          
          transform.position= new Vector3(transform.position.x,transform.position.y+stepJumpHeight,transform.position.z);
        }
      } 
      Debug.DrawRay(newlowposY,stepdirRight*5,Color.blue);
 if( Physics.Raycast(newlowposY,stepdirRight,0.2f,whatisground))
      {
       
        if(!Physics.Raycast(newlowposY,stepdirRight,1,whatisground))
        {
          
          transform.position= new Vector3(transform.position.x,transform.position.y+stepJumpHeight,transform.position.z);
        }
      } 
      Debug.DrawRay(newlowposY,stepdirLeft*5,Color.blue);
 if( Physics.Raycast(newlowposY,stepdirLeft,0.2f,whatisground))
      {
       
        if(!Physics.Raycast(newlowposY,stepdirLeft,1,whatisground))
        {
          
          transform.position= new Vector3(transform.position.x,transform.position.y+stepJumpHeight,transform.position.z);
        }
      } 

        }
    }
  }
     public void Jump()
      {
        if(grounded&&Input.GetKeyDown(KeyCode.Space))
        {
          rb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
        }else if(nearclimbable&&Input.GetKeyDown(KeyCode.Space))
        {
           rb.AddForce(transform.up*(jumpForce/2f),ForceMode.Impulse);
        }
      } 
      public void Climb()
      {
        //
        float maxdis =float.MinValue;
        float mindis =float.MaxValue;
        
       Collider[]climablesNearPlayer= Physics.OverlapSphere(transform.position,6f,whatisclimbable);

       //
       if(climablesNearPlayer.Length>0&& (!Physics.Raycast(transform.position,transform.forward+(Vector3.down*0.2f),2.5f,whatisground))&&!forclimbdownbool)
       {
        freeLookcam.m_Priority=11;
       
        
          
        
        nearclimbable=true;

        foreach (var item in climablesNearPlayer)
       {
        Vector3 withouty=new Vector3(item.transform.position.x,transform.position.y,item.transform.position.z);
        float dis= Vector3.Distance(transform.position,withouty);
        

        if(dis>maxdis)
        {
          maxdis=dis;
        }
         if(dis<mindis)
        {
          mindis=dis;
          climablecolider=item;
        }
       }
       } else

       {
        freeLookcam.m_Priority=9;
        nearclimbable=false;
       }


       if(nearclimbable)
       {
        grounded=false;
       onSlop=false;
        rb.useGravity=false;
       Vector3 garivitydir=(climablecolider.transform.position-transform.position).normalized;
       garivitydir.y=0;

       Debug.DrawRay(transform.position,garivitydir*40,Color.black);

       

if(Physics.Raycast(transform.position,-transform.up,out RaycastHit hitinfoCLImbforNormal, 6f,whatisclimbable)&&!forclimbdownbool)
       
       {
        
         Vector3 moveclimedir= (transform.forward*vertical+transform.right*horizontal).normalized;
       

         Vector3 climbdir=Vector3.ProjectOnPlane(moveclimedir,hitinfoCLImbforNormal.normal);
         if(moveclimedir.magnitude<0.05f)
         {
        
          // transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(transform.forward,hitinfoCLImbforNormal.normal),Time.deltaTime*3);
         }else if(vertical>=0)
         {
         transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(climbdir,hitinfoCLImbforNormal.normal),Time.deltaTime*3f) ;

         }
         Debug.DrawRay(transform.position,climbdir*30f,Color.blue);
          rb.velocity=Vector3.Lerp(rb.velocity,climbdir*speed*1.5f,Time.deltaTime*2f);
          rb.AddForce(-transform.up*0.5f,ForceMode.Impulse);
        onclimb =true;
       }else
       {
        onclimb =false;
       }
 if(!onclimb)
 {
 rb.AddForce(garivitydir*2,ForceMode.Impulse);
 }
      Debug.DrawRay(transform.position,(transform.forward+(-transform.up*0.2f))*5,Color.magenta);

       if(Physics.Raycast(transform.position,garivitydir,out RaycastHit hitinfoCLINB, 7f,whatisclimbable)&&!onclimb&&!Physics.Raycast(transform.position,transform.forward+(Vector3.down*0.2f),2.5f,whatisground)&&!forclimbdownbool)
       
       {
        
         Vector3 moveclimedir= (Vector3.up*vertical+transform.right*horizontal).normalized;
       

         Vector3 climbdir=Vector3.ProjectOnPlane(moveclimedir,hitinfoCLINB.normal);
         transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(Vector3.up,hitinfoCLINB.normal),Time.deltaTime*3f) ;
         Debug.DrawRay(transform.position,climbdir*30f,Color.blue);
         
          
          rb.velocity=Vector3.Lerp(rb.velocity,climbdir*speed,Time.deltaTime*2);
           
       }
    
       }else 
       {
        rb.useGravity=true;
        
       }
       if(onclimb&&Physics.Raycast(transform.position,transform.forward,out RaycastHit hitinfo,5.5f,whatisground))
       {
        forclimbdownbool=true;
        Debug.Log("tried to roatate");
        transform.position= Vector3.Lerp(transform.position,hitinfo.point,Time.deltaTime*2f);
        transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(transform.forward,hitinfo.normal),Time.deltaTime*3f) ;
      rb.AddForce(transform.forward,ForceMode.Force);
     Invoke(nameof(Setforclimbdownbool),2f);
       }
       
       
       
       
       //

       

       
      }
      public void Setforclimbdownbool()
      {
forclimbdownbool=false;
      }
     
        
    
}

