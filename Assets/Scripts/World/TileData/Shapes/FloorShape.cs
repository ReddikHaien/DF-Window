using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelLoading;
using RemoteFortressReader;
using UnityEngine;

using System;

public class FloorShape : AbstractShape
{
    public override bool SideVisible(World world, Vector3Int position, Direction direction)
    {
        if (direction == Direction.Down) return false;
        if (direction == Direction.Up) return true;
        
        Vector3Int p = position + (direction switch{
            Direction.Left => Vector3Int.left,
            Direction.Right => Vector3Int.right,
            Direction.Front => Vector3Int.forward,
            Direction.Back => Vector3Int.back,
            _ => throw new InvalidOperationException("should not be up or down"),
        });

        var tile = world.GetTile(p.x,p.y,p.z);
        var remote = RemoteManager.Instance;
        var shape = remote.GetTiletype(tile.TileType).Shape;
        return shape != RemoteFortressReader.TiletypeShape.Floor;
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
