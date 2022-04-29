using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using UnityEngine;

public class ShapeManager
{
    public readonly AbstractShape EMPTY;
    public readonly AbstractShape FLOOR;
    public readonly AbstractShape BOULDER;
    public readonly AbstractShape WALL;
    public readonly AbstractShape RAMP;
    public readonly AbstractShape TRUNK_BRANCH;

    public readonly AbstractShape BUSH;

    public ShapeManager(TextureManager textureManager){
        FLOOR = new FloorShape();
        EMPTY = new EmptyShape();
        BOULDER = new BoulderShape(textureManager);
        WALL = new WallShape();
        RAMP = new RampShape();
        TRUNK_BRANCH = new TrunkBranchShape();
        BUSH = new BushShape();
    }

    public void GetTileShape(Tile tile){

    }

    public AbstractShape GetShape(RemoteFortressReader.Tiletype type){
        return type.Shape switch {
            RemoteFortressReader.TiletypeShape.Floor => FLOOR,
            RemoteFortressReader.TiletypeShape.Boulder => BOULDER,
            RemoteFortressReader.TiletypeShape.Wall => WALL,
            //RemoteFortressReader.TiletypeShape.TreeShape => WALL,
            RemoteFortressReader.TiletypeShape.Ramp => RAMP,
            RemoteFortressReader.TiletypeShape.TrunkBranch => TRUNK_BRANCH,
            RemoteFortressReader.TiletypeShape.Sapling => BUSH,
            _ => EMPTY
        };
    }
}
