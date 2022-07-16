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
            var cornerOutModels = ModelLoader.LoadModel("Models/RampCornerOutwards");
            cornerOut = cornerOutModels.AsEnumerable().First().Value;
            var cornerInModels = ModelLoader.LoadModel("Models/RampCornerInwards");
            cornerIn = cornerInModels.AsEnumerable().First().Value;
        }

        public override string Name => "Ramp";

        public override void AddMesh(RemoteManager remote, World world, ChunkMeshBuilder builder, Vector3Int chunkPosition, Vector3Int tilePosition, Tile tile, Dictionary<string, Vector4> baseColors, MaterialManager materialManager)
        {
            var wp = new Vector3Int(chunkPosition.x*Chunk.WIDTH,chunkPosition.y,chunkPosition.z*Chunk.WIDTH) + tilePosition;
            var tp = new Vector3Int(Chunk.TILE_WIDTH,Chunk.TILE_HEIGHT,Chunk.TILE_WIDTH) * tilePosition;
            
            var nz = isSideTall(getShapeAt(remote,world,wp + Vector3Int.back));
            var pz = isSideTall(getShapeAt(remote,world,wp + Vector3Int.forward));
            var nx = isSideTall(getShapeAt(remote,world,wp + Vector3Int.left));
            var px = isSideTall(getShapeAt(remote,world,wp + Vector3Int.right));
            
            var nznx = isSideTall(getShapeAt(remote,world,wp + Vector3Int.back + Vector3Int.left));
            var nzpx = isSideTall(getShapeAt(remote,world,wp + Vector3Int.back + Vector3Int.right));
            var pznx = isSideTall(getShapeAt(remote,world,wp + Vector3Int.forward + Vector3Int.left));
            var pzpx = isSideTall(getShapeAt(remote,world,wp + Vector3Int.forward + Vector3Int.right));

            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
            var material = materialManager.GetMaterial2(matdef).DefaultTexture;

            if (nz && !pz && !px && !nx){
                builder.AddModel(edge,tp,material);
            }
            else if (pz && !nz && !px && !nx){
                builder.AddModel(edge,tp,material,yRotation: 180.0f);
            }
            else if (px && !nz && !pz && !nx){
                builder.AddModel(edge,tp,material,yRotation: -90.0f);
            }
            else if (nx && !nz && !pz && !px){
                builder.AddModel(edge,tp,material,yRotation: 90.0f);
            }
            else{
                //00#
                //0|0
                //000
                if (pzpx && !nzpx && !pznx && !nznx && !px && !pz){
                    builder.AddModel(cornerOut,tp,material, yRotation: 180.0f);
                }
                //#00
                //0|0
                //000
                else if (nzpx && !pzpx && !nznx && !pznx && !px && !nz){
                    builder.AddModel(cornerOut,tp,material, yRotation: -90.0f);    
                }
                //000
                //0|0
                //#00
                else if (nznx && !pzpx && !pznx && !nzpx && !nx && !nz){
                    builder.AddModel(cornerOut,tp,material);    
                }
                //000
                //0|0
                //00#
                else if (pznx && !pzpx && !nznx && !nzpx && !nx && !pz){
                    builder.AddModel(cornerOut,tp,material, yRotation: 90.0f);    
                }
                //0##
                //0|#
                //000
                else if (pzpx && pz && px && !nzpx){
                    builder.AddModel(cornerIn,tp,material,yRotation: 180.0f);
                }
                //000
                //0|#
                //0##
                else if (pznx && pz && nx && !nzpx){
                    builder.AddModel(cornerIn,tp,material,yRotation: 90.0f);
                }
                //000
                //#|0
                //##0
                else if (nznx && nz && nx && !pzpx){
                    builder.AddModel(cornerIn,tp,material);
                }
                //##0
                //#|0
                //000
                else if (nzpx && nz && px && !pznx){
                    builder.AddModel(cornerIn,tp,material,yRotation: -90.0f);
                }
                else{
                    FloorModel.AddFloor(remote,world,builder,chunkPosition,tilePosition,tile,baseColors,materialManager);
                }
            }
        }

        private RemoteFortressReader.TiletypeShape getShapeAt(RemoteManager remote, World world, Vector3Int wp){
            var tile = world.GetTile(wp.x,wp.y,wp.z);
            return remote.GetTiletype(tile.TileType).Shape;
        }

        private bool isSideTall(RemoteFortressReader.TiletypeShape shape) => shape switch
        {
            RemoteFortressReader.TiletypeShape.Wall => true,
            _ => false
        };
    }
}

