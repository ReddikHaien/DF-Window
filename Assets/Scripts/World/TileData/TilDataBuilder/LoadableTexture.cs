using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileDataBuilder{
    public abstract class LoadableTexture
    {
        public abstract string Name { get; }
        public abstract Texture2D LoadTexture();
    }

    public class ResourceTexture : LoadableTexture
    {
        private readonly string name;
        private Texture2D loadedTexture;
        public override string Name => name;

        public ResourceTexture(string name){
            this.name = name;
        }

        public override Texture2D LoadTexture()
        {
            if (loadedTexture == null){
                loadedTexture = Resources.Load<Texture2D>($"Textures/{name}");
                if (loadedTexture == null){
                    throw new System.Exception($"Invalid texture name {name}");
                }
            }

            return loadedTexture;
        }
    }
}


