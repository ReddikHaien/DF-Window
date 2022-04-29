using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TileDataBuilder{    
    public class ModelBaker
    {
        private Dictionary<string,Dictionary<string,ModelData>> models;
        private Dictionary<string, ColoredModelData> completedModels;

        public ModelBaker(){
            models = new Dictionary<string, Dictionary<string, ModelData>>();
            completedModels = new Dictionary<string, ColoredModelData>();
        }

        private void LoadResourceModel(string path){

            var dict = new Dictionary<string,ModelData>();
            
            var model = Resources.Load<GameObject>(path);

            if (model == null){
                throw new System.Exception($"Invalid Model Path {path}");
            }

            GetMeshes(model,dict);

            models.Add(path,dict);
        }

        private static void GetMeshes(GameObject obj, Dictionary<string, ModelData> output){

            if (obj.TryGetComponent<MeshFilter>(out var filter)){
                output[obj.name] = new ModelData{
                    verts = filter.sharedMesh.vertices,
                    indices = filter.sharedMesh.triangles,
                };
            }

            for (int i = 0; i < obj.transform.childCount; i++){
                var child = obj.transform.GetChild(i);
                GetMeshes(child.gameObject,output);
            }
        }
    }

    

    public class ModelData{
        public Vector3[] verts {get; set;}
        public Vector2[] uvs {get; set;}
        public int[] indices {get; set;}
    }

    public class ColoredModelData{
        public ModelData model { get; set; }
        public Vector2 uvPos { get; set; }
        public Vector2 uvSize {get; set; }

        public void AddToModel(List<Vector3> verts, List<Vector2> uvs, List<int> indices, Vector3 pos){
            int vcount = verts.Count;
            verts.AddRange(model.verts.Select(x => x + pos));
            uvs.AddRange(model.uvs.Select(x => x*uvSize + uvPos));
            indices.AddRange( model.indices.Select(x => x + vcount));
        }
    }
    
    public class ModelRecipe{
        public string MeshName { get; set; }
        public Dictionary<string, LoadableTexture> textures { get; set; }
    }
}
