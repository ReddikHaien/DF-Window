using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TileDataBuilder{
    public class TextureBuilder
    {
        private int size;

        private Dictionary<string,LoadableTexture> textures;

        private Dictionary<string,Vector4> uvs;

        public TextureBuilder(){
            size = 1;
            textures = new Dictionary<string, LoadableTexture>();
            uvs = new Dictionary<string, Vector4>();
        }

        public void AddTexture(LoadableTexture texture){
            if (textures.ContainsKey(texture.Name)){
                return;
            }

            textures.Add(texture.Name,texture);
        }

        public Vector4 GetUv(string name) => uvs.TryGetValue(name, out var x) ? x : new Vector4(0,0,1,1);

        public Texture2D CreateTexture(){

            var textures = this.textures.AsEnumerable().ToList();
            textures.Sort((a,b) => 
                b.Value.LoadTexture().width == a.Value.LoadTexture().width ? 
                    b.Value.LoadTexture().height - a.Value.LoadTexture().height : 
                    b.Value.LoadTexture().width - a.Value.LoadTexture().width 
                );
            
            var node = new List<List<(Vector2Int,LoadableTexture)>>();
            node.Add(new List<(Vector2Int, LoadableTexture)>());


            TextureCell root = null;

            for (int i = 0; i <= 31; i++){
                root = new TextureCell{
                    Height = 1 << i,
                    Width = 1 << i,
                    X = 0,
                    Y = 0,
                };
                size = 1 << i;
                try{
                    foreach (var t in textures){
    
                        var texNode = root.Insert(t.Value);
                        if (texNode != null){
                            texNode.Texture = t.Value;
                            texNode.Name = t.Key;
                        }
                        else{
                            throw new Exception("Failed to create texture");
                        }
                    }
                    //We reached this far, meaning we placed all the textures
                    break;
                } catch(Exception){
                    continue;
                }
            }

            
            Debug.Log($"Creating Texture With Size {size}");

            var texture = new Texture2D(size,size,UnityEngine.Experimental.Rendering.DefaultFormat.LDR,UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
            texture.filterMode = FilterMode.Point;
            
            foreach(var (name, tex, x, y) in root.ToEnumerator()){
                var pos = new Vector2(x,y) / (float)size;
                var dim = new Vector2(tex.LoadTexture().width, tex.LoadTexture().height) / (float)size;
                uvs.Add(name,new Vector4(pos.x,pos.y,dim.x,dim.y));

                var colors = tex.LoadTexture().GetPixels32(0);

                texture.SetPixels32(x,y,tex.LoadTexture().width,tex.LoadTexture().height,colors,0);
            }
            
            texture.Apply(false,false);
            return texture;
        }

        private static int GetLineLength(List<(Vector2Int,LoadableTexture)> line) => line.Select(x => x.Item2.LoadTexture().width).Sum();
        
        private static int GetTexHeight(List<List<(Vector2Int,LoadableTexture)>> lines) => lines.Select(x => x.Count > 0 ? x[0].Item2.LoadTexture().height : 0).Sum();
    }

    internal class TextureCell{
        public LoadableTexture Texture { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        
        public TextureCell[] Children;


        public TextureCell Insert(LoadableTexture texture){
            if (Children != null){
                var x = Children[0].Insert(texture);
                if (x != null) return x;

                return Children[1].Insert(texture);
            }
            else{
                if (Texture != null){
                    return null;
                }
                if (Width < texture.LoadTexture().width || Height < texture.LoadTexture().height){
                    return null;
                }
                if (Width == texture.LoadTexture().width && Height == texture.LoadTexture().height){
                    return this;
                }
                this.Children = new TextureCell[]{
                    new TextureCell(),
                    new TextureCell(),
                };
                var dw = Width - texture.LoadTexture().width;
                var dh = Height - texture.LoadTexture().height;

                if (dw > dh){

                    Children[0].X = this.X;
                    Children[0].Y = this.Y;
                    Children[0].Width = texture.LoadTexture().width;
                    Children[0].Height = this.Height;

                    Children[1].X = this.X + texture.LoadTexture().width;
                    Children[1].Y = this.Y;
                    Children[1].Width = dw;
                    Children[1].Height = this.Height;
                
                }
                else{
                    Children[0].X = this.X;
                    Children[0].Y = this.Y;
                    Children[0].Width = this.Width;
                    Children[0].Height = texture.LoadTexture().height;

                    Children[1].X = this.X;
                    Children[1].Y = this.Y + texture.LoadTexture().height;
                    Children[1].Width = this.Width;
                    Children[1].Height = dh;
                }

                return Children[0].Insert(texture);
            }
        }

        public IEnumerable<(string, LoadableTexture, int, int)> ToEnumerator(){
            List<(string,LoadableTexture,int,int)> list = new List<(string, LoadableTexture, int, int)>();
            AddToList(list);
            return list;
        }

        private void AddToList(List<(string, LoadableTexture, int, int)> list){
            if (this.Children != null){
                Children[0].AddToList(list);
                Children[1].AddToList(list);
            }
            if (this.Texture != null){
                list.Add((Name,Texture,X,Y));
            }
        }
    }
}

