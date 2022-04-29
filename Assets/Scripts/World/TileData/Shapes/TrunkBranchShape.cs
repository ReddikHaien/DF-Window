using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using RemoteFortressReader;
using UnityEngine;

public class TrunkBranchShape : AbstractShape
{
    public override void CreateMesh(World world, Tiletype type, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        var baseColor = materialManager.DEFAULT;
        int tx = x * Chunk.TILE_WIDTH;
        int ty = y * Chunk.TILE_HEIGHT;
        int tz = z * Chunk.TILE_WIDTH;

        int vcount = verts.Count;
        
        string direction = type.Direction.ToString(System.Text.Encoding.ASCII);
        switch(direction){
            // case "----W---": {
            //     verts.Add(new Vector3(tx,ty+1,tz));
            //     verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz));
            //     verts.Add(new Vector3(tx,ty+1,tz+Chunk.TILE_WIDTH));
            //     verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz+Chunk.TILE_WIDTH));
            //     uvs.Add(baseColor);
            //     uvs.Add(baseColor);
            //     uvs.Add(baseColor);
            //     uvs.Add(baseColor);

            //     indices.Add(vcount+2);
            //     indices.Add(vcount+1);
            //     indices.Add(vcount);
            //     indices.Add(vcount+1);
            //     indices.Add(vcount+2);
            //     indices.Add(vcount+3);
            //     vcount+=4;
            // } break;
            default: Debug.Log(direction); break;
        }

        verts.Add(new Vector3(tx,ty+1,tz));
                verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz));
                verts.Add(new Vector3(tx,ty+1,tz+Chunk.TILE_WIDTH));
                verts.Add(new Vector3(tx+Chunk.TILE_WIDTH,ty+1,tz+Chunk.TILE_WIDTH));
                uvs.Add(baseColor);
                uvs.Add(baseColor);
                uvs.Add(baseColor);
                uvs.Add(baseColor);

                indices.Add(vcount+2);
                indices.Add(vcount+1);
                indices.Add(vcount);
                indices.Add(vcount+1);
                indices.Add(vcount+2);
                indices.Add(vcount+3);
                vcount+=4;
    }

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {

    }
}
