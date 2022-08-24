using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMaker : MonoBehaviour
{
     public static Texture2D texturefromColorMap(Color[] colormap,int width, int height)
  {
      Texture2D texture =new Texture2D(width,height);
      //texture.filterMode=FilterMode.Point;

      texture.wrapMode = TextureWrapMode.Clamp;
      texture.SetPixels(colormap);
      texture.Apply();
      return texture;
      

  }
  public static Texture2D texturefromHeightMap(float[,] heightmap)
  {
      int width = heightmap.GetLength(0);
   int height = heightmap.GetLength(1);
   Texture2D  texture = new Texture2D(width,height);
   Color[]colourmap =new Color[width*height];
   for (int y = 0; y < height ; y++)
   {
       for (int x = 0; x < width ; x++)
       {
           colourmap[y*width+x]=Color.Lerp(Color.black,Color.white,heightmap[x,y]);
       }
   }
  return texturefromColorMap(colourmap,width,height);

  }
}
