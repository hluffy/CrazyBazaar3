using UnityEngine;

public class Chunk
{
    public Vector2Int coord;   // Chunk 坐标
    public GameObject go;
    public MeshRenderer renderer;
    public ComputeBuffer buffer;

    private int[,] map;        // 全局地图
    private int chunkSize;

    public Chunk(Vector2Int coord, int chunkSize, Material mat, int[,] map)
    {
        this.coord = coord;
        this.chunkSize = chunkSize;
        this.map = map;

        go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.transform.position = new Vector3(coord.x * chunkSize, 0, coord.y * chunkSize);
        go.transform.localScale = new Vector3(chunkSize, chunkSize, 1);

        renderer = go.GetComponent<MeshRenderer>();
        renderer.material = new Material(mat);

        UploadTileData();
    }

    void UploadTileData()
    {
        int[,] tiles = ExtractTiles();
        int[] flat = new int[chunkSize * chunkSize];

        for (int y = 0; y < chunkSize; y++)
        for (int x = 0; x < chunkSize; x++)
        {
            flat[y * chunkSize + x] = tiles[y, x];
        }

        buffer = new ComputeBuffer(flat.Length, sizeof(int));
        buffer.SetData(flat);
        renderer.material.SetBuffer("_TileData", buffer);
        renderer.material.SetInt("_ChunkSize", chunkSize);
    }

    int[,] ExtractTiles()
    {
        int[,] tiles = new int[chunkSize, chunkSize];
        int startX = coord.x * chunkSize;
        int startY = coord.y * chunkSize;

        for (int y = 0; y < chunkSize; y++)
        for (int x = 0; x < chunkSize; x++)
        {
            int gx = startX + x;
            int gy = startY + y;
            if (gx >= 0 && gx < map.GetLength(1) && gy >= 0 && gy < map.GetLength(0))
                tiles[y, x] = map[gy, gx];
            else
                tiles[y, x] = 0;
        }
        return tiles;
    }

    public void Destroy()
    {
        if (buffer != null) buffer.Release();
        GameObject.Destroy(go);
    }
}
