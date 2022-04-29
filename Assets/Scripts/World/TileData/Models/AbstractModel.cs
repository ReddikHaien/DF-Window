using System.Collections;
using System.Collections.Generic;
using DwarfFortress.TileInfo;
using UnityEngine;


namespace ModelImplementation{
    public abstract class AbstractModel
    {

        public abstract string Name {get;}

        public abstract void AddMesh(
            RemoteManager remote,
            World world,
            Vector3Int chunkPosition,
            Vector3Int tilePosition,
            Tile tile,
            List<Vector3> verts,
            List<Vector2> uvs,
            List<int> indices,
            Dictionary<string,Vector4> baseColors,
            MaterialManager materialManager
        );
    }
}
