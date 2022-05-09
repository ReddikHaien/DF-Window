using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class RampModel : AbstractModel
    {

        private readonly Model edge;
        private readonly Model cornerOut;
        private readonly Model cornerIn;

        public RampModel(){
            var models = ModelLoader.LoadModel("Models/Ramp");
            edge = models.AsEnumerable().First().Value;
        }

        public override string Name => "Ramp";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x*Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition;
            var pz = world.IsSideVisible(wp + Vector3Int.forward,AbstractShape.Direction.Back);
            var nz = world.IsSideVisible(wp + Vector3Int.back,AbstractShape.Direction.Front);
            var px = world.IsSideVisible(wp + Vector3Int.right,AbstractShape.Direction.Left);
            var nx = world.IsSideVisible(wp + Vector3Int.left, AbstractShape.Direction.Right);

            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var material = materialManager.GetMaterial2(matdef).DefaultTexture;

            if (!nz && pz && px && nx){
                builder.AddModel(edge,tp,material);
            }
            else if (!pz && nz && px && nx){
                builder.AddModel(edge,tp,material,yRotation: 180.0f);
            }
            else if (!px && nz && pz && nx){
                builder.AddModel(edge,tp,material,yRotation: -90.0f);
            }
            else if (!nx && nz && pz && px){
                builder.AddModel(edge,tp,material,yRotation: 90.0f);
            }
        }
    }
}

