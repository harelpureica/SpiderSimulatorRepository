using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshGenerator : MonoBehaviour
{
    public static MeshData GenerateMesh(float[,]heightmap,float heightMultyplayer,AnimationCurve meshaAnimationCurve, int levelOfDetails)

    {
                 
            int width = heightmap.GetLength(0);
            int height = heightmap.GetLength(1);
            float topleftX =(width-1)/-2f;
            float topleftY =(height-1)/2f;
            int meshSimplificationIncrement =(levelOfDetails==0)?1:levelOfDetails;
            int vertecisPerLine =(width-1)/meshSimplificationIncrement+1;
            MeshData meshData =new MeshData(vertecisPerLine,vertecisPerLine);
            int vertexindex=0;
            for (int y = 0; y < height; y+=meshSimplificationIncrement)
            {
                for (int x = 0; x < width; x+=meshSimplificationIncrement)
                {
                  // FIRST 2 VERTS ON EDGES
                  
                  /* if(x==0||x==1 )
                   {
                     meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,0,topleftY-y);
                   }else if(y==0|| y==1)
                   {
                    meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,0,topleftY-y);
                   }else if(x==width||x==width-1)
                   {
                    meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,0,topleftY-y);
                   }else if(y==height||y==height-1)
                   {
                     meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,0,topleftY-y);
                   }
                   //SECOND34 VERTS FROM EDGES
                  
                   
                  else if(x>width-20)
                   {
                    float currentheight =meshaAnimationCurve.Evaluate(heightmap[x,y])*heightMultyplayer;
                    
                    
                       currentheight=currentheight/1.7f;
                    
                    meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,currentheight,topleftY-y); 
                   }
                  else if(y>height-20)
                   {
                     float currentheight =meshaAnimationCurve.Evaluate(heightmap[x,y])*heightMultyplayer;
                    currentheight=currentheight/1.7f;
                     meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,currentheight,topleftY-y);
                   }else if(y<20)
                   {
                    float currentheight =meshaAnimationCurve.Evaluate(heightmap[x,y])*heightMultyplayer;
                    currentheight=currentheight/1.7f;
                     meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,currentheight,topleftY-y);
                   }else if(x<20)
                   {
                     float currentheight =meshaAnimationCurve.Evaluate(heightmap[x,y])*heightMultyplayer;
                    currentheight=currentheight/1.7f;
                     meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,currentheight,topleftY-y);
                   }
                   else
                   {*/
                    
                     meshData.vertecis[vertexindex]=new Vector3(topleftX+ x,meshaAnimationCurve.Evaluate(heightmap[x,y]) *heightMultyplayer,topleftY-y);
                      
                  // }
                   
                   
                   
                   
                   
                    meshData.uvs[vertexindex] =new Vector2(x/(float)width,y/(float)height);
                    if (x < width - 1 && y < height - 1)
                    {
                        meshData.AddTriangle(vertexindex,vertexindex+1+vertecisPerLine,vertexindex+vertecisPerLine);
                    meshData.AddTriangle(vertexindex+vertecisPerLine+1,vertexindex,vertexindex+1);
                    }
                    

                    vertexindex++;
                }
                
            }
            return meshData;
    }

    
}
public class MeshData
{
  public  Vector3[] vertecis ;
   public int[]triangles;
   int trisindex = 0;
   public Vector2[] uvs;
    public MeshData(int meshwidth, int meshheight)
    {
        vertecis = new Vector3[meshwidth*meshheight];
        triangles = new int[(meshheight-1)*(meshwidth-1)*6];
        uvs = new Vector2[meshheight*meshwidth];
        
    }
    public void AddTriangle(int a,int b,int c)
    {
        triangles[trisindex] = a;
        triangles[trisindex+1] = b;
        triangles[trisindex+2] = c;
        trisindex+=3;

    }
    public Mesh CreateMesh()

    {
        Mesh mesh = new Mesh();
        mesh.vertices =vertecis;
        mesh.triangles= triangles;
        mesh.uv =uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
