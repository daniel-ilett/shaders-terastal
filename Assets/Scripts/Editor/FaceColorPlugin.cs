#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Based on: https://forum.unity.com/threads/save-rendertexture-or-texture2d-as-image-file-utility.1325130/
public class FaceColorPlugin
{
    public static void GenerateColors(string filepath, int width, int height, Mesh mesh)
    {
        var pixelCount = width * height;
        var pixels = new Color[pixelCount];
        
        int[] triangles = mesh.triangles;
        Vector2[] uvs = mesh.uv;

        for(int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int vert1 = mesh.triangles[i];
            int vert2 = mesh.triangles[i + 1];
            int vert3 = mesh.triangles[i + 2];

            Vector2 uv1 = Mod(uvs[triangles[i]]);
            Vector2 uv2 = Mod(uvs[triangles[i + 1]]);
            Vector2 uv3 = Mod(uvs[triangles[i + 2]]);

            float rand = Random.value;
            int minX = (int)(Mathf.Min(uv1.x, Mathf.Min(uv2.x, uv3.x)) * width);
            int maxX = (int)(Mathf.Min((Mathf.Max(uv1.x, Mathf.Max(uv2.x, uv3.x)) * width), width));
            int minY = (int)(Mathf.Min(uv1.y, Mathf.Min(uv2.y, uv3.y)) * height);
            int maxY = (int)(Mathf.Min((Mathf.Max(uv1.y, Mathf.Max(uv2.y, uv3.y)) * height), height));

            for(int x = minX; x <= maxX; ++x)
            {
                for(int y = minY; y <= maxY; ++y)
                {
                    Vector2 point = new Vector2(x, y);
                    if(PointIsInTriangle(uv1 * width, uv2 * width, uv3 * width, point))
                    {
                        pixels[x + y * width] = new Color(rand, rand, rand, 1.0f);
                    }
                }
            }
        }

        var tex = new Texture2D(width, height);
        tex.SetPixels(pixels);
        tex.Apply(true, false);

        SaveTextureToFile(tex, filepath);
    }

    private static Vector2 Mod(Vector2 inVector)
    {
        return new Vector2(inVector.x % 1.0f, inVector.y % 1.0f);
    }

    // Based on: https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle
    private static float Sign(Vector2 v1, Vector2 v2, Vector2 v3)
    {
        return (v1.x - v3.x) * (v2.y - v3.y) - (v2.x - v3.x) * (v1.y - v3.y);
    }
    
    private static bool PointIsInTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 point)
    {
        float d1, d2, d3;
        bool hasNeg, hasPos;

        d1 = Sign(point, v1, v2);
        d2 = Sign(point, v2, v3);
        d3 = Sign(point, v3, v1);

        hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);
        return !(hasNeg && hasPos);
    }

    private static void SaveTextureToFile(Texture2D tex, string filepath)
    {
        string uniqueFilepath = AssetDatabase.GenerateUniqueAssetPath(filepath);
        System.IO.File.WriteAllBytes(uniqueFilepath, tex.EncodeToPNG());

        AssetDatabase.Refresh();
        Object file = AssetDatabase.LoadAssetAtPath(uniqueFilepath, typeof(Texture2D));
        Debug.Log($"Texture saved to [{uniqueFilepath}]", file);
    }
}
#endif
