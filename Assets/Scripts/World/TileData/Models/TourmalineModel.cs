using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation
{
    public class TourmalineModel : AbstractModel
    {
        private Model normal;
        private Model bottom;

        public TourmalineModel(){
            var model = ModelLoader.LoadModel("Models/TourmalineWall");
            normal = model["Normal"];
            bottom = model["Bottom"];

            normal.CreateNewUvs(randomizePositions: false);
            bottom.CreateNewUvs();
        }

        public override string Name => "TourmalineWall";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x*Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(tilePosition.x*Chunk.TILE_WIDTH,tilePosition.y*Chunk.TILE_HEIGHT,tilePosition.z*Chunk.TILE_WIDTH);

            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var color = materialManager.GetMaterial2(matdef).DefaultTexture;

            builder.AddModel(normal,tp,color);
            if (world.IsSideVisible(wp + Vector3Int.down,AbstractShape.Direction.Up)){
                builder.AddModel(bottom,tp,color);
            }
        }
    }
}
