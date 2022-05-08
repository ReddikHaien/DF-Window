using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;


namespace ModelImplementation{
    public class GenericFruitBushModel : AbstractModel
    {
        private readonly string name;
        
        private readonly Model bush;
        private readonly Model fruit;

        public override string Name => name;

        public GenericFruitBushModel(string name){
            this.name = name;
            var models = ModelLoader.LoadModel($"Models/{name}");

            this.bush = models["Bush"];
            this.fruit = models["Fruit"];
        }

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            FloorModel.AddFloor(remote,world,builder,chunkPosition,tilePosition,tile,baseColors,materialManager);

            var bushColors = baseColors.TryGetValue("bush", out var x) ? x : remote.TileDataManager.textureManager.getUv("DefaultTexture");
            var fruitColors = baseColors.TryGetValue("fruit", out var y) ? y : remote.TileDataManager.textureManager.getUv("DefaultTexture");

            var tp = new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition;

            builder.AddModel(bush,tp,bushColors);
            builder.AddModel(fruit,tp,fruitColors);

        }
    }
}

