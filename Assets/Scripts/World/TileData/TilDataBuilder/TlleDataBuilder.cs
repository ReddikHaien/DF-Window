using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Linq.Expressions;
using ModelImplementation;

namespace TileDataBuilder{
    public class Builder
    {
        private Dictionary<string,MaterialBuilderEntry> entries;

        private readonly MaterialManager.MaterialCategory defaultCategory;
        private readonly Color32 defaultColor;
        private readonly LoadableTexture defaultTexture;

        private readonly Dictionary<RemoteFortressReader.TiletypeShape,string> defaultModels;


        private readonly Dictionary<string, AbstractShape> shapes;
        private readonly Dictionary<string, AbstractModel> models;
        public Builder(
            MaterialManager.MaterialCategory defaultCategory,
            Color32 defaultColor,
            LoadableTexture defaultTexture,
            IEnumerable<(string,AbstractShape)> shapes,
            IEnumerable<(string,AbstractModel)> models
            
        ){
            entries = new Dictionary<string, MaterialBuilderEntry>();
            this.defaultCategory = defaultCategory;
            this.defaultColor = defaultColor;
            this.defaultTexture = defaultTexture;
            this.shapes = new Dictionary<string, AbstractShape>(shapes.Select(x => KeyValuePair.Create(x.Item1,x.Item2)));
            this.models = new Dictionary<string, AbstractModel>(models.Select(x => KeyValuePair.Create(x.Item1,x.Item2)));
        }

        public void AddNewEntry(
            string name,
            Dictionary<RemoteFortressReader.TiletypeShape,(string,string,Dictionary<string,LoadableTexture>)> models,
            MaterialManager.MaterialCategory? category,
            LoadableTexture defaultTexture
            ){
            entries[name] = new MaterialBuilderEntry{
                Name = name,
                Category = category ?? defaultCategory,
                DefaultTexture = defaultTexture ?? this.defaultTexture,
                Models = models
            };
        }

        public (TextureManager, MaterialManager) BuildManagers(){
            var textureBuilder = new TextureBuilder();

            textureBuilder.AddTexture(new ResourceTexture("RockMineralPattern"));

            foreach(var x in entries){
                textureBuilder.AddTexture(x.Value.DefaultTexture);
                foreach (var y in x.Value.Models){
                    foreach (var z in y.Value.Item3){
                        textureBuilder.AddTexture(z.Value);
                    }
                }
            }
            var texture = textureBuilder.CreateTexture();
            var materials = new Dictionary<string,MaterialEntry2>();
            foreach(var entry in entries){
                var name = entry.Key;
                var obj = entry.Value;

                var defaultTexture = textureBuilder.GetUv(obj.DefaultTexture.Name);

                var models =new Dictionary<RemoteFortressReader.TiletypeShape,(AbstractShape,AbstractModel,Dictionary<string,Vector4>)>(); 

                foreach(var modelEntry in obj.Models){
                    var (shape, model, coloring) = modelEntry.Value;
                    shape = shape ?? "Empty";
                    model = model ?? "Empty";
                    var uvList = new Dictionary<string,Vector4>();
                    var abstractModel = this.models.TryGetValue(model, out var x) ? x : null;
                    var abstractShape = shapes.TryGetValue(shape, out var y) ? y : null;

                    foreach(var color in coloring){
                        uvList[color.Key] = textureBuilder.GetUv(color.Value.Name);
                    }
                    
                    models[modelEntry.Key] = (abstractShape,abstractModel,uvList);
                }

                materials[name] = new MaterialEntry2(
                    defaultTexture,
                    models,
                    entry.Value.Category
                );
            }
            
            var materialManager = new MaterialManager(
            materials,
            new MaterialEntry2(
                textureBuilder.GetUv("DefaultTexture"),
                new Dictionary<RemoteFortressReader.TiletypeShape, (AbstractShape, AbstractModel, Dictionary<string, Vector4>)>(),
                MaterialManager.MaterialCategory.Stone
            ),
            (shapes["Empty"],models["Empty"],new Dictionary<string, Vector4>()));
            var textureManager = new TextureManager(texture, textureBuilder.GetUvList());

            return (textureManager, materialManager);
        }
    }

    public class MaterialBuilderEntry{
        public string Name { get; set; }
        public MaterialManager.MaterialCategory Category { get; set; }
        public LoadableTexture DefaultTexture { get; set; }
        public Dictionary<RemoteFortressReader.TiletypeShape,(string,string,Dictionary<string,LoadableTexture>)> Models { get; set; }

    }
}

