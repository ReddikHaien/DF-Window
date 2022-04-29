using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RemoteFortressReader;
public class FortressReader
{   
    private readonly DfClient client;

    private readonly short getMaterialListId;
    private readonly short getWorldMapId;
    private readonly short getMapInfoId;
    private readonly short getGrowthListId;
    private readonly short getBlockListId;
    private readonly short checkHashesId;
    private readonly short resetMapHashesId;
    private readonly short getTileTypeListId;
    public FortressReader(DfClient client){
        this.client = client;
        getMaterialListId = client.BindMethod("GetMaterialList","dfproto.EmptyMessage","RemoteFortressReader.MaterialList","RemoteFortressReader");
        getWorldMapId = client.BindMethod("GetWorldMap","dfproto.EmptyMessage","RemoteFortressReader.WorldMap","RemoteFortressReader");
        getGrowthListId = client.BindMethod("GetGrowthList","dfproto.EmptyMessage","RemoteFortressReader.MaterialList","RemoteFortressReader");
        getBlockListId = client.BindMethod("GetBlockList", "RemoteFortressReader.BlockRequest","RemoteFortressReader.BlockList","RemoteFortressReader");
        checkHashesId = client.BindMethod("CheckHashes","dfproto.EmptyMessage","dfproto.EmptyMessage","RemoteFortressReader");
        resetMapHashesId = client.BindMethod("ResetMapHashes","dfproto.EmptyMessage","dfproto.EmptyMessage","RemoteFortressReader");
        getMapInfoId = client.BindMethod("GetMapInfo","dfproto.EmptyMessage","RemoteFortressReader.MapInfo","RemoteFortressReader");
        getTileTypeListId = client.BindMethod("GetTiletypeList","dfproto.EmptyMessage","RemoteFortressReader.TiletypeList","RemoteFortressReader");
    }

    public MaterialList GetMaterialList() => client.SendMessage<MaterialList>(getMaterialListId, MaterialList.Parser);
    public WorldMap GetWorldMap() => client.SendMessage<WorldMap>(getWorldMapId,WorldMap.Parser);
    public MapInfo GetMapInfo() => client.SendMessage<MapInfo>(getMapInfoId,MapInfo.Parser);
    public MaterialList GetGrowthList() => client.SendMessage<MaterialList>(getGrowthListId,MaterialList.Parser);
    public BlockList GetBlockList(BlockRequest request) => client.SendMessage<BlockRequest,BlockList>(getBlockListId,request,BlockList.Parser);
    public TiletypeList GetTiletypeList() => client.SendMessage<TiletypeList>(getTileTypeListId,TiletypeList.Parser);
    public void CheckHashes(){
        client.SendMessage<Dfproto.EmptyMessage>(checkHashesId,Dfproto.EmptyMessage.Parser);
    }
    public void ResetMapHashes(){
        client.SendMessage<Dfproto.EmptyMessage>(resetMapHashesId,Dfproto.EmptyMessage.Parser);
    }
}
