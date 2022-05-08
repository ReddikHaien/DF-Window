using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class FloorModel : AbstractModel
    {
        private static FloorModel instance;
        private readonly Model Top;
        private readonly Model Bottom;
        private readonly Model Left;
        private readonly Model Right;
        private readonly Model Front;
        private readonly Model Back;

        public FloorModel(){
            var models = ModelLoader.LoadModel("Models/Floor");
            Top = models["Top"];
            Bottom = models["Bottom"];
            Left = models["Left"];
            Right = models["Right"];
            Front = models["Front"];
            Back = models["Back"];
            instance = this;
        }

        public override string Name => "Floor";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var material = materialManager.GetMaterial2(matdef);

            var wp = new Vector3Int(chunkPosition.x * Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = (Vector3)(new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition);

            builder.AddModel(Top,tp,material.DefaultTexture);
            
            var color = material.DefaultTexture;

            if (world.IsSideVisible(wp + Vector3Int.forward, AbstractShape.Direction.Back)){
                builder.AddModel(Back,tp,color);                
            }
            if (world.IsSideVisible(wp + Vector3Int.back, AbstractShape.Direction.Front)){
                builder.AddModel(Front,tp,color);                
            }
            if (world.IsSideVisible(wp + Vector3Int.left, AbstractShape.Direction.Right)){
                builder.AddModel(Left,tp,color);
            }
            if (world.IsSideVisible(wp + Vector3Int.right, AbstractShape.Direction.Left)){
                builder.AddModel(Right,tp,color);    
            }
            if(world.IsSideVisible(wp + Vector3Int.down,AbstractShape.Direction.Up)){
                builder.AddModel(Bottom,tp,color);
            }
        }

        public static void AddFloor(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager){
            instance.AddMesh(
                remote,
                world,
                builder,
                chunkPosition,
                tilePosition,
                tile,
                baseColors,
                materialManager
            );
        }
    }
}
