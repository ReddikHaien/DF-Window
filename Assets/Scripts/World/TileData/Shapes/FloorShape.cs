using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using RemoteFortressReader;
using UnityEngine;

public class FloorShape : AbstractShape
{
    private readonly Model grassModel;
    private readonly Model BambooModel;
    private readonly (Model, Model) berryBushModel;
    private readonly (Model, Model) bubbleBulbModel;
    private readonly (Model, Model, Model) flowerGrassModel;
    public FloorShape(){
        var grassObjs = ModelLoader.LoadModel("Models/Grass");
        grassModel = grassObjs["Leaves"];
        
        var bubbleBulbObjs = ModelLoader.LoadModel("Models/Bubble Bulb");
        bubbleBulbModel = (bubbleBulbObjs["Stem"], bubbleBulbObjs["Bulb"]);

        var flowerObj = ModelLoader.LoadModel("Models/FlowerGrass");
        flowerGrassModel = (
            flowerObj["Leaves"],
            flowerObj["FlowerLeaves"],
            flowerObj["FlowerCores"]
        );
        
        var berryBushObj = ModelLoader.LoadModel("Models/BerryBush");
        berryBushModel = (
            berryBushObj["Bush"],
            berryBushObj["Berries"]
        );

        var bambooObj = ModelLoader.LoadModel("Models/Bamboo");
        BambooModel = bambooObj["default"];
    }

    public override void CreateMesh(
        World world,
        Tiletype type,
        Vector3Int chunkPosition,
        int x, int y, int z,
        List<Vector3> verts, 
        List<Vector2> uvs,
        List<int> inds,
        MaterialManager materialManager)
    {
    }

    public static void CreateFloorShape(World world,
        Tiletype type,
        Vector3Int chunkPosition,
        int x, int y, int z,
        List<Vector3> verts, 
        List<Vector2> uvs,
        List<int> inds,
        Vector2 color)
        {
        int tx = x * Chunk.TILE_WIDTH;
        int ty = y * Chunk.TILE_HEIGHT;
        int tz = z * Chunk.TILE_WIDTH;

        int vcount = verts.Count;
        verts.Add(new Vector3(tx,ty+1,tz));
        verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz));
        verts.Add(new Vector3(tx,ty+1,tz+Chunk.TILE_WIDTH));
        verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz+Chunk.TILE_WIDTH));
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

        if (world.GetVisibilityAmt(chunkPosition.x*Chunk.WIDTH + x + 1 , chunkPosition.y + y, chunkPosition.z*Chunk.WIDTH + z) == World.Visibility.Empty){
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty,tz));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty,tz+Chunk.TILE_WIDTH));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz+Chunk.TILE_WIDTH));
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

        if (world.GetVisibilityAmt(chunkPosition.x*Chunk.WIDTH + x - 1 , chunkPosition.y + y, chunkPosition.z*Chunk.WIDTH + z) == World.Visibility.Empty){
            verts.Add(new Vector3(tx,ty,tz));
            verts.Add(new Vector3(tx,ty+1,tz));
            verts.Add(new Vector3(tx,ty,tz+Chunk.TILE_WIDTH));
            verts.Add(new Vector3(tx,ty+1,tz+Chunk.TILE_WIDTH));
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

        if (world.GetVisibilityAmt(chunkPosition.x*Chunk.WIDTH + x, chunkPosition.y + y, chunkPosition.z*Chunk.WIDTH + z + 1) == World.Visibility.Empty){
            verts.Add(new Vector3(tx,                 ty,tz+Chunk.TILE_WIDTH));
            verts.Add(new Vector3(tx,                 ty+1,tz+Chunk.TILE_WIDTH));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty,tz+Chunk.TILE_WIDTH));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz+Chunk.TILE_WIDTH));
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
        if (world.GetVisibilityAmt(chunkPosition.x*Chunk.WIDTH + x, chunkPosition.y + y, chunkPosition.z*Chunk.WIDTH + z - 1) == World.Visibility.Empty){
            verts.Add(new Vector3(tx,                 ty,tz));
            verts.Add(new Vector3(tx,                 ty+1,tz));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty,tz));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz));
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
        
        if (world.GetVisibilityAmt(chunkPosition.x*Chunk.WIDTH + x, chunkPosition.y + y -1 , chunkPosition.z*Chunk.WIDTH + z) == World.Visibility.Wall){
            verts.Add(new Vector3(tx,ty,tz));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty,tz));
            verts.Add(new Vector3(tx,ty,tz+Chunk.TILE_WIDTH));
            verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty,tz+Chunk.TILE_WIDTH));
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

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        var remote = RemoteManager.Instance;
        var material = remote.GetMaterialDefinition(tile.Material);
        var entry = materialManager.GetMaterial(material);
        switch(entry.Category){
            case MaterialManager.MaterialCategory.Stone: break;
            case MaterialManager.MaterialCategory.Wood: break;
            case MaterialManager.MaterialCategory.Crystal: break;
            case MaterialManager.MaterialCategory.Grass: {
                switch(entry.SubCategory){
                    case 1:{
                        bubbleBulbModel.Item1.AddToMesh(verts, uvs, indices, entry.SubColor1,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                    
                        bubbleBulbModel.Item2.AddToMesh(verts, uvs, indices, entry.SubColor2,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                    
                    } break;
                    case 2:{
                        flowerGrassModel.Item1.AddToMesh(verts, uvs, indices, entry.SubColor1,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                        flowerGrassModel.Item2.AddToMesh(verts, uvs, indices, entry.SubColor2,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                        flowerGrassModel.Item3.AddToMesh(verts, uvs, indices, entry.SubColor3,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                    } break;
                    case 3:{
                        berryBushModel.Item1.AddToMesh(verts, uvs, indices, entry.SubColor1,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                    
                        berryBushModel.Item2.AddToMesh(verts, uvs, indices, entry.SubColor2,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                    
                    } break;
                    case 4:{
                        BambooModel.AddToMesh(verts, uvs, indices, entry.SubColor1,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);    
                    } break;
                    default: {
                        grassModel.AddToMesh(verts, uvs, indices, entry.SubColor1,
                        x * Chunk.TILE_WIDTH, y * Chunk.TILE_HEIGHT, z * Chunk.TILE_WIDTH);
                    } break;
                }
            } break;
            default: 
                Debug.Log($"Unknown floor mat: {entry.Category} {material.Name}");
            break;
        }
        
        FloorShape.CreateFloorShape(world,remote.GetTiletype(tile.TileType),chunkPosition,x,y,z,verts,uvs,indices,entry.BaseColor);
    }
}
