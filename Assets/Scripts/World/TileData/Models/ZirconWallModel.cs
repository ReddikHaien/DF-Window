using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;


namespace ModelImplementation{
    public class ZirconWallModel : AbstractModel
    {
        private readonly Model model;
        
        public ZirconWallModel(){
            var models = ModelLoader.LoadModel("Models/ZirconWall");
            model = models["Normal"];
            model.CreateNewUvs();
        }

        public override string Name => "ZirconWall";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var material = materialManager.GetMaterial2(matdef);
            builder.AddModel(
                model,
                new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition,
                material.DefaultTexture);
        }
    }

}
