using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace TileDataBuilder{
    public class DefinitionLoader
    {
        public void LoadBaseDefinitions(Builder builder){
            var baseDefinitions = Resources.LoadAll<TextAsset>("Definitions");
            foreach(var def in baseDefinitions){
                var obj = JsonConvert.DeserializeObject<DefinitionObject>(def.text);
                
                
                Debug.Log(obj.Definitions.Count);

                foreach(var x in obj.Definitions){
                    var shapes = new Dictionary<RemoteFortressReader.TiletypeShape,(string,string,Dictionary<string,LoadableTexture>)>();

                    var shapeDict = x.Value.Shapes;
                    if (shapeDict != null){
                        foreach(var entry in shapeDict){
                            var shapeName = entry.Value.Shape ?? "Empty";
                            var modelName = entry.Value.Model ?? "Empty";

                            var coloring = entry.Value.Coloring;

                            var parsedColors = new Dictionary<string,LoadableTexture>();

                            if (coloring != null){
                                foreach(var color in coloring){
                                    parsedColors[color.Key] = new ResourceTexture(color.Value);
                                }
                            }

                            try{
                                shapes[Enum.Parse<RemoteFortressReader.TiletypeShape>(entry.Key, true)] = (shapeName,modelName,parsedColors);
                            }catch(Exception e){
                                Debug.LogError($"Failed to Create Entry for Shape: {entry.Key}. Error: {e} ");
                            }
                        }
                    }

                    builder.AddNewEntry(
                        x.Key,
                        shapes,
                        ParseCategory(x.Value.Category),
                        CreateResourceTexture(x.Value.Texture)
                    );
                }
            }
        }

        private LoadableTexture CreateResourceTexture(string name) => !string.IsNullOrEmpty(name) ? new ResourceTexture(name) : null;

        private MaterialManager.MaterialCategory? ParseCategory(string category) => !string.IsNullOrEmpty(category) ? Enum.Parse<MaterialManager.MaterialCategory>(category,true) : null;

        private Color32? ParseRGB(string hexCode){

            if (!string.IsNullOrEmpty(hexCode)){
                byte r = byte.Parse(hexCode.Substring(0,2),System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hexCode.Substring(2,2),System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hexCode.Substring(4,2),System.Globalization.NumberStyles.HexNumber);

                return new Color32(r,g,b,255);    
            }
            else{
                return null;
            }
        }   
    }

    public class DefinitionObject{

        [JsonProperty("definitions")]
        public Dictionary<string,DefinitionEntry> Definitions {get; set;}
    }

    public class DefinitionEntry{

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("defaultTexture")]
        public string Texture { get; set; }

        [JsonProperty("shapes")]
        public Dictionary<string,ShapeEntry> Shapes { get; set; }
    }

    public class ShapeEntry{
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("coloring")]
        public Dictionary<string,string> Coloring { get; set; }
        
        [JsonProperty("shape")]
        public string Shape { get; set; }

    }
}

