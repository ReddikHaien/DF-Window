using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface WorldEvent : IEventSystemHandler
{
    void LoadData(Vector3Int position, Chunk dist);

    void CreateChunk(Vector3 position);
}
