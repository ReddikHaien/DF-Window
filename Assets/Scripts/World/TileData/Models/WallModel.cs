using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using UnityEngine;

namespace ModelImplementation{
    public class WallModel : AbstractModel
    {
        private readonly Model top;
        private readonly Model bottom;
        private readonly Model front;
        private readonly Model back;
        private readonly Model left;
        private readonly Model right;

        public WallModel(){
            var models = ModelLoader.LoadModel("Models/DefaultWall");
            top = models["Top"];
            bottom = models["Bottom"];
            front = models["Front"];
            back = models["Back"];
            left = models["Left"];
            right = models["Right"];
            foreach(var m in models.Values){
                m.CreateNewUvs(randomizePositions: false);
            }
        }

        public override string Name => "wall";

        public override void AddMesh(
            RemoteManager remote,
            World world,
            Vector3Int chunkPosition,
            Vector3Int tilePosition,
            Tile tile,
            List<Vector3> verts,
            List<Vector2> uvs,
            List<int> indices,
            Dictionary<string,Vector4> baseColors,
            MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x * Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(tilePosition.x*Chunk.TILE_WIDTH,tilePosition.y*Chunk.TILE_HEIGHT,tilePosition.z*Chunk.TILE_WIDTH);


            //================ OBS ===============
            //
            // ALLE SIDENE UTENOM TOP OG BOTTOM ER SPEILVENDTE
            //
            //====================================
            
            if (world.IsSideVisible(wp + Vector3Int.up,AbstractShape.Direction.Down)){
                var color = 
                    GetColoring(baseColors,"top") ?? 
                    GetColoring(baseColors,"all") ?? 
                    GetDefaultTexture(remote,materialManager,tile);
                
                top.AddToMesh(verts,uvs,indices,color,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.forward, AbstractShape.Direction.Back)){
                var color = 
                    GetColoring(baseColors,"front") ??
                    GetColoring(baseColors,"sides") ?? 
                    GetColoring(baseColors,"all") ?? 
                    GetDefaultTexture(remote,materialManager,tile);
                
                back.AddToMesh(verts,uvs,indices,color,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.back, AbstractShape.Direction.Front)){
                var color = 
                    GetColoring(baseColors,"back") ??
                    GetColoring(baseColors,"sides") ?? 
                    GetColoring(baseColors,"all") ?? 
                    GetDefaultTexture(remote,materialManager,tile);
                
                front.AddToMesh(verts,uvs,indices,color,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.left, AbstractShape.Direction.Right)){
                var color = 
                    GetColoring(baseColors,"left") ??
                    GetColoring(baseColors,"sides") ?? 
                    GetColoring(baseColors,"all") ?? 
                    GetDefaultTexture(remote,materialManager,tile);
                
                right.AddToMesh(verts,uvs,indices,color,tp);
            }
            if (world.IsSideVisible(wp + Vector3Int.right, AbstractShape.Direction.Left)){
                var color = 
                    GetColoring(baseColors,"right") ??
                    GetColoring(baseColors,"sides") ?? 
                    GetColoring(baseColors,"all") ?? 
                    GetDefaultTexture(remote,materialManager,tile);
                
                left.AddToMesh(verts,uvs,indices,color,tp);
            }
            

            
            if(world.IsSideVisible(wp + Vector3Int.down,AbstractShape.Direction.Up)){
                var color = 
                    GetColoring(baseColors,"top") ?? 
                    GetColoring(baseColors,"all") ?? 
                    GetDefaultTexture(remote,materialManager,tile);
                
                bottom.AddToMesh(verts,uvs,indices,color,tp);
            }
            
        }


        private Vector4 GetDefaultTexture(RemoteManager remote, MaterialManager manager, Tile tile){

            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var material = matdef != null ? manager.GetMaterial2(matdef) : null;
            return material?.DefaultTexture ?? new Vector4(0,0,1,1);
        }
        private Vector4? GetColoring(Dictionary<string,Vector4> baseColors, string name){
            return baseColors.TryGetValue(name, out var x) ? x : null;
        }
         
    }
}


