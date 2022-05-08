using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class FloorModel : AbstractModel
    {
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
        }

        public override string Name => "Floor";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var material = materialManager.GetMaterial2(matdef);

            var tp = (Vector3)(new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition);

            builder.AddModel(Top,tp,material.DefaultTexture);
            
        }
    }
}
