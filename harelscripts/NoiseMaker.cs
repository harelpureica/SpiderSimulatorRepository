using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMaker 
{
  public static float[,] GenerateNoiseMap(int width,int height,float scale,int octaves,float persistance,float lacunarity,int seed,Vector2 offset)
  {

      System.Random prng =new System.Random(seed);
      Vector2[] octaveOffset =new Vector2[octaves];
      for (int i = 0; i < octaves; i++)
      {
        float offsetx = prng.Next(-100000,100000)+offset.x;
         float offsety = prng.Next(-100000,100000)+offset.y;
         octaveOffset[i]= new Vector2(offsetx,offsety);
      }
      float[,] noiseMap = new float[width,height];
      if(scale<=0)
      {
          scale = 0.0001f;
      }

      float maxNoiseHeight = float.MinValue;
      float minNoiseHeight = float.MaxValue;
      float halfwidth = width/2f;
      float halfheight=height/2f;
      
      
      for (int y = 0; y < height ; y++)
      {
          for (int x = 0; x < width ; x++)
          {
              float amplitude =1;
              float frequensy = 1;
              float noiseHeight =0;
              for (int i = 0; i < octaves ; i++)
              {
                  //perlin spots
                  float samplex =(x-halfwidth)/scale*frequensy+octaveOffset[i].x;
              float sampley =(y-halfheight)/scale*frequensy+octaveOffset[i].y;
              //perlin value - *2-1 =make it go from -1 to 1 
              float perlinValue = Mathf.PerlinNoise(samplex,sampley)*2-1;
              //for next octaves
              noiseHeight += perlinValue*amplitude;
              amplitude*=persistance;
              frequensy*=lacunarity;
              
              }
              if(noiseHeight>maxNoiseHeight)
              {
                  maxNoiseHeight =noiseHeight;
              }else if(noiseHeight<minNoiseHeight)
              {
                  minNoiseHeight = noiseHeight;
              }
             noiseMap[x,y ]=noiseHeight;

          }

      } 
      for (int y = 0; y < height ; y++)
      {
          for (int x = 0; x < width ; x++)
          {
               noiseMap[x,y]=Mathf.InverseLerp(minNoiseHeight,maxNoiseHeight,noiseMap[x,y]);
          }
          
      }
      
      return noiseMap;
  }
}
