using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using RemoteFortressReader;
using UnityEngine;

public class BoulderShape : AbstractShape
{
    public override void CreateMesh(World world, Tiletype type, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {

    }

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
       
    }
}
