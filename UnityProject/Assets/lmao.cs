using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class lmao : MonoBehaviour
{
    public RenderTexture texture;
    // Start is called before the first frame update
    void Update()
    {
        RenderTexture.active = texture;
        Texture2D tex = new Texture2D(2024, 2024);
        tex.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes("C:/Users/Marin/Pictures/img.png", bytes);
    }
}
