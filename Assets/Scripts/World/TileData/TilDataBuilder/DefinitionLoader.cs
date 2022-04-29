using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

                var defaults = obj.DefaultDefinition;

                foreach(var x in obj.Definitions){
                    var shapes = new Dictionary<RemoteFortressReader.TiletypeShape,(string,string,Dictionary<string,LoadableTexture>)>();

                    var shapeDict = x.Value.Shapes;

                    var defaultShapes = defaults?.Shapes ?? new Dictionary<string, ShapeEntry>();

                    var missing = shapeDict != null ? defaultShapes.Keys.Except(shapeDict.Keys) : defaultShapes.Keys.ToHashSet();
                

                    
                    if (shapeDict != null){
                        foreach(var entry in shapeDict){

                            var defvalues = defaultShapes.TryGetValue(entry.Key,out var dv) ? dv : null;

                            var shapeName = entry.Value.Shape ?? defvalues?.Shape;
                            var modelName = entry.Value.Model ?? defvalues?.Model;

                            var coloring = entry.Value.Coloring;

                            var parsedColors = new Dictionary<string,LoadableTexture>();

                            if (coloring != null){
                                foreach(var color in coloring){
                                    parsedColors[color.Key] = new ResourceTexture(color.Value);
                                }
                            }

                            if (defvalues?.Coloring != null){
                                foreach(var color in defvalues.Coloring){
                                    parsedColors.TryAdd(color.Key,new ResourceTexture(color.Value));
                                }
                            }

                            try{
                                shapes[Enum.Parse<RemoteFortressReader.TiletypeShape>(entry.Key, true)] = (shapeName,modelName,parsedColors);
                            }catch(Exception e){
                                Debug.LogError($"Failed to Create Entry for Shape: {entry.Key}. Error: {e} ");
                            }
                        }    
                    }

                    foreach(var missingShape in missing){
                        var shapeName = defaultShapes[missingShape].Shape ?? "Empty";
                        var modelName = defaultShapes[missingShape].Model ?? "Empty";
                        var coloring = defaultShapes[missingShape].Coloring ?? new Dictionary<string,string>();
                        var id = Enum.Parse<RemoteFortressReader.TiletypeShape>(missingShape, true);
                        var (existingShape, existingModel, existingColors) = shapes.TryGetValue(id, out var existing) ? existing : (null, null, new Dictionary<string, LoadableTexture>());

                        foreach(var color in coloring){
                            existingColors.TryAdd(color.Key,new ResourceTexture(color.Value));
                        }

                        shapes[id] = (existingShape ?? shapeName, existingModel ?? modelName, existingColors);
                    }
                

                    builder.AddNewEntry(
                        x.Key,
                        shapes,
                        ParseCategory(x.Value.Category ?? defaults?.Category ?? "stone"),
                        CreateResourceTexture(x.Value.Texture ?? defaults?.Texture ?? "DefaultTexture")
                    );
                }
            }
        }

        private LoadableTexture CreateResourceTexture(string name) => !string.IsNullOrEmpty(name) ? new ResourceTexture(name) : throw new ArgumentNullException("name");

        private MaterialManager.MaterialCategory? ParseCategory(string category) => !string.IsNullOrEmpty(category) ? Enum.Parse<MaterialManager.MaterialCategory>(category,true) : throw new ArgumentNullException("category");

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
        
        [JsonProperty("defaults")]
        public DefinitionEntry DefaultDefinition {get; set;}

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

