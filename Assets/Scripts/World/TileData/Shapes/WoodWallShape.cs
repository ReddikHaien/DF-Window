using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using RemoteFortressReader;
using UnityEngine;

public class WoodWallShape : AbstractShape
{
    public override bool SideVisible(World world, Vector3Int position, Direction direction)
    {
        var remote = RemoteManager.Instance;
        var p = Vector3Int.zero;
        if (direction == Direction.Up){
            p = position + Vector3Int.up;
        }
        else if (direction == Direction.Down){
            p = position + Vector3Int.down;
        }
        else{
            //All Sides are visible
            return true;
        }
        var tile = world.GetTile(p.x,p.y,p.z);
        var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
        var material = remote.TileDataManager.materialManager.GetMaterial2(matdef);

        //We know that the faces are indentical in size, and therefore invisible
        return material.Category != MaterialManager.MaterialCategory.Wood;
        
        
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
