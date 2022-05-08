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
        BOULDER = new BoulderShape();
        WALL = new WallShape();
        RAMP = new RampShape();
        TRUNK_BRANCH = new TrunkBranchShape();
        BUSH = new BushShape();
    }
}
