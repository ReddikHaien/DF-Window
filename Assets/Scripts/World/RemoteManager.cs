using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using UnityEngine;

public class RemoteManager
{
    public static RemoteManager Instance{
        get => instance;
    }

    private static readonly RemoteManager instance = new RemoteManager();    
    static RemoteManager(){}



    private readonly IList<RemoteFortressReader.Tiletype> tiletypes;

    private readonly DfClient client;
    private readonly FortressReader fortressReader;

    private readonly TileDataManager tileDataManager;

    private Dictionary<MatPair, MaterialDefinition> materialDefinitions;

    private readonly MaterialDefinition defaultDefinition = new MaterialDefinition{
        Color = new DfColor(255,255,255),
        ArmorLayer = RemoteFortressReader.ArmorLayer.LayerCover,
        DownStep = 0,
        Id = "empty",
        InstrumentDef = null,
        MatPair = new MatPair(0,-1),
        Name = "empty"
    };

    private RemoteManager(){
        
        tileDataManager = new TileDataManager();

        client = new DfClient("127.0.0.1",5000);
        fortressReader = new FortressReader(client);

        fortressReader.ResetMapHashes();
        var list = fortressReader.GetMaterialList();

        materialDefinitions = new Dictionary<MatPair, MaterialDefinition>();

        foreach (var entry in list.MaterialList_){
            var matpair = new MatPair(entry.MatPair.MatType,entry.MatPair.MatIndex);
            var id = entry.Id.ToString(System.Text.Encoding.ASCII);
            var name = entry.Name.ToString(System.Text.Encoding.ASCII);
            var color = entry.StateColor != null ? new DfColor((byte)entry.StateColor.Red,(byte)entry.StateColor.Blue,(byte)entry.StateColor.Green) : new DfColor(0,0,0);
            var instrument = entry.Instrument;
            var upStep = entry.UpStep;
            var downStep = entry.DownStep;
            var armorLayer = entry.Layer;

            materialDefinitions.Add(
                matpair,
                new MaterialDefinition{
                ArmorLayer = armorLayer,
                Color = color,
                DownStep = downStep,
                Id = id,
                InstrumentDef = instrument,
                MatPair = matpair,
                Name = name,
                UpStep = upStep,
            });

        }

        tiletypes = fortressReader.GetTiletypeList().TiletypeList_;


    }

    public MaterialDefinition GetMaterialDefinition(MatPair matpair){
        return materialDefinitions.TryGetValue(matpair,out var m) ? m : defaultDefinition;
    }

    public RemoteFortressReader.Tiletype GetTiletype(int id){
        return tiletypes[id];
    }

    public FortressReader FortressReader => fortressReader;
    public TileDataManager TileDataManager => tileDataManager;
    public RemoteFortressReader.MapInfo GetMapInfo(){
        return fortressReader.GetMapInfo();
    }
}
