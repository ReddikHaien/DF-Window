using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using RemoteFortressReader;
using UnityEngine;

public class FloorShape : AbstractShape
{
    public override bool SideVisible(World world, Vector3Int position, Direction direction)
    {
        return direction != Direction.Down;
    }
    
    public override void CreateMesh(World world, Tiletype type, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        throw new System.NotImplementedException();
    }

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        throw new System.NotImplementedException();
    }
}
