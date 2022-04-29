using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager
{
    private Texture2D texture;

    public TextureManager(Texture2D texture){
        this.texture = texture;
    }

    public Vector2 AddColor(Color32 c){
        return Vector2.zero;   
    }

    public Texture2D CreateTexture(){
        return texture;
    }
}
