using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using RemoteFortressReader;
using UnityEngine;

public class BoulderShape : AbstractShape
{
    private readonly Vector2 boulderColor;

    public BoulderShape(TextureManager textureManager){
        this.boulderColor = textureManager.AddColor(new Color32(128,128,128,255));
    }
    public override void CreateMesh(World world, Tiletype type, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        var color = materialManager.GetBaseColor(type);
        FloorShape.CreateFloorShape(world,type,chunkPosition,x,y,z,verts,uvs,indices,color);

        int tx = x * Chunk.TILE_WIDTH;
        int ty = y * Chunk.TILE_HEIGHT;
        int tz = z * Chunk.TILE_WIDTH;

        int vcount = verts.Count;
        verts.Add(new Vector3(tx+0.2f,ty+1,tz+0.2f)); // 0
        verts.Add(new Vector3(tx+1.5f,ty+1,tz+2.8f)); // 1
        verts.Add(new Vector3(tx+2.8f,ty+1.0f,tz+0.2f)); // 2

        verts.Add(new Vector3(tx+0.2f,ty+2.3f,tz+0.2f)); // 3
        verts.Add(new Vector3(tx+1.5f,ty+2.3f,tz+2.8f)); // 4
        verts.Add(new Vector3(tx+2.8f,ty+2.3f,tz+0.2f)); // 5

        verts.Add(new Vector3(tx+1.5f,ty+2.8f,tz+1.5f));

        uvs.Add(boulderColor);
        uvs.Add(boulderColor);
        uvs.Add(boulderColor);
        uvs.Add(boulderColor);  
        uvs.Add(boulderColor);
        uvs.Add(boulderColor);
        uvs.Add(boulderColor);

        indices.Add(vcount+4);
        indices.Add(vcount);
        indices.Add(vcount+1);
        indices.Add(vcount+3);
        indices.Add(vcount);
        indices.Add(vcount+4);

        indices.Add(vcount+1);
        indices.Add(vcount+2);
        indices.Add(vcount+5);
        indices.Add(vcount+5);
        indices.Add(vcount+4);
        indices.Add(vcount+1);

        indices.Add(vcount);
        indices.Add(vcount+5);
        indices.Add(vcount+2);
        indices.Add(vcount+3);
        indices.Add(vcount+5);
        indices.Add(vcount);

        indices.Add(vcount+3);
        indices.Add(vcount+4);
        indices.Add(vcount+6);

        indices.Add(vcount+4);
        indices.Add(vcount+5);
        indices.Add(vcount+6);

        indices.Add(vcount+5);
        indices.Add(vcount+3);
        indices.Add(vcount+6);

        vcount+=1;
    }

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        
    }
}
