using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshCollider))]
public class WebDangle : MonoBehaviour
{
    public LayerMask whatisplayer,whatIsclimbable;
    public MeshCollider meshCollider; 
    public GameObject meshlinecoliderprefabe;
    public GameObject webpool;
    
    public float maxDistanceToGraple;
    public bool isenabled;
    public float maxdis;
    public float mindis;
    public Vector3 GraplePoint; 
    public Transform fpscam,playerrig,gunTip;
    public float spring=4.5f;
    public float damper=7;
    public float massScale=4.5f;
    LineRenderer lineRenderer;
    SpringJoint joint;
    List<SpringJoint> joints;
    int i;
    Transform player;
    void Start()
    {
        
        player=transform;
        i =0;
        joints=new List<SpringJoint>();
        
        
        // lineRenderer.positionCount=0;
    }
   

    // Update is called once per frame
    void Update()
    {
      
      
             if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(i==0)
            {
                if(meshlinecoliderprefabe!=null)
                {
                
                 meshCollider= Instantiate(meshlinecoliderprefabe,Vector3.zero,Quaternion.identity).GetComponent<MeshCollider>();
                 meshCollider.transform.parent=webpool.transform;
                 lineRenderer =meshCollider. gameObject.GetComponent<LineRenderer>();
                }
           
          
            }
           
            StartGraple();
            float anothersameDistanceFromPoint=Vector3.Distance(player.position,GraplePoint);
             GenerateMesh();
            
             

        }
        
       
         
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            StopWebing();
        }
        
       
        
    }
     
    public void StartGraple()
    {
        if(lineRenderer!=null)
        {
            lineRenderer.enabled=true;
        }
        
        RaycastHit hitinfo;
        if(Physics.Raycast(player. transform.position,-player.transform.up,out hitinfo,maxDistanceToGraple,whatIsclimbable))
        {
            lineRenderer.positionCount=i+2;
            if(i==0)
            {
                 lineRenderer.SetPosition(0,player.transform.position);
                 lineRenderer.SetPosition(1,player.transform.position);
                
            }
            
            GraplePoint =hitinfo.point;
            
            lineRenderer.SetPosition(i,GraplePoint);
           
            lineRenderer.SetPosition(i+1, player.transform.position);
           
            i++;
            
    
           
            
           
            
        }
    }
    public void StopWebing()
    { 
    /*
        lineRenderer.positionCount=0;
        lineRenderer.enabled=false;*/
        i=0;
        
       

       
       
     
    } public void GenerateMesh()
    {
        Mesh meshline=new Mesh();
        if(meshCollider==null)
        {
            Debug.Log("meshcolider is null");
        }else{
            meshCollider.GetComponent<LineRenderer>().BakeMesh(meshline);
            meshCollider. sharedMesh=meshline;
        }
    }

    
}

