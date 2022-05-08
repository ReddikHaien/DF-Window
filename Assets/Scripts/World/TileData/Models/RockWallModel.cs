using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class RockWallModel : AbstractModel
    {
        private readonly Model top;
        private readonly Model bottom;
        private readonly Model front;
        private readonly Model back;
        private readonly Model left;
        private readonly Model right;

        public RockWallModel(){
            var models = ModelLoader.LoadModel("Models/DefaultWall");
            foreach(var model in models){
                model.Value.CreateNewUvs();
            }
            top = models["Top"];
            bottom = models["Bottom"];
            front = models["Front"];
            back = models["Back"];
            left = models["Left"];
            right = models["Right"];
        }

        public override string Name => "RockWall";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x * Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(tilePosition.x*Chunk.TILE_WIDTH,tilePosition.y*Chunk.TILE_HEIGHT,tilePosition.z*Chunk.TILE_WIDTH);
            
            var pattern = remote.TileDataManager.textureManager.getUv("RockMineralPattern");

            var layer = GetColor(remote, materialManager, tile.LayerMaterial);
            var vein = GetColor(remote, materialManager, tile.VeinMaterial);
            
            //var noVein = remote.GetMaterialDefinition(tile.VeinMaterial).Name == "empty";
            

            if (world.IsSideVisible(wp + Vector3Int.up, AbstractShape.Direction.Down)){
                builder.AddModel(top,tp,layer,pattern,vein);
            }
            if (world.IsSideVisible(wp + Vector3Int.forward, AbstractShape.Direction.Back)){
                builder.AddModel(back,tp,layer,pattern,vein);
            }
            if (world.IsSideVisible(wp + Vector3Int.back, AbstractShape.Direction.Front)){
                builder.AddModel(front,tp,layer,pattern,vein);
            }
            if (world.IsSideVisible(wp + Vector3Int.left, AbstractShape.Direction.Right)){
                builder.AddModel(right,tp,layer,pattern,vein);
            }
            if (world.IsSideVisible(wp + Vector3Int.right, AbstractShape.Direction.Left)){
                builder.AddModel(left,tp,layer,pattern,vein);
            }
            if (world.IsSideVisible(wp + Vector3Int.down, AbstractShape.Direction.Up)){
                builder.AddModel(bottom,tp,layer,pattern,vein);
            }   
        }

        private Vector4 GetColor(RemoteManager remote, MaterialManager manager, MatPair matpair){
            var def = remote.GetMaterialDefinition(matpair);
            var material = manager.GetMaterial2(def);

            if (manager.isDefaultMaterial(material)){
                Chunk.missingEls.TryAdd(def.Name,0);
            }

            return material.DefaultTexture;
        }
    }
}
