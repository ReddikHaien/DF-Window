using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace ModelLoading{
    public static class ModelLoader
    {
        public static Dictionary<string,Model> LoadModel(string path){

            var dict = new Dictionary<string,Model>();
            
            var model = Resources.Load<GameObject>(path);

            if (model == null){
                throw new System.Exception($"Invalid Model Path {path}");
            }

            GetMeshes(model,dict);

            return dict;
        }

        private static void GetMeshes(GameObject obj, Dictionary<string, Model> output){

            if (obj.TryGetComponent<MeshFilter>(out var filter)){
                output[obj.name] = new Model{
                    verts = filter.sharedMesh.vertices,
                    indices = filter.sharedMesh.triangles,
                    uvs = filter.sharedMesh.uv
                };
            }

            for (int i = 0; i < obj.transform.childCount; i++){
                var child = obj.transform.GetChild(i);
                GetMeshes(child.gameObject,output);
            }
        }
    }

    public class Model{
        public Vector3[] verts;
        public Vector2[] uvs;
        public int[] indices;
        public void AddToMesh(List<Vector3> verts, List<Vector2> uvs, List<int> indices, Vector2 color, int x, int y, int z){
            var p = new Vector3(x,y,z);
            var count = verts.Count;
            verts.AddRange(this.verts.AsEnumerable().Select(x => x + p));
            uvs.AddRange(this.verts.AsEnumerable().Select(_ => color));
            indices.AddRange(this.indices.AsEnumerable().Select(x => x + count));
        }

        public void AddToMesh(List<Vector3> verts, List<Vector2> uvs, List<int> indices, Vector4 color, Vector3Int position){
            var p = (Vector3)position;
            var cs = new Vector2(color.z,color.w);
            var cp = new Vector2(color.x,color.y);
            var count = verts.Count;
            verts.AddRange(this.verts.AsEnumerable().Select(x => x + p));
            uvs.AddRange(this.uvs.AsEnumerable().Select(x => x*cs+cp));
            indices.AddRange(this.indices.AsEnumerable().Select(x => x + count));
        }

        public void CreateNewUvs(bool randomizePositions = false, float scale = 6.0f){
            var uvs = new Vector2[verts.Length];
            for (int i = 0; i < indices.Length; i += 3){
                var a = verts[indices[i]];
                var b = verts[indices[i+1]];
                var c = verts[indices[i+2]];
                var center = (a + b + c) / 3.0f;
                a -= center;
                b -= center;
                c -= center;

                var ac = c - a;
                var ab = b - a;

                var normal = Vector3.Cross(ac, ab).normalized;

                // var mat1 = RotateAlign(normal,Vector3.up);
                var matz = GetRotation(normal);

                var mat = matz;
                var ta = RotatePoint(a,mat);
                var tb = RotatePoint(b,mat);
                var tc = RotatePoint(c,mat);

                var uva = new Vector2(ta.x*2.0f,ta.y);
                var uvb = new Vector2(tb.x*2.0f,tb.y);
                var uvc = new Vector2(tc.x*2.0f,tc.y);

                uva = (uva) / scale;
                uvb = (uvb) / scale;
                uvc = (uvc) / scale;

                var lowest = Vector2.Min(uva,Vector2.Min(uvb,uvc));
                
                uva -= lowest;
                uvb -= lowest;
                uvc -= lowest;

                if (randomizePositions){
                    var highest = Vector2.Max(uva,Vector2.Max(uvb,uvc));
                    var rem = Vector2.one - highest;
                    var rand = new Vector2(Random.Range(0.0f,rem.x),Random.Range(0.0f,rem.y));
                    uva += rand;
                    uvb += rand;
                    uvc += rand;
                }

                uvs[indices[i]] = uva;
                uvs[indices[i+1]] = uvb;
                uvs[indices[i+2]] = uvc;



                //Debug.Log($"{normal} =>{RotatePoint(normal,mat)}: v {a + center} {b + center} {c + center} Uv {uvs[indices[i]]} {uvs[indices[i+1]]} {uvs[indices[i+2]]}");
            
                
            }
            this.uvs = uvs;
        }

        private Matrix4x4 GetRotation(Vector3 normal) => Matrix4x4.Rotate(Quaternion.LookRotation(-normal,Vector3.up));

        private Matrix4x4 RotateAlign(Vector3 a, Vector3 b){


            var axis = (Vector3.Cross(a, b)).normalized;
            var dot = Mathf.Clamp(Vector3.Dot(a,b),-1.0f,1.0f);
            var rads = Mathf.Acos(dot);
            return Matrix4x4.Rotate(Quaternion.AngleAxis(Mathf.Rad2Deg*rads,axis));
        }

        private Vector3 RotatePoint(Vector3 point, Matrix4x4 rot) =>rot * new Vector4(point.x,point.y,point.z,0.0f);
    }
}