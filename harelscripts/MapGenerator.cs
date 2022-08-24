using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
       public float decayersmothedge;
    public  AnimationCurve meshaAnimationCurve;
    public float heightMultyplayer;
    public enum Drawmode {noiseMap,colormap,mesh};
    public Drawmode drawmode;
   public const int mapChunkSize = 241;
   [Range(0,6)]
   public int levelOfDetails;

    
  public float noiseScale;
  public bool autoUpdate = false;
  
  
public terrainType[] regions;
  public int octaves;
  [Range(0,1)]
  public float persistance;
  public float lacunarity;
  public int seed;
  public Vector2 offset;
  
  
  
  public void Start()
  {
    GenerateMap();
    
  }
 

   public void GenerateMap()
   {
      

     
       float[,] noiseMap =NoiseMaker.GenerateNoiseMap(mapChunkSize,mapChunkSize,noiseScale,octaves,persistance,lacunarity,seed,offset);
       Color[] colourMap =new Color[mapChunkSize*mapChunkSize];
        for (int y = 0; y < mapChunkSize ; y++)
       {
           for (int x = 0; x < mapChunkSize ; x++)
           {
               float curentHeight = noiseMap[x,y];
               for (int i = 0; i < regions.Length ; i++)
               {
                   if(curentHeight<=regions[i].height)
                   {
                   


                      //colourMap[y*mapChunkSize+x] =regions[i].texture2D.GetPixel(x,y);
                     colourMap[y*mapChunkSize+x] =regions[i].colour;
                       break;
                   }
               }
               
           }
       }
       MapDisplay display =FindObjectOfType<MapDisplay>();
       if(drawmode == Drawmode.noiseMap )
       {
        display.Drawtexture(TextureMaker.texturefromHeightMap(noiseMap));
       }else if(drawmode == Drawmode.colormap)
       {
           display.Drawtexture(TextureMaker.texturefromColorMap(colourMap,mapChunkSize,mapChunkSize));
       }else if(drawmode== Drawmode.mesh)
       {
           display.DrawMesh(TerrainMeshGenerator.GenerateMesh(noiseMap,heightMultyplayer,meshaAnimationCurve,levelOfDetails),TextureMaker.texturefromColorMap(colourMap,mapChunkSize,mapChunkSize));

       }
       
      
        

   }
   private void OnValidate() {
      /* if(mapChunkSize<1)
       {
           mapChunkSize=1;
       }
       if(mapChunkSize<1)
       {
           mapChunkSize=1;
       }*/
       if(lacunarity<1)
       {
           lacunarity=1;
       }
       if(octaves<0)
       {
           octaves=0;
       }
   }
   [System.Serializable]
   public struct terrainType
   {
      public string name;
      public Texture2D texture2D;
     public float height;
     public Color colour; 
   }
}
