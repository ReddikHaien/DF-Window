using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager
{
    private Texture2D texture;
    private readonly Dictionary<string,Vector4> uvs;
    public TextureManager(Texture2D texture, Dictionary<string,Vector4> uvs){
        this.texture = texture;
        this.uvs = uvs;
    }


    public Vector4 getUv(string name) => uvs.TryGetValue(name, out var x) ? x : uvs["DefaultTexture"];
    
    public Vector2 AddColor(Color32 c){
        return Vector2.zero;   
    }

    public Texture2D CreateTexture(){
        return texture;
    }
}
