using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class SimpleModel : AbstractModel{
        private readonly Model model;
        private readonly string name;
    
        public SimpleModel(string name, bool createUvs = false){
            var models = ModelLoader.LoadModel($"Models/{name}");
            model = models.Values.AsEnumerable().First();

            if (createUvs) model.CreateNewUvs();
            this.name = name;
        }

        public override string Name => name;

        public override void AddMesh(RemoteManager remote, World world, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, List<Vector3> verts, List<Vector2> uvs, List<int> indices, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var material = materialManager.GetMaterial2(matdef);
            model.AddToMesh(verts,uvs,indices,material.DefaultTexture,new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition);
        }
    }
}