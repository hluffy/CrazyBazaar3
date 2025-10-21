using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Tilemaps;

// 动态加载

// 每帧检查玩家所在 Chunk

// 加载中心及周围 3×3 Chunk

// 卸载远 Chunk

// 玩家离开区域时，自动销毁 CPU 贴图

// Shader 不再采样远 Chunk，释放显存

// Shader 绑定

// _IDMask0~_IDMask8 对应最多 9 张 Chunk

// _ChunkOrigin0~_ChunkOrigin8 保存每张 Chunk 的世界原点

// Shader 根据 worldPos - chunkOrigin → UV 采样 ID mask

// FilterMode.Point

// 保证每个像素对应一个 tile，不被插值模糊

public class TilemapChunkManager : MonoBehaviour
{
    [Header("设置")]
    public Transform player;             // 玩家对象
    public Tilemap tilemap;
    private int chunkSize = MapProperty.chunkSize;         // Chunk 大小（tile 数）
    private string idMaskFolder =  MapProperty.idMaskFolder;

    [Header("Shader 设置")]
    public Material tileMaterial;        // Shader 使用的材质

    private Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();
    private Vector2Int lastCenterChunk = new Vector2Int(int.MinValue, int.MinValue);

    private class Chunk
    {
        public Vector2Int index;
        public Texture2D cpuTexture;   // CPU 贴图
        public Texture gpuTexture;     // 上传 GPU 的 Texture
    }

    void Update()
    {

         // 1. 卸载旧的
        // if (currentChunk != null)
        // {
        //     foreach (var kv in currentChunk.tiles)
        //     {
        //         tilemap.SetTile(kv.Key, null); // 清理旧的 Tile
        //     }
        // }
        
        Vector2Int centerChunk = GetChunkIndex(player.position);
       
        if (centerChunk != lastCenterChunk)
        {
             Debug.Log("当前玩家所在的chunk:"+centerChunk.x+",--:"+centerChunk.y);
            lastCenterChunk = centerChunk;
            UpdateChunks(centerChunk);
        }
    }

    // 获取玩家所在 Chunk 索引
    Vector2Int GetChunkIndex(Vector3 worldPos)
    {
//         Vector2 mapOrigin = new Vector2(tilemap.cellBounds.xMin, tilemap.cellBounds.yMin);
//         Vector2Int center = new Vector2Int(
//     Mathf.FloorToInt((player.position.x - mapOrigin.x) / chunkSize),
//     Mathf.FloorToInt((player.position.y - mapOrigin.y) / chunkSize)
// );

//         return center;
        int cx = Mathf.FloorToInt(worldPos.x / chunkSize);
        int cy = Mathf.FloorToInt(worldPos.y / chunkSize);
        return new Vector2Int(cx, cy);
    }

    // 更新 3x3 Chunk
    void UpdateChunks(Vector2Int center)
    {
        HashSet<Vector2Int> needed = new HashSet<Vector2Int>();

        // 加载周围 3x3  获取玩家周边
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                Vector2Int idx = center + new Vector2Int(dx, dy);
                needed.Add(idx);

                if (!loadedChunks.ContainsKey(idx))
                    LoadChunk(idx);
            }

        // 卸载不需要的 Chunk
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kv in loadedChunks)
        {
            if (!needed.Contains(kv.Key))
            {
                UnloadChunk(kv.Key);
                toRemove.Add(kv.Key);
            }
        }
        foreach (var idx in toRemove)
            loadedChunks.Remove(idx);

        // 更新 Shader
        UpdateShaderChunks();
    }

    // 加载单个 Chunk
    void LoadChunk(Vector2Int idx)
    {
        string path = $"{idMaskFolder}IDMask_{idx.x}_{idx.y}.png";
        if (!File.Exists(path))
        {
            Debug.LogWarning($"ID Mask 文件不存在: {path}");
            return;
        }
        Debug.LogWarning($"ID Mask 文件存在: {path}");
        byte[] data = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(chunkSize, chunkSize, TextureFormat.R8, false);
        tex.LoadImage(data);
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        Chunk chunk = new Chunk
        {
            index = idx,
            cpuTexture = tex,
            gpuTexture = tex
        };
        loadedChunks.Add(idx, chunk);
    }

    // 卸载 Chunk
    void UnloadChunk(Vector2Int idx)
    {
        if (loadedChunks.TryGetValue(idx, out Chunk chunk))
        {
            Destroy(chunk.cpuTexture);
            // GPU texture 会随材质引用释放
        }
    }

    // 更新 Shader
    void UpdateShaderChunks()
    {
        // Shader 根据 worldPos - chunkOrigin → UV 采样 ID mask
        int i = 0;
        foreach (var kv in loadedChunks)
        {
             //_IDMask0~_IDMask8 对应最多 9 张 Chunk
            tileMaterial.SetTexture($"_IDMask{i}", kv.Value.gpuTexture);
            //_ChunkOrigin0~_ChunkOrigin8 保存每张 Chunk 的世界原点
            tileMaterial.SetVector($"_ChunkOrigin{i}", new Vector4(kv.Key.x * chunkSize, kv.Key.y * chunkSize, 0, 0));
            i++;
        }
        tileMaterial.SetInt("_ChunkCount", loadedChunks.Count);
    }
}
