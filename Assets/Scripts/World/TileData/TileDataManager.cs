using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using UnityEngine;
using TileDataBuilder;
using System.Linq;
using ModelImplementation;

public class TileDataManager
{
    public readonly TextureManager textureManager;
    public readonly MaterialManager materialManager;



    public TileDataManager(){
        var builder = new Builder(
            MaterialManager.MaterialCategory.Stone,
            new Color32(255,0,255,255),
            new ResourceTexture("DefaultTexture"),

            new (string,AbstractShape)[]{
                ("Empty", new EmptyShape()),
                ("Wall", new WallShape()),
                ("WoodWall", new WoodWallShape()),
            }.AsEnumerable(),

            new (string,AbstractModel)[]{
                ("Wall", new WallModel()),
                ("Empty", new EmptyModel()),
                ("Tourmaline",new TourmalineModel()),
                ("WoodWall", new WoodWallModel()),
            }
        );

        var definitionLoader = new DefinitionLoader();

        definitionLoader.LoadBaseDefinitions(builder);

        
        var (t, m) = builder.BuildManagers();


        this.textureManager = t;
        this.materialManager = m;
    }

    public void CreateMeshForTile(
        World world,
        Tile tile, 
        Vector3Int chunkPos,
        int x, int y, int z,
        List<Vector3> verts,
        List<Vector2> uvs,
        List<int> indices){
        
        var remote = RemoteManager.Instance;


        var tiletype = remote.GetTiletype(tile.TileType);
        var shape = tiletype.Shape;

        if (shape == RemoteFortressReader.TiletypeShape.Empty){
            return;
        }

        var matdef = remote.GetMaterialDefinition(tile.BaseMaterial);
        if  (matdef != null){
            var material = materialManager.GetMaterial2(matdef);
            if (materialManager.isDefaultMaterial(material) && shape == RemoteFortressReader.TiletypeShape.Wall){
                Debug.Log($"{matdef.Name} doesn't have shape {shape}");
            }
            var (_, model, coloring) = material.GetModelEntry(shape);
            model.AddMesh(remote,world,chunkPos, new Vector3Int(x,y,z),tile, verts, uvs, indices, coloring, materialManager);
        }
    }
}
