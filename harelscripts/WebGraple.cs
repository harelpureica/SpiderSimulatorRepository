using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraplingGun : MonoBehaviour
{
    public LayerMask whatisplayer,whatIsclimbable;
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
    void Start()
    {
        joints=new List<SpringJoint>();
        lineRenderer =GetComponent<LineRenderer>();
        // lineRenderer.positionCount=0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isenabled)
        {
             if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartGraple();
        }
        }
         float DistanceFromPoint=Vector3.Distance(playerrig.position,GraplePoint);
            joint.maxDistance=maxdis;
            joint.minDistance=mindis;
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopGraple();
        }
        
       
        
    }
     void LateUpdate() {
        if(!joint)return;
         lineRenderer.SetPosition(0,gunTip.position);
            lineRenderer.SetPosition(1,GraplePoint);
            lineRenderer.positionCount=2;
    }
    public void StartGraple()
    {
        RaycastHit hitinfo;
        if(Physics.Raycast(transform.position,-transform.up,out hitinfo,maxDistanceToGraple,whatIsclimbable))
        {
            GraplePoint =hitinfo.point;
            joint=playerrig.gameObject.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor=false;
            joint.connectedAnchor=GraplePoint;
           
            //mess around with variables
            joint.spring =spring;
            joint.damper =damper;
            joint.massScale=massScale;
            joints.Add(joint);
           
            
        }
    }
    public void StopGraple()
    { 
    
        
        Destroy (joint);

       
       
     
    }
    
}
