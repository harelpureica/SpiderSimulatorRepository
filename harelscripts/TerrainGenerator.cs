using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    //map parameters
    [Range(1f,20000)]
   public int Width;
   [Range(1f,20000)]
   public int height;
   public float heightMultyplayer;
   [Range(1,20)]
   public int octaves;
   [Range(0.001f,200)]
   public float scale;
   [Range(1,5)]
   public float lacunarirty=2f;
   [Range(0.0001f,1)]
   public float persistance=0.5f;
   public Renderer noiseMapPlan;
   public bool autoGenerate=false;
    
   void Start ()
   {
    DisplayNoiseMap();
   }
 private void Update() {
  
   }
   

   public  float[,] GenerateNoiseMap(int width,int height,float scale,float persistance,float lacunarirty)
   {
    
     float halfwidth = Width/2f;
      float halfheight=height/2f;
    float [,] noiseMap=new float[Width,height];
    float maxnoiseheight =float.MinValue;
    float minnoiseheight =float.MaxValue;

   

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < Width; x++)
        {
            float amplitude=1;
            float frequency=1;
            float noiseHeight=0;
            for (int i = 0; i < octaves; i++)
            {
            float sampleX=x/scale*frequency;
            float sampleY=y/scale*frequency;
            float  perlinValue=Mathf.PerlinNoise(sampleX,sampleY);
            noiseHeight=perlinValue*amplitude;
            amplitude*=persistance;
            frequency*=lacunarirty;
            
            

            }
            if(noiseHeight>maxnoiseheight)
            {
              maxnoiseheight=noiseHeight;
            } else if(noiseHeight<minnoiseheight)
            {
              minnoiseheight=noiseHeight;
            }
            noiseMap[x,y]=noiseHeight;
            
        }
    }
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < Width ; x++)
      {
        noiseMap[x,y]=Mathf.InverseLerp(minnoiseheight,maxnoiseheight,noiseMap[x,y]);
      }
    }
    return noiseMap;
   }
   
   public Texture2D GenerateTextureFromNooiseMap()
   {
    Color[] colorMap=new Color[Width*height];
    
    for (int y = 0; y < height ; y++)
    {
        for (int x = 0; x < Width; x++)
        {
           colorMap[(y*Width)+x]= Color.Lerp(Color.black,Color.white,GenerateNoiseMap(Width,height,scale,persistance,lacunarirty)[x,y]);
        }
    }
    Texture2D texture2D=new Texture2D(Width,height);
    texture2D.SetPixels(colorMap);
    texture2D.Apply();
    return texture2D;
   }
   public void DisplayNoiseMap()
   {
        noiseMapPlan.sharedMaterial.mainTexture=GenerateTextureFromNooiseMap();
        noiseMapPlan.transform.localScale=new Vector3(Width,1,height);
   }
}

