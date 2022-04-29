using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using UnityEngine;

public abstract class AbstractShape
{
    public enum Direction{
        Up,
        Down,
        Left,
        Right,
        Front,
        Back
    }
    public virtual bool SideVisible(World world, Vector3Int position, Direction direction){
        return true;
    }

    public abstract void CreateMesh(
        World world,
        RemoteFortressReader.Tiletype type, 
        Vector3Int chunkPosition, 
        int x, int y, int z,
        List<Vector3> verts,
        List<Vector2> uvs,
        List<int> indices,
        MaterialManager materialManager);
    
    public abstract void CreateTileMesh(
        World world,
        Tile tile, 
        Vector3Int chunkPosition, 
        int x, int y, int z,
        List<Vector3> verts,
        List<Vector2> uvs,
        List<int> indices,
        MaterialManager materialManager
    );
}
