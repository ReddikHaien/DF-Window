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

        public override string Name => "Tourmaline";

        public override void AddMesh(RemoteManager remote, World world, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, List<Vector3> verts, List<Vector2> uvs, List<int> indices, Dictionary<string,Vector4> baseColors, MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x*Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(tilePosition.x*Chunk.TILE_WIDTH,tilePosition.y*Chunk.TILE_HEIGHT,tilePosition.z*Chunk.TILE_WIDTH);

            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var color = materialManager.GetMaterial2(matdef).DefaultTexture;

            normal.AddToMesh(verts,uvs,indices,color,tp);
            if (world.IsSideVisible(wp + Vector3Int.down,AbstractShape.Direction.Up)){
                bottom.AddToMesh(verts,uvs,indices,color,tp);
            }
        }
    }
}
