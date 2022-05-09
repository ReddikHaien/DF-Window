using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModelLoading;
using UnityEngine;

public class ChunkMeshBuilder{
    private readonly List<Vector3> verts;
    private readonly List<Vector2> baseUvs;
    private readonly List<Vector2> layer1Uvs;
    private readonly List<Vector2> layer2Uvs;
    private readonly List<Color32> colors;
    private readonly List<int> indices;

    public ChunkMeshBuilder(){
        verts = new List<Vector3>();
        baseUvs = new List<Vector2>();
        layer1Uvs = new List<Vector2>();
        layer2Uvs = new List<Vector2>();
        indices = new List<int>();
        colors = new List<Color32>();
    }

    public void AddModel(
        Model model, 
        Vector3 position,
        Vector4 baseLayer,
        Vector4? layer1 = null,
        Vector4? layer2 = null,
        Color32? color = null,
        float yRotation = 0.0f){
        
        var quat = Quaternion.AngleAxis(yRotation,Vector3.up);

        var center = new Vector3(1.5f,0.0f,1.5f);

        int icount = this.verts.Count;
        Vector4 l1 = layer1 ?? baseLayer;
        Vector4 l2 = layer2 ?? baseLayer;
        Color32 c = color ?? new Color32(255,255,255,255);

        var venum = model.verts.AsEnumerable();
        var uvenum = model.uvs.AsEnumerable();
        verts.AddRange(venum.Select(x => {
            var d = x - center;
            d = quat * d;

            return (d + center) + position;
            }));

        baseUvs.AddRange(uvenum.Select(x => x*new Vector2(baseLayer.z,baseLayer.w) + new Vector2(baseLayer.x,baseLayer.y)));
        layer1Uvs.AddRange(uvenum.Select(x => x*new Vector2(l1.z,l1.w) + new Vector2(l1.x,l1.y)));
        layer2Uvs.AddRange(uvenum.Select(x => x*new Vector2(l2.z,l2.w) + new Vector2(l2.x,l2.y)));

        colors.AddRange(venum.Select(x => c));

        indices.AddRange(model.indices.AsEnumerable().Select(x => x + icount));
    }

    public void BuildMesh(Mesh mesh){
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.uv = baseUvs.ToArray();
        mesh.uv2 = layer1Uvs.ToArray();
        mesh.uv3 = layer2Uvs.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.colors32 = colors.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }
}