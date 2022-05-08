using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class WoodWallModel : AbstractModel
    {

        private readonly Model top;
        private readonly Model bottom;

        private readonly Model middle;

        public WoodWallModel(){
            var models = ModelLoader.LoadModel("Models/TreeTrunk");
            top = models["Top"];
            bottom = models["Bottom"];
            middle = models["Middle"];
        }
        public override string Name => "WoodWall";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x * Chunk.WIDTH, chunkPosition.y, chunkPosition.z * Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(tilePosition.x*Chunk.TILE_WIDTH,tilePosition.y*Chunk.TILE_HEIGHT,tilePosition.z*Chunk.TILE_WIDTH);

            var bark = materialManager.GetMaterial2(remote.GetMaterialDefinition(tile.BaseMaterial)).DefaultTexture;
            var center = baseColors.TryGetValue("wood", out var x) ? x : bark;

            builder.AddModel(middle,tp,bark);
            if (world.IsSideVisible(wp + Vector3Int.up, AbstractShape.Direction.Down)){
                builder.AddModel(top,tp,center);
            }
        
            if (world.IsSideVisible(wp + Vector3Int.down, AbstractShape.Direction.Up)){
                builder.AddModel(bottom,tp,center);
            }
        }
    }
}

