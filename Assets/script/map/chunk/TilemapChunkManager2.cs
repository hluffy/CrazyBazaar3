using UnityEngine;
using System.Collections.Generic;

public class TilemapChunkManager2 : MonoBehaviour
{
    public Material tileMaterial;
    public int chunkSize = 128; // 每个 chunk 的 tile 数量 (128x128)
    public int mapWidth = 1024; // 整个地图大小
    public int mapHeight = 1024;

    private Dictionary<Vector2Int, ComputeBuffer> activeChunks = new Dictionary<Vector2Int, ComputeBuffer>();
    private Vector2Int currentChunk;

    void Start()
    {
        currentChunk = GetChunkPos(Vector2Int.zero);
        LoadChunksAround(currentChunk);
    }

    void Update()
    {
        Vector2Int playerTile = GetPlayerTile();
        Vector2Int playerChunk = GetChunkPos(playerTile);

        if (playerChunk != currentChunk)
        {
            currentChunk = playerChunk;
            LoadChunksAround(currentChunk);
        }
    }

    Vector2Int GetChunkPos(Vector2Int tilePos)
    {
        return new Vector2Int(tilePos.x / chunkSize, tilePos.y / chunkSize);
    }

    Vector2Int GetPlayerTile()
    {
        Vector3 pos = GameObject.FindWithTag("Player").transform.position;
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }

    void LoadChunksAround(Vector2Int center)
    {
        HashSet<Vector2Int> needed = new HashSet<Vector2Int>();
        for (int dx = -1; dx <= 1; dx++)
        for (int dy = -1; dy <= 1; dy++)
        {
            needed.Add(center + new Vector2Int(dx, dy));
        }

        // 卸载不需要的
        var oldChunks = new List<Vector2Int>(activeChunks.Keys);
        foreach (var c in oldChunks)
        {
            if (!needed.Contains(c))
            {
                activeChunks[c].Release();
                activeChunks.Remove(c);
            }
        }

        // 加载需要的
        foreach (var c in needed)
        {
            if (!activeChunks.ContainsKey(c))
            {
                int[] tileIds = LoadChunkData(c);
                var buffer = new ComputeBuffer(tileIds.Length, sizeof(int));
                buffer.SetData(tileIds);
                activeChunks[c] = buffer;
            }
        }

        // 绑定到材质
        int idx = 0;
        foreach (var kv in activeChunks)
        {
            tileMaterial.SetBuffer("_TileIdBuffer" + idx, kv.Value);
            tileMaterial.SetVector("_ChunkOrigin" + idx, new Vector4(kv.Key.x * chunkSize, kv.Key.y * chunkSize, 0, 0));
            idx++;
        }
    }

    int[] LoadChunkData(Vector2Int chunkCoord)
    {
        int[] data = new int[chunkSize * chunkSize];
        for (int i = 0; i < data.Length; i++)
            data[i] = Random.Range(1, 5); // 示例：随机 tile id
        return data;
    }
}
