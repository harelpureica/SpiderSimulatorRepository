using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Spawner : MonoBehaviour
{
     public float minRandomnum ;
  public enum RegionMode{water ,light,purple,rock}
public float minheightSpwanrange;
public float maxheightSpwanrange ;
  public float maxrandomnum;
  public int frequency;
    public GameObject ObjToSpawn;
    public Transform ParentObj;

    public LayerMask layerMask;
    MeshData meshData;
    public Vector3 spwanspot;
   MapGenerator mapGenerator;
   
   
  private void Start() {
      
      
      mapGenerator = GetComponent<MapGenerator>();
      
     

  }
  private void Update() {
   if(frequency <=5)
   {
    frequency =5;
   }
   if(minRandomnum<5)
   {
    minRandomnum=5;
   }
  }
 
    public void SpwanObjectOnTerrain()
    {
       
      
       
        int width =240*10;
        int height =240*10;
        
        int i=0;
        for (int y = 0; y < height; y+=frequency)
        {
           for (int x = 0; x < width; x+=frequency)
           {
             Vector3 spot = new Vector3((float)x-(float)width/2f,maxheightSpwanrange,(float)y-(float)height/2f);

            if(i>Random.Range(minRandomnum,maxrandomnum))
            {
             // Debug.DrawRay(spot,Vector3.down,Color.red,1000f);
                 RaycastHit raycastHit;
                if(Physics.Raycast(spot,Vector3.down,out raycastHit, 100000f,layerMask))
                 {
                  // Debug.DrawRay(spot,Vector3.down,Color.red,100000f);
                  Vector3 spwanspot =raycastHit.point;
                     if(raycastHit.point.y>=minheightSpwanrange)
                     {
                      Quaternion randomQuaternion =Quaternion.Euler(0,Random.Range(0,360),0);
                      //spwanspot.y =spwanspot.y-0.1f;
                      var MYnewObj= Instantiate(ObjToSpawn,spwanspot,randomQuaternion);
                      MYnewObj.transform.parent=ParentObj;
                      
                      
                     }
                   i=0;
                 }
            }
               
               i++;
            
           } 
        }
    }
}
