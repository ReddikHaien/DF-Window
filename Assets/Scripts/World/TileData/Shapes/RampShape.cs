using System.Collections;
using System.Collections.Generic;
using System.Text;
using DwarfFortress.TileInfo;
using RemoteFortressReader;
using UnityEngine;

public class RampShape : AbstractShape
{
    public override void CreateMesh(World world, Tiletype type, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {
        Vector2 color = materialManager.GetBaseColor(type);

        int tw = Chunk.TILE_WIDTH;
        int th = Chunk.TILE_HEIGHT;

        int tx = x * tw;
        int ty = y * th;
        int tz = z * tw;

        int px = chunkPosition.x*Chunk.WIDTH + x;
        int py = chunkPosition.y + y;
        int pz = chunkPosition.z*Chunk.WIDTH + z;   

        int vcount = verts.Count;

        var neighbours = GetNeighbours(px,py,pz,world);

        float pox = 3.0f;
        float nox = 0.0f;
        float poy = 3.0f;
        float noy = 0.0f;

        if (CanRampUp(neighbours[3])){
            nox = 3.0f;
        }
        if (CanRampUp(neighbours[4])){
            pox = 0.0f;
        }
        if (CanRampUp(neighbours[1])){
            noy = 3.0f;
        }
        if (CanRampUp(neighbours[6])){
            poy = 0.0f;
        }

        if (poy < 3.0f){
            verts.Add(new Vector3(tx,ty+th+1,tz + tw));
            verts.Add(new Vector3(tx+tw,ty+th+1,tz + tw));
            verts.Add(new Vector3(tx+nox,ty+1,tz + poy));
            verts.Add(new Vector3(tx+pox,ty+1,tz + poy));
            indices.AddRange(new int[]{vcount + 1,vcount + 2, vcount,vcount + 2,vcount + 1,vcount + 3});
            vcount += 4;
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color); 
        }

        if (noy > 0.0f){
            verts.Add(new Vector3(tx+nox,ty+1,tz+noy));
            verts.Add(new Vector3(tx+pox,ty+1,tz+noy));
            verts.Add(new Vector3(tx,ty+th+1,tz));
            verts.Add(new Vector3(tx+tw,ty+th+1,tz));
            indices.AddRange(new int[]{vcount + 1,vcount + 2, vcount,vcount + 2,vcount + 1,vcount + 3});
            vcount += 4;
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);            
        }

        if (pox < 3.0f){
            verts.Add(new Vector3(tx+tw,ty+th+1,tz));
            verts.Add(new Vector3(tx+tw,ty+th+1,tz+tw));
            verts.Add(new Vector3(tx+pox,ty+1,tz+noy));
            verts.Add(new Vector3(tx+pox,ty+1,tz+poy));
            indices.AddRange(new int[]{vcount + 2,vcount + 1, vcount,vcount + 1,vcount + 2,vcount + 3});
            vcount += 4;
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
        }

        if (nox > 0.0f){
            verts.Add(new Vector3(tx+nox,ty+1.0f,tz+noy));
            verts.Add(new Vector3(tx+nox,ty+1.0f,tz+poy));
            verts.Add(new Vector3(tx,ty+th+1,tz));
            verts.Add(new Vector3(tx,ty+th+1,tz+tw));
            indices.AddRange(new int[]{vcount + 2,vcount + 1, vcount,vcount + 1,vcount + 2,vcount + 3});
            vcount += 4;
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);
            uvs.Add(color);            
        }

        verts.Add(new Vector3(tx,ty+1,tz));
        verts.Add(new Vector3(tx+tw,ty+1,tz));
        verts.Add(new Vector3(tx,ty+1,tz+tw));
        verts.Add(new Vector3(tx+tw,ty+1,tz+tw));
        uvs.Add(color);
        uvs.Add(color);
        uvs.Add(color);
        uvs.Add(color);

        indices.Add(vcount+2);
        indices.Add(vcount+1);
        indices.Add(vcount);
        indices.Add(vcount+1);
        indices.Add(vcount+2);
        indices.Add(vcount+3);
        vcount+=4;

        //CORNERS
        if (CanRampUp(neighbours[0])){
            if (!CanRampUp(neighbours[1]) && !CanRampUp(neighbours[3])){
                verts.Add(new Vector3(tx+tw,ty+1,tz));
                verts.Add(new Vector3(tx,ty+1,tz+tw));
                verts.Add(new Vector3(tx,ty+th+1,tz));
                indices.AddRange(new int[]{vcount + 2,vcount + 1, vcount});
                vcount += 3;
                uvs.Add(color);
                uvs.Add(color);
                uvs.Add(color);
            }
        }
        if (CanRampUp(neighbours[2])){
            if (!CanRampUp(neighbours[1]) && !CanRampUp(neighbours[4])){
                verts.Add(new Vector3(tx,ty+1,tz));
                verts.Add(new Vector3(tx+tw,ty+1,tz+tw));
                verts.Add(new Vector3(tx+tw,ty+th+1,tz));
                indices.AddRange(new int[]{vcount + 1,vcount + 2, vcount});
                vcount += 3;
                uvs.Add(color);
                uvs.Add(color);
                uvs.Add(color);
            }

        }
        if (CanRampUp(neighbours[7])){
            if (!CanRampUp(neighbours[6]) && !CanRampUp(neighbours[4])){
                verts.Add(new Vector3(tx,ty+1,tz+tw));
                verts.Add(new Vector3(tx+tw,ty+1,tz));
                verts.Add(new Vector3(tx+tw,ty+th+1,tz+tw));
                indices.AddRange(new int[]{vcount + 2,vcount + 1, vcount});
                vcount += 3;
                uvs.Add(color);
                uvs.Add(color);
                uvs.Add(color);
            }
        }

        if (CanRampUp(neighbours[5])){
            if (!CanRampUp(neighbours[6]) && !CanRampUp(neighbours[3])){
                verts.Add(new Vector3(tx+tw,ty+1,tz+tw));
                verts.Add(new Vector3(tx,ty+1,tz));
                verts.Add(new Vector3(tx,ty+th+1,tz+tw));
                indices.AddRange(new int[]{vcount + 1,vcount + 2, vcount});
                vcount += 3;
                uvs.Add(color);
                uvs.Add(color);
                uvs.Add(color);
            }
        }
    }

    //0 1 2
    //3 # 4
    //5 6 7
    private RemoteFortressReader.TiletypeShape[] GetNeighbours(int x, int y, int z, World world){
        var info = RemoteManager.Instance;
        return new RemoteFortressReader.TiletypeShape[]{
            GetShape(x-1,y,z-1,world,info),
            GetShape(x  ,y,z-1,world,info),
            GetShape(x+1,y,z-1,world,info),
            GetShape(x-1,y,z  ,world,info),
            GetShape(x+1,y,z  ,world,info),
            GetShape(x-1,y,z+1,world,info),
            GetShape(x  ,y,z+1,world,info),
            GetShape(x+1,y,z+1,world,info),
        };
    }

    private RemoteFortressReader.TiletypeShape GetShape(int x, int y, int z, World world, RemoteManager manager){
        var tile = world.GetTile(x, y, z);
        return tile != null ? manager.GetTiletype(tile.TileType).Shape : RemoteFortressReader.TiletypeShape.Empty;
    }

    public bool CanRampUp(RemoteFortressReader.TiletypeShape shape) => shape switch{
        RemoteFortressReader.TiletypeShape.Wall => true,
        _ => false
    };

    public override void CreateTileMesh(World world, Tile tile, Vector3Int chunkPosition, int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> indices, MaterialManager materialManager)
    {

    }
}
