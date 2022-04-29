using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using RemoteFortressReader;
using UnityEngine;

public class WallShape : AbstractShape
{
    private Model crystalModel;

    private Model woodTopModel;
    private Model woodBotModel;
    private Model woodMiddleModel;

    public WallShape(){

        var models = ModelLoader.LoadModel("Models/Crystal");

        crystalModel = models["default"];

        var trunkModel = ModelLoader.LoadModel("Models/TreeTrunk");
        woodBotModel = trunkModel["Bottom"];
        woodTopModel = trunkModel["Top"];
        woodMiddleModel = trunkModel["Middle"];
    }

    public override bool SideVisible(World world, Vector3Int position, Direction direction)
    {
        return false;
    }

    public override void CreateMesh(World world, Tiletype type, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> inds, MaterialManager materialManager)
    {

    }

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        var remote = RemoteManager.Instance;

        var material = remote.GetMaterialDefinition(tile.Material);

        if (material == null){
            throw new System.Exception($"Could Not Find Material with MatPair {tile.Material.ToString()}");
        }

        var entry = materialManager.GetMaterial(material);

        var category = entry.Category;

        var baseColor = entry.BaseColor;

        switch(category){
            case MaterialManager.MaterialCategory.Stone: {
                CreateWallShape(world,tile,chunkPosition,x,y,z,verts,uvs,indices,baseColor);
            } break;
            case MaterialManager.MaterialCategory.Wood: {
                
                var woodMeatColor = entry.SubColor1;
                
                if (world.GetVisibilityAmt(
                    chunkPosition.x*Chunk.WIDTH + x,
                    chunkPosition.y + y + 1,
                    chunkPosition.z*Chunk.WIDTH + z) == World.Visibility.Empty){
                    woodTopModel.AddToMesh(verts,uvs,indices,woodMeatColor,
                    x*Chunk.TILE_WIDTH, y*Chunk.TILE_HEIGHT, z*Chunk.TILE_WIDTH);
                }

                if (world.GetVisibilityAmt(
                    chunkPosition.x*Chunk.WIDTH + x,
                    chunkPosition.y + y - 1,
                    chunkPosition.z*Chunk.WIDTH + z) == World.Visibility.Empty){
                    woodBotModel.AddToMesh(verts,uvs,indices,woodMeatColor,
                    x*Chunk.TILE_WIDTH, y*Chunk.TILE_HEIGHT, z*Chunk.TILE_WIDTH);
                }

                woodMiddleModel.AddToMesh(verts,uvs,indices,baseColor,
                x*Chunk.TILE_WIDTH, y*Chunk.TILE_HEIGHT, z*Chunk.TILE_WIDTH);

            } break;
            case MaterialManager.MaterialCategory.Crystal: {
                crystalModel.AddToMesh(verts, uvs, indices, baseColor, 
                x*Chunk.TILE_WIDTH, y*Chunk.TILE_HEIGHT, z*Chunk.TILE_WIDTH);
            } break;

            default:{
                Debug.Log($"Unknown wall mat: {entry.Category} {material.Name}");
                CreateWallShape(world,tile,chunkPosition,x,y,z,verts,uvs,indices,baseColor);
            } break;
        }
    }

    public static void CreateWallShape(World world, Tile t, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> inds, Vector2 color, bool keep_faces = false){
        int tw = Chunk.TILE_WIDTH;
        int th = Chunk.TILE_HEIGHT;

        int tx = x * tw;
        int ty = y * th;
        int tz = z * tw;

        int px = chunkPosition.x*Chunk.WIDTH + x;
        int py = chunkPosition.y + y;
        int pz = chunkPosition.z*Chunk.WIDTH + z;   

        int vcount = verts.Count;


        if (world.GetVisibilityAmt(px,py+1,pz) == World.Visibility.Empty || keep_faces){
            verts.Add(new Vector3(tx,    ty+th, tz));
            verts.Add(new Vector3(tx+tw, ty+th, tz));
            verts.Add(new Vector3(tx,    ty+th, tz+tw));
            verts.Add(new Vector3(tx+tw, ty+th, tz+tw));
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);

            inds.Add(vcount+2);
            inds.Add(vcount+1);
            inds.Add(vcount);
            inds.Add(vcount+1);
            inds.Add(vcount+2);
            inds.Add(vcount+3);
            vcount+=4;
        }
        var visible = world.GetVisibilityAmt(px + 1 , py, pz);

        if (visible != World.Visibility.Hidden || keep_faces){
            var lowest = visible == World.Visibility.Empty ? 0.0f : 1.0f;

            verts.Add(new Vector3(tx+tw, ty+lowest, tz));
            verts.Add(new Vector3(tx+tw, ty+th,     tz));
            verts.Add(new Vector3(tx+tw, ty+lowest, tz+tw));
            verts.Add(new Vector3(tx+tw, ty+th,     tz+tw));
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);

            inds.Add(vcount+1);
            inds.Add(vcount+2);
            inds.Add(vcount);
            inds.Add(vcount+2);
            inds.Add(vcount+1);
            inds.Add(vcount+3);
            vcount+=4;
        }

        visible = world.GetVisibilityAmt(px- 1 , py, pz);

        if (visible != World.Visibility.Hidden || keep_faces){
            var lowest = visible == World.Visibility.Empty ? 0.0f : 1.0f;

            verts.Add(new Vector3(tx, ty+lowest, tz));
            verts.Add(new Vector3(tx, ty+th,     tz));
            verts.Add(new Vector3(tx, ty+lowest, tz+tw));
            verts.Add(new Vector3(tx, ty+th,     tz+tw));
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);

            inds.Add(vcount+2);
            inds.Add(vcount+1);
            inds.Add(vcount);
            inds.Add(vcount+1);
            inds.Add(vcount+2);
            inds.Add(vcount+3);
            vcount+=4;
        }

        visible = world.GetVisibilityAmt(chunkPosition.x*Chunk.WIDTH + x, chunkPosition.y + y, chunkPosition.z*Chunk.WIDTH + z + 1);

        if (visible != World.Visibility.Hidden || keep_faces){
            var lowest = visible == World.Visibility.Empty ? 0.0f : 1.0f;
            verts.Add(new Vector3(tx,    ty+lowest, tz+tw));
            verts.Add(new Vector3(tx,    ty+th,     tz+tw));
            verts.Add(new Vector3(tx+tw, ty+lowest, tz+tw));
            verts.Add(new Vector3(tx+tw, ty+th,     tz+tw));
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);

            inds.Add(vcount+2);
            inds.Add(vcount+1);
            inds.Add(vcount);
            inds.Add(vcount+1);
            inds.Add(vcount+2);
            inds.Add(vcount+3);
            vcount+=4;
        }

        visible = world.GetVisibilityAmt(px, py, pz - 1);
        if (visible != World.Visibility.Hidden || keep_faces){
            var lowest = visible == World.Visibility.Empty ? 0.0f : 1.0f;
            verts.Add(new Vector3(tx,    ty+lowest, tz));
            verts.Add(new Vector3(tx,    ty+th,     tz));
            verts.Add(new Vector3(tx+tw, ty+lowest, tz));
            verts.Add(new Vector3(tx+tw, ty+th,     tz));
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);

            inds.Add(vcount+1);
            inds.Add(vcount+2);
            inds.Add(vcount);
            inds.Add(vcount+2);
            inds.Add(vcount+1);
            inds.Add(vcount+3);
            vcount+=4;
        }
        
        visible = world.GetVisibilityAmt(px, py - 1 , pz);

        if (visible != World.Visibility.Hidden || keep_faces){
            verts.Add(new Vector3(tx,    ty,  tz));
            verts.Add(new Vector3(tx+tw, ty, tz));
            verts.Add(new Vector3(tx,    ty, tz+tw));
            verts.Add(new Vector3(tx+tw, ty, tz+tw));
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);

            inds.Add(vcount+1);
            inds.Add(vcount+2);
            inds.Add(vcount);
            inds.Add(vcount+2);
            inds.Add(vcount+1);
            inds.Add(vcount+3);
            vcount+=4;
        }
    }
}
