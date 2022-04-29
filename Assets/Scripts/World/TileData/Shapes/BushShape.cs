using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using RemoteFortressReader;
using UnityEngine;

public class BushShape : AbstractShape
{
    private readonly Model trunkModel;
    private readonly Model bushModel;
    
    public BushShape(){
        var models = ModelLoader.LoadModel("Models/Bush");
        trunkModel = models["Trunk"];
        bushModel = models["Bush"];
    }

    public override void CreateMesh(World world, Tiletype type, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {


        Vector2 color = materialManager.DEFAULT;
        
        Debug.Log($"Bush Material {type.Caption.ToString(System.Text.Encoding.ASCII)}");

        FloorShape.CreateFloorShape(world,type,chunkPosition,x,y,z,verts,uvs,indices,color);
        trunkModel.AddToMesh(verts,uvs,indices,color,x*Chunk.TILE_WIDTH,y*Chunk.TILE_HEIGHT+1,z*Chunk.TILE_WIDTH);
        bushModel.AddToMesh(verts,uvs,indices,color,x*Chunk.TILE_WIDTH,y*Chunk.TILE_HEIGHT+1,z*Chunk.TILE_WIDTH);
    }

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        
    }
}
