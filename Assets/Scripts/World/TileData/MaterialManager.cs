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

    public enum Visibility{
        None,
        Wall,
        Pit,
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

    public MaterialManager(TextureManager textureManager){
        DEFAULT = textureManager.AddColor(new Color32(255,0,255,255));
        SOIL = textureManager.AddColor(new Color32(128,64,0,255));
        STONE = textureManager.AddColor(new Color32(64,64,64,255));
        GRASS_DARK = textureManager.AddColor(new Color32(64,128,64,255));
        GRASS_LIGHT = textureManager.AddColor(new Color32(64,188,64,255));
        GRASS_DRY = textureManager.AddColor(new Color32(128,128,64,255));
        GRASS_DEAD = textureManager.AddColor(new Color32(213,188,43,255));
        TREE = textureManager.AddColor(new Color32(142,88,0,255));
        WATER = textureManager.AddColor(new Color32(0,189,242,255));
        var t = textureManager;

        entries = new MaterialDictBuilder(textureManager)

        //Stones
        .Add("galena",MaterialCategory.Stone,new Color32(0,0,0,255))
        .Add("slate",MaterialCategory.Stone,new Color32(0,173,173,255))
        .Add("phyllite", MaterialCategory.Stone, new Color32(33,8,6,255))
        .Add("marble", MaterialCategory.Stone, new Color32(216,216,216,255))
        .Add("granite", MaterialCategory.Stone,new Color32(255,163,128,255))
        .Add("orthoclase", MaterialCategory.Stone, new Color32(255,252,119,255))
        .Add("pyrolusite", MaterialCategory.Stone, new Color32(145,145,145,255))
        .Add("white sand", MaterialCategory.Stone, new Color32(253,255,216,255))
        .Add("native gold", MaterialCategory.Stone, new Color32(255,255,0,255))
        .Add("microcline", MaterialCategory.Stone, new Color32(0,255,255,255))
        .Add("gabbro", MaterialCategory.Stone, new Color32(104,104,104,255))
        .Add("siltstone", MaterialCategory.Stone, new Color32(183,151,134,255))
        .Add("bauxite", MaterialCategory.Stone, new Color32(142,138,128,255))
        .Add("schist", MaterialCategory.Stone, new Color32(142,138,128,255))
        .Add("quartzite", MaterialCategory.Stone, new Color32(61,61,61,255))
        .Add("diorite", MaterialCategory.Stone, new Color32(255,255,255,255))
        .Add("clay loam", MaterialCategory.Stone, new Color32(165,151,132,255))
        .Add("tiger iron", MaterialCategory.Stone, new Color32(255,229,0,255))
        .Add("tetrahedrite", MaterialCategory.Stone, new Color32(93,61,61,255))
        .Add("cobaltite", MaterialCategory.Stone, new Color32(0,29,255,255))
        .Add("mica", MaterialCategory.Stone, new Color32(226,226,226,255))
        .Add("sphalerite", MaterialCategory.Stone, new Color32(35,0,61,255))

        //Gems
        .Add("pyrite", MaterialCategory.Crystal, new Color32(0,81,0,255))
        .Add("rubicelle", MaterialCategory.Crystal, new Color32(219,219,0,255))
        .Add("clear garnet", MaterialCategory.Crystal, new Color32(255,255,255,255))
        .Add("wax opal", MaterialCategory.Crystal, new Color32(165,110,0,255))
        .Add("claro opal", MaterialCategory.Crystal, new Color32(170,170,255,255))
        .Add("white opal", MaterialCategory.Crystal, new Color32(255,255,255,255))
        .Add("shell opal", MaterialCategory.Crystal, new Color32(188,188,188,255))
        .Add("almandine", MaterialCategory.Crystal, new Color32(122,0,20,255))
        .Add("prase", MaterialCategory.Crystal, new Color(0,81,0,255))
        .Add("amethyst", MaterialCategory.Crystal, new Color32(219,0,219,255))
        .Add("black pyrope", MaterialCategory.Crystal, new Color32(51,51,51,255))
        .Add("rose quartz", MaterialCategory.Crystal, new Color32(255,159,81,255))
        .Add("milk quartz", MaterialCategory.Crystal, new Color32(239,239,239,255))
        .Add("tanzanite", MaterialCategory.Crystal, new Color32(228,159,185, 255))
        .Add("clear diamond", MaterialCategory.Crystal, new Color32(255,255,255,255))
        .Add("red tourmaline", MaterialCategory.Crystal, new Color32(187,0,165,255))
        .Add("pink tourmaline", MaterialCategory.Crystal, new Color32(240,116,170,255))
        .Add("clear tourmaline", MaterialCategory.Crystal, new Color32(221,221,221,255))
        .Add("clear zircon", MaterialCategory.Crystal, new Color32(255,255,255,255))
        .Add("brown zircon", MaterialCategory.Crystal, new Color32(165,77,0,255))
        .Add("green zircon", MaterialCategory.Crystal, new Color32(0,137,0,255))
        .Add("yellow zircon", MaterialCategory.Crystal, new Color32(137,137,0,255))
        .Add("black zircon", MaterialCategory.Crystal, new Color32(0,0,0,255))
        .Add("red zircon", MaterialCategory.Crystal, new Color32(137,0,0,255))
        .Add("violet spessartine", MaterialCategory.Crystal, new Color32(132,0,165,255))
        .Add("bandfire opal", MaterialCategory.Crystal, new Color32(122,226,225,255))
        .Add("heliodor", MaterialCategory.Crystal, new Color32(24,167,0,255))
        .Add("rock crystal", MaterialCategory.Crystal, new Color32(191,191,191,255))

        //Wood
        .Add("date palm plant", MaterialCategory.Wood, new Color32(150,132,106,255), subColor1: new Color32(150,102,34,255))
        .Add("round lime tree plant", MaterialCategory.Wood, new Color32(147,143,137,255), subColor1: new Color32(247,202,138,255))
        .Add("coffee tree plant", MaterialCategory.Wood, new Color32(173,130,71,255), subColor1: new Color32(219,143,37,255))
        .Add("kumquat tree plant", MaterialCategory.Wood, new Color32(0,143,32,255), subColor1: new Color32(247,202,138,255))
        .Add("willow plant", MaterialCategory.Wood, new Color32(97,124,99,255), subColor1: new Color32(124,158,127,255))
        .Add("custard-apple tree plant", MaterialCategory.Wood, new Color32(165,83,28,255), subColor1: new Color32(193,113,60,255))
        .Add("acacia plant", MaterialCategory.Wood, new Color32(147,143,137,255), subColor1: new Color32(193,113,60,255))
        .Add("pomegranate tree plant", MaterialCategory.Wood, new Color32(158,109,88,255), subColor1: new Color32(247,202,138,255))
        .Add("tunnel tube plant", MaterialCategory.Wood, new Color32(122,0,122,255), subColor1: new Color32(187,0,187,255))
        .Add("spore tree plant", MaterialCategory.Wood, new Color32(0,128,128,255), subColor1: new Color32(0,76,76,255))
        .Add("goblin-cap plant", MaterialCategory.Wood, new Color32(255,0,0,255), subColor1: new Color32(255,163,163,255))
        .Add("fungiwood plant", MaterialCategory.Wood, new Color32(255,244,79, 255), subColor1: new Color32(198,196,157,255))
        .Add("tower-cap plant", MaterialCategory.Wood, new Color32(255,255,255,255), new Color32(200,200,200,255))
        .Add("black-cap plant", MaterialCategory.Wood, new Color32(32,32,32,255), subColor1: new Color32(0,0,0,255))
        
        //Grass
        .Add("hair grass plant", MaterialCategory.Grass, new Color32(170,186,124, 255), subColor1: new Color32(124,186,124,255))
        .Add("meadow-grass plant", MaterialCategory.Grass, new Color32(170,186,124, 255), subColor1: new Color32(124,186,124,255))
        .Add("fescue grass plant", MaterialCategory.Grass, new Color32(170,186,124, 255), subColor1: new Color32(124,186,124,255))
        .Add("bubble bulb plant", MaterialCategory.Grass, new Color32(170,186,124,255), subColor1: new Color32(0,198,140, 255), subColor2: new Color32(0,255,255,255), subCategory: 1)
        .Add("white mountain heather plant", MaterialCategory.Grass, new Color32(170,186,124, 255), subColor1: new Color32(124,186,124,255))
        .Add("downy grass feather", MaterialCategory.Grass, new Color32(170,186,124, 255), subColor1: new Color32(255,255,255,255))
        .Add("mountain avens plant", MaterialCategory.Grass, new Color32(170,186,124,255), subColor1: new Color32(124,186,124,255), subColor2: new Color32(255,255,255,255), subColor3: new Color32(255,255,0,255), subCategory: 2)
        .Add("cloudberry plant", MaterialCategory.Grass, new Color32(170,186,124,255), subColor1: new Color32(0,119,15,255), subColor2: new Color32(255,147,0,255), subCategory: 3)
        .Add("dog's tooth grass plant", MaterialCategory.Grass, new Color32(170,186,124, 255), subColor1: new Color32(124,186,124,255))
        .Add("carpetgrass plant", MaterialCategory.Grass, new Color32(170,186,124, 255), subColor1: new Color32(124,186,124,255))
        .Add("golden bamboo plant", MaterialCategory.Grass, new Color32(170,186,124,255), subColor1: new Color32(204,163,0,255),subCategory: 4)
        .Add("hedge bamboo plant", MaterialCategory.Grass, new Color32(170,186,124,255), subColor1: new Color32(15,239,0,255),subCategory: 4)
        //cottongrass plant
        .Complete();

        DEFAULT_ENTRY = new MaterialEntry(textureManager,MaterialCategory.Unknown,DEFAULT_COLOR);
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