using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DwarfFortress.TileInfo;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chunk : MonoBehaviour, ChunkEvent
{
    public const int WIDTH = 16;
    public const int TILE_WIDTH = 3;
    public const int TILE_HEIGHT = 6;
    private Vector3Int chunkPosition;
    private World world;

    private Mesh mesh;
    // Start is called before the first frame update

    void Start()
    {
        Vector3 pos = transform.position;

        chunkPosition = new Vector3Int(
            Mathf.FloorToInt(pos.x / (WIDTH * TILE_WIDTH)),
            Mathf.FloorToInt(pos.y / TILE_HEIGHT),
            Mathf.FloorToInt(pos.z / (WIDTH * TILE_WIDTH))
        );

        world = FindObjectOfType<World>();
        
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;

        if (world != null){ 
            ExecuteEvents.Execute<WorldEvent>(world.gameObject,null,(x,y) => x.LoadData(chunkPosition,this));
        }
        else{
            Debug.Log("Failed to retrieve world object");
        }
    }

    public void CreateMeshData(ChunkMeshBuilder builder){
        var remote = RemoteManager.Instance;
        for (int x = 0; x < Chunk.WIDTH; x++){
            for (int y = 0; y < Chunk.WIDTH; y++){
                for (int z = 0; z < Chunk.WIDTH; z++){
                    
                    if (!world.isTileVisible(new Vector3Int(chunkPosition.x*WIDTH + x, chunkPosition.y + y, chunkPosition.z*WIDTH + z))){
                        continue;
                    }

                    var tile = world.GetTile(chunkPosition.x*WIDTH + x, chunkPosition.y + y, chunkPosition.z*WIDTH + z);

                    if (tile == null){
                        continue;
                    }

                    remote.TileDataManager.CreateMeshForTile(world,tile,chunkPosition,x,y,z,builder);
                }
            }
        }
    }

    public void GenerateMesh()
    {
        StartCoroutine(nameof(WaiterRoutine));
    }


    public static ConcurrentDictionary<string,byte> missingEls = new ConcurrentDictionary<string, byte>();
    private static int completedChunks = 0;

    IEnumerator WaiterRoutine(){
        var builder = new ChunkMeshBuilder();
        var task = Task.Factory.StartNew(() => CreateMeshData(builder));

        while(!task.IsCompleted){
            yield return new WaitForSeconds(0.1f);
        }

        if (task.IsFaulted){
            var exception = task.Exception;
            var log = $"Model Creation Produced Exceptions: {exception.InnerExceptions.Select(x => x.ToString()).Aggregate((a,b) => $"{a}\n{b}")}";
            Debug.LogError(log);
        }

        builder.BuildMesh(mesh);

        completedChunks++;
        
        Debug.Log($"completed {completedChunks}/{world.ChunkCount}");
        
        if (completedChunks >= world.ChunkCount){
            var b = new StringBuilder();

            var entries = missingEls.AsEnumerable().ToList();

            foreach(var entry in missingEls.AsEnumerable().OrderBy(x => x.Key)){
                b.Append(entry.Key);
                b.Append('\n');
            }
            Debug.Log(b.ToString());
        }
    }
}
