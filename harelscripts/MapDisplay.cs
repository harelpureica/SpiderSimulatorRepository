using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
  public MeshFilter meshFilter;
  public MeshRenderer meshRenderer;
  public void Drawtexture(Texture2D texture2D)
  {
   
   textureRender.sharedMaterial.mainTexture =texture2D;
   textureRender.transform.localScale =new Vector3(texture2D.width,1,texture2D.height);
   
   

   
  }
  public void DrawMesh(MeshData meshData,Texture2D texture)
  {
meshFilter.sharedMesh =meshData.CreateMesh();
meshRenderer. sharedMaterial.mainTexture =texture;
  }
}
