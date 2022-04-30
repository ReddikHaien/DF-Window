using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class RockWallModel : AbstractModel
    {
        private readonly (Model, Model) top;
        private readonly (Model, Model) bottom;
        private readonly (Model, Model) front;
        private readonly (Model, Model) back;
        private readonly (Model, Model) left;
        private readonly (Model, Model) right;

        public RockWallModel(){
            var models = ModelLoader.LoadModel("Models/RockWall");
            foreach(var model in models){
                model.Value.CreateNewUvs();
            }
            top = (models["Top"],models["MineralTop"]);
            bottom = (models["Bottom"],models["MineralBottom"]);
            front = (models["Front"],models["MineralFront"]);
            back = (models["Back"],models["MineralBack"]);
            left = (models["Left"],models["MineralLeft"]);
            right = (models["Right"],models["MineralRight"]);
        }

        public override string Name => "RockWall";

        public override void AddMesh(RemoteManager remote, World world, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, List<Vector3> verts, List<Vector2> uvs, List<int> indices, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x * Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(tilePosition.x*Chunk.TILE_WIDTH,tilePosition.y*Chunk.TILE_HEIGHT,tilePosition.z*Chunk.TILE_WIDTH);
            
            var layer = GetColor(remote, materialManager, tile.LayerMaterial);
            var vein = GetColor(remote, materialManager, tile.VeinMaterial);

            if (world.IsSideVisible(wp + Vector3Int.up, AbstractShape.Direction.Down)){
                top.Item1.AddToMesh(verts,uvs,indices,layer,tp);
                top.Item2.AddToMesh(verts,uvs,indices,vein,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.down, AbstractShape.Direction.Up)){
                bottom.Item1.AddToMesh(verts,uvs,indices,layer,tp);
                bottom.Item2.AddToMesh(verts,uvs,indices,vein,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.forward, AbstractShape.Direction.Back)){
                back.Item1.AddToMesh(verts,uvs,indices,layer,tp);
                back.Item2.AddToMesh(verts,uvs,indices,vein,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.back, AbstractShape.Direction.Front)){
                front.Item1.AddToMesh(verts,uvs,indices,layer,tp);
                front.Item2.AddToMesh(verts,uvs,indices,vein,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.left, AbstractShape.Direction.Right)){
                right.Item1.AddToMesh(verts,uvs,indices,layer,tp);
                right.Item2.AddToMesh(verts,uvs,indices,vein,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.right, AbstractShape.Direction.Left)){
                left.Item1.AddToMesh(verts,uvs,indices,layer,tp);
                left.Item2.AddToMesh(verts,uvs,indices,vein,tp);
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
