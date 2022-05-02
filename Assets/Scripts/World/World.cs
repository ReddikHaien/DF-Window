using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DwarfFortress.TileInfo;
using UnityEngine;
using UnityEngine.EventSystems;

public class World : MonoBehaviour, WorldEvent
{
    public enum Visibility{
        ///Solid tile
        Hidden,
        ///Walls Are Visible, but there is a floro
        Wall,
        ///Nothing there
        Empty,
    }
    public Material chunkMaterial;
    public GameObject ChunkPrefab;
    // Start is called before the first frame update

    private Tile[,,] tiles;
    private Vector3Int size;
    void Start()
    {
        var manager = RemoteManager.Instance;

        var info = manager.GetMapInfo();

        size = new Vector3Int(
            info.BlockSizeX,
            info.BlockSizeZ,
            info.BlockSizeY
        );

        Debug.Log($"Embark size is {size}");

        tiles = new Tile[size.x*16,size.y,size.z*16];

        var texture = manager.TileDataManager.textureManager.CreateTexture();

        chunkMaterial.SetTexture("_MainTex",texture);

        StartCoroutine(nameof(ChunkCreators));
    }

    public int ChunkCount => size.x * (size.y / 16) * size.z;

    IEnumerator ChunkCreators(){
        for (int y =  size.y/16-1; y >= 0; y--){
            for (int x = 0; x < size.x; x++){
                for (int z = 0; z < size.z; z++){
                    ExecuteEvents.Execute<WorldEvent>(this.gameObject,null,(w,d) => w.CreateChunk(
                        new Vector3(
                            x*Chunk.WIDTH*Chunk.TILE_WIDTH,
                            y*Chunk.WIDTH*Chunk.TILE_HEIGHT,
                            z*Chunk.WIDTH*Chunk.TILE_WIDTH)
                        )
                    );
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }

    public Tile GetTile(int x, int y, int z){
        if (x < 0 || x >= size.x*16 || y < 0 || y >= size.y || z < 0 || z >= size.z*16){
            return null;
        }
        return tiles[x,y,z];
    }

    public bool IsSideVisible(Vector3Int position, AbstractShape.Direction direction){
        var tile = GetTile(position.x,position.y,position.z);

        if (tile == null){
            return true;
        }
        else{
            var remote = RemoteManager.Instance;
            var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);

            if (matdef == null) return true;

            var material = remote.TileDataManager.materialManager.GetMaterial2(matdef);
            
            var shape = remote.GetTiletype(tile.TileType).Shape;

            var (shapeManager,_,_) = material.GetModelEntry(shape);

            // if (shapeManager is EmptyShape && shape == RemoteFortressReader.TiletypeShape.Wall){
            //     Debug.Log($"Undefined Material {matdef.Name}, Doesn't Have a Shape for {shape}");
            // }

            return shapeManager?.SideVisible(this,position,direction) ?? true;
        }
    }

    public bool isTileVisible(Vector3Int p){
        return 
            IsSideVisible(p + Vector3Int.up,AbstractShape.Direction.Down) ||
            IsSideVisible(p + Vector3Int.down,AbstractShape.Direction.Up) ||
            IsSideVisible(p + Vector3Int.forward,AbstractShape.Direction.Back) ||
            IsSideVisible(p + Vector3Int.back,AbstractShape.Direction.Front) || 
            IsSideVisible(p + Vector3Int.left,AbstractShape.Direction.Right) || 
            IsSideVisible(p + Vector3Int.right,AbstractShape.Direction.Left);

    }

    public Visibility GetVisibilityAmt(int x, int y, int z){
        var tile = GetTile(x,y,z);
        if (tile == null){
            return Visibility.Empty;
        }
        else{
            var remote = RemoteManager.Instance;

            var material = remote.GetMaterialDefinition(tile.Material);
            if (material == null){
                return Visibility.Empty;
            }

            var entry = remote.TileDataManager.materialManager.GetMaterial(material);

            var tiletype = remote.GetTiletype(tile.TileType);
            return tiletype.Shape switch{
                RemoteFortressReader.TiletypeShape.Floor => Visibility.Wall,
                RemoteFortressReader.TiletypeShape.Boulder => Visibility.Wall,
                RemoteFortressReader.TiletypeShape.Wall => entry.Category switch{
                    MaterialManager.MaterialCategory.Crystal => Visibility.Empty,
                    _ => Visibility.Hidden
                },
                _ => Visibility.Empty
            };
        }
    }


    public void LoadSegment(Vector3Int position){
        var reader = RemoteManager.Instance.FortressReader;
        var list = reader.GetBlockList(new RemoteFortressReader.BlockRequest{
            BlocksNeeded = 4096,
            MinX = position.x,
            MinY = position.z,
            MinZ = position.y,
            MaxX = position.x + 1,
            MaxY = position.z + 1,
            MaxZ = position.y + 16,
        });

        var result = new Tile[16,list.MapBlocks.Count,16];

        foreach(var mapBlock in list.MapBlocks){
            var mapX = mapBlock.MapX;
            var mapY = mapBlock.MapY;
            var mapZ = mapBlock.MapZ;
            
            if (mapBlock.Tiles.Count == 0){
                continue;
            }

            for(int x = 0; x < 16; x++){
                for (int y = 0; y < 16; y++){
                    var tileX = mapX+x;
                    var tileY = mapY+y;
                    var tileZ = mapZ;

                    var index = (x + y*16);

                    var original = tiles[tileX,tileZ,tileY] ?? new Tile();
                    original.TileType = mapBlock.Tiles[index];
                    original.Material = new MatPair(mapBlock.Materials[index].MatType,mapBlock.Materials[index].MatIndex);
                    original.BaseMaterial = new MatPair(mapBlock.BaseMaterials[index].MatType,mapBlock.BaseMaterials[index].MatIndex);
                    original.LayerMaterial = new MatPair(mapBlock.LayerMaterials[index].MatType,mapBlock.LayerMaterials[index].MatIndex);
                    original.VeinMaterial = new MatPair(mapBlock.VeinMaterials[index].MatType,mapBlock.VeinMaterials[index].MatIndex);
                    original.ConstructionMaterial = new MatPair(mapBlock.ConstructionItems[index].MatType,mapBlock.ConstructionItems[index].MatIndex);
                    original.WaterLevel = mapBlock.Water[index];
                    original.MagmaLevel = mapBlock.Magma[index];
                    original.Hidden = mapBlock.Hidden[index];

                    tiles[tileX,tileZ,tileY] = original;
                }
            }
        }
    }
    
    public void LoadData(Vector3Int position, Chunk dist){
        LoadSegment(position);

        if (dist != null)
            dist.GenerateMesh();
    }

    public void CreateChunk(Vector3 position)
    {
        var c = GameObject.Instantiate(ChunkPrefab,
                position,
                Quaternion.identity
            );
        c.name = $"Chunk({(int)position.x},{(int)position.y},{(int)position.z})";
    }
}
