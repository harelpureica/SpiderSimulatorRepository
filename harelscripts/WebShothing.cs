using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShothing : MonoBehaviour
{
    public Transform SpiderAimObj;
    LineRenderer lineRenderer;
    [SerializeField] LayerMask whatisflys,whatisclimable;
    Vector3 GraplePoint;
    SpringJoint joint;
    // spring settings
    [SerializeField] float spring;
    [SerializeField] float damper;
    [SerializeField] float massScale;
    
    void Start()
    {
        Cursor.lockState=CursorLockMode.Confined;
        lineRenderer= GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse2))
        {
            Ray CamRay=Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(CamRay,out RaycastHit hitinfo,100,whatisclimable))
            {
                 GraplePoint =hitinfo.point;
            joint=SpiderAimObj.gameObject.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor=false;
            joint.connectedAnchor=GraplePoint;
            float DistanceFromPoint=Vector3.Distance(SpiderAimObj.position,GraplePoint);
           
            //mess around with variables
            joint.spring =spring;
            joint.damper =damper;
            joint.massScale=massScale;
            
            }
        }
        if(joint)
        {
             float DistanceFromPoint=Vector3.Distance(SpiderAimObj.position,GraplePoint);
             joint.maxDistance=DistanceFromPoint* 0.6f;
            joint.minDistance= 0f;
        }
        if(Input.GetKeyUp(KeyCode.Mouse2))
        {
          
                Destroy(joint);
                lineRenderer.enabled=false;
            
            
        }
        
    }  
     void LateUpdate() {
        if(joint)
        { 
            lineRenderer.positionCount=2;
            lineRenderer.enabled=true;
            lineRenderer.SetPosition(0,SpiderAimObj.transform.position);
             lineRenderer.SetPosition(1,GraplePoint);
        }else
        {


        }
        
    }
}
