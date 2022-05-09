using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using ModelImplementation;
using UnityEngine;

public class MaterialManager
{
    internal static readonly Color32 DEFAULT_COLOR = new Color32(255,0,255,255);

    public enum MaterialCategory{
        Crystal,
        Stone,
        Wood,
        Grass,
        Unknown,
    }

    public readonly Vector2 DEFAULT;
    public readonly Vector2 SOIL;
    public readonly Vector2 STONE;
    public readonly Vector2 GRASS_DARK;
    public readonly Vector2 GRASS_LIGHT;
    public readonly Vector2 GRASS_DRY;
    public readonly Vector2 GRASS_DEAD;
    public readonly Vector2 TREE;
    public readonly Vector2 WATER;

    private readonly Dictionary<string,MaterialEntry> entries;

    private readonly Dictionary<string,MaterialEntry2> entries2;

    private readonly MaterialEntry DEFAULT_ENTRY;

    private readonly MaterialEntry2 defaultEntry;
    private readonly (AbstractShape,AbstractModel,Dictionary<string,Vector4>) defaultModelEntry;
    public MaterialManager(
        Dictionary<string,MaterialEntry2> entries,
        MaterialEntry2 defaultMaterial,
        (AbstractShape,AbstractModel,Dictionary<string,Vector4>) defaultModelEntry
        ){
        this.entries2 = entries;
        this.defaultModelEntry = defaultModelEntry;
        this.defaultEntry = defaultMaterial;
        MaterialEntry2.defaultModel = defaultModelEntry;
    }
    
    public Vector2 GetBaseColor(RemoteFortressReader.Tiletype type){
        return type.Material switch{
            RemoteFortressReader.TiletypeMaterial.Air => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Ashes => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Brook => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Campfire => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Construction => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Driftwood => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Feature => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Fire => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.FrozenLiquid => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.GrassDark => GRASS_DARK,
            RemoteFortressReader.TiletypeMaterial.GrassDead => GRASS_LIGHT,
            RemoteFortressReader.TiletypeMaterial.GrassDry => GRASS_DRY,
            RemoteFortressReader.TiletypeMaterial.GrassLight => GRASS_DEAD,
            RemoteFortressReader.TiletypeMaterial.Hfs => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.LavaStone => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Magma => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Mineral => STONE,
            RemoteFortressReader.TiletypeMaterial.Mushroom => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.NoMaterial => DEFAULT,
            RemoteFortressReader.TiletypeMaterial.Plant => GRASS_DARK,
            RemoteFortressReader.TiletypeMaterial.Pool => WATER,
            RemoteFortressReader.TiletypeMaterial.Root => TREE,
            RemoteFortressReader.TiletypeMaterial.River => WATER,
            RemoteFortressReader.TiletypeMaterial.Soil => SOIL,
            RemoteFortressReader.TiletypeMaterial.Stone => STONE,
            RemoteFortressReader.TiletypeMaterial.TreeMaterial => TREE,
            RemoteFortressReader.TiletypeMaterial.UnderworldGate => DEFAULT,
            _ => DEFAULT
        };
    }

    public bool isDefaultMaterial(MaterialEntry2 entry){
        return entry == defaultEntry;
    }

    public MaterialEntry2 GetMaterial2(MaterialDefinition definition) {
        
        if (entries2.TryGetValue(definition.Name,out var x)){
            return x;
        }
        else{
            return defaultEntry;
        }
    }

    public MaterialEntry GetMaterial(MaterialDefinition definition) => entries.TryGetValue(definition.Name, out var x) ? x : DEFAULT_ENTRY;
}

//TODO rename
public class MaterialEntry2{

    internal static (AbstractShape,AbstractModel,Dictionary<string,Vector4>) defaultModel;

    private readonly Dictionary<RemoteFortressReader.TiletypeShape,(AbstractShape,AbstractModel,Dictionary<string,Vector4>)> models;

    private readonly Vector4 defaultTexture;
    
    private readonly MaterialManager.MaterialCategory category;

    private readonly Color color;

    public MaterialEntry2(
        Vector4 defaultTexture,
        Dictionary<RemoteFortressReader.TiletypeShape,(AbstractShape,AbstractModel,Dictionary<string,Vector4>)> models,
        MaterialManager.MaterialCategory category   
    ){
        this.defaultTexture = defaultTexture;
        this.models = models;
        this.category = category;
    }

    public Vector4 DefaultTexture => defaultTexture;

    public (AbstractShape,AbstractModel,Dictionary<string,Vector4>) GetModelEntry(RemoteFortressReader.TiletypeShape shape) => models.TryGetValue(shape, out var x) ? x : defaultModel;

    public MaterialManager.MaterialCategory Category => category;
}

public class MaterialEntry{
    public MaterialManager.MaterialCategory Category { get; }
    public Vector2 BaseColor { get; }
    public Vector2 SubColor1 { get; }
    public Vector2 SubColor2 { get; }
    public Vector2 SubColor3 { get; }
    public int SubCategory { get; }

    public MaterialEntry(
        TextureManager textureManager,
        MaterialManager.MaterialCategory category, 
        Color32 baseColor, 
        Color32? subColor1 = null,
        Color32? subColor2 = null,
        Color32? subColor3 = null,
        int subCategory = 0){
        Category = category;
        BaseColor = textureManager.AddColor(baseColor);
        SubColor1 = textureManager.AddColor(subColor1 ?? MaterialManager.DEFAULT_COLOR);
        SubColor2 = textureManager.AddColor(subColor2 ?? MaterialManager.DEFAULT_COLOR);
        SubColor3 = textureManager.AddColor(subColor3 ?? MaterialManager.DEFAULT_COLOR);
        SubCategory = subCategory;
    }
}

internal class MaterialDictBuilder{
    private readonly Dictionary<string, MaterialEntry> dict;
    private readonly TextureManager textureManager;
    private bool completed;
    public MaterialDictBuilder(TextureManager textureManager){
        this.dict = new Dictionary<string, MaterialEntry>();
        this.textureManager = textureManager;
    }

    public MaterialDictBuilder Add(
        string name,
        MaterialManager.MaterialCategory category,
        Color32 baseColor,
        Color32? subColor1 = null,
        Color32? subColor2 = null,
        Color32? subColor3 = null,
        int subCategory = 0){
        if (completed){
            throw new System.AccessViolationException("Builder Completed, Can't Add More Data");
        }
        dict.Add(name, new MaterialEntry(
            textureManager,
            category,
            baseColor,
            subColor1,
            subColor2,
            subColor3,
            subCategory));
        return this;
    }
    public Dictionary<string,MaterialEntry> Complete(){
        completed = true;
        return dict;
    }
}