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

    public void CreateMeshData(List<Vector3> verts, List<Vector2> uvs, List<int> inds){
        var remote = RemoteManager.Instance;
        for (int x = 0; x < Chunk.WIDTH; x++){
            for (int y = 0; y < Chunk.WIDTH; y++){
                for (int z = 0; z < Chunk.WIDTH; z++){
                    
                    var tile = world.GetTile(chunkPosition.x*WIDTH + x, chunkPosition.y + y, chunkPosition.z*WIDTH + z);

                    if (tile == null){
                        continue;
                    }
                    if (!tile.Hidden){

                        remote.TileDataManager.CreateMeshForTile(world,tile,chunkPosition,x,y,z,verts,uvs,inds);
                    }
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
        var verts = new List<Vector3>();
        var uvs = new List<Vector2>();
        var inds = new List<int>();
        var task = Task.Factory.StartNew(() => CreateMeshData(verts,uvs,inds));

        while(!task.IsCompleted){
            yield return new WaitForSeconds(0.1f);
        }

        if (task.IsFaulted){
            var exception = task.Exception;
            var log = $"Model Creation Produced Exceptions: {exception.InnerExceptions.Select(x => x.ToString()).Aggregate((a,b) => $"{a}\n{b}")}";
            Debug.LogError(log);
        }

        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = inds.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        completedChunks++;
        
        Debug.Log($"completed {completedChunks}/{world.ChunkCount}");
        
        if (completedChunks >= world.ChunkCount){
            var b = new StringBuilder();
            foreach(var entry in missingEls){
                b.Append(entry.Key);
                b.Append('\n');
            }
            Debug.Log(b.ToString());
        }
    }
}
