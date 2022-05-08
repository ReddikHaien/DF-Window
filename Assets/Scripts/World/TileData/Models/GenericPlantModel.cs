using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{

    public class GenericPlantModel : AbstractModel
    {
        private readonly string name;
        
        private readonly Model plant;

        public override string Name => name;

        public GenericPlantModel(string name){
            this.name = name;
            var models = ModelLoader.LoadModel($"Models/{name}");

            this.plant = models["Plant"];
        }

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            FloorModel.AddFloor(remote,world,builder,chunkPosition,tilePosition,tile,baseColors,materialManager);

            var plantColors = baseColors.TryGetValue("plant", out var x) ? x : remote.TileDataManager.textureManager.getUv("DefaultTexture");

            var tp = new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition;

            builder.AddModel(plant,tp,plantColors);

        }
    }
}

