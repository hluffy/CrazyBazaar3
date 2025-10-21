using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HouseController : MonoBehaviour
{
    [Header("npc")]
    [SerializeField] private NpcController npc;

    [Header("Tile Check")]
    public Tilemap tilemap;
    public List<GameObject> tileChecks;
    private List<Vector3Int> tiles = new();
    public Dictionary<GameObject, List<Vector3Int>> tileDictionary;

    void Start()
    {
        GetTiles();
        if (npc != null)
        {
            npc.StartWork(tiles, tilemap);
        }
    }

    private void GetTiles()
    {
        tiles.Clear();
        if (tileChecks == null || tileChecks.Count == 0) return;
        foreach (var tileCheck in tileChecks)
        {
            Bounds bounds = tileCheck.GetComponent<BoxCollider2D>().bounds;

            Vector3Int min = tilemap.WorldToCell(bounds.min);
            Vector3Int max = tilemap.WorldToCell(bounds.max);

            int index = 0;
            for (int x = min.x; x < max.x; x+=2)
            {
                if (index % 2 == 0)
                {
                    for (int y = min.y; y < max.y; y+=2)
                    {
                        Vector3Int cellPos = new(x, y, 0);
                        TileBase tile = tilemap.GetTile(cellPos);
                        if (tile != null && tile is TypedTile)
                        {
                            tiles.Add(cellPos);
                        }
                    }
                }
                else
                {
                    for (int y = max.y - 1; y >= min.y; y-=2)
                    {
                        Vector3Int cellPos = new(x, y, 0);
                        TileBase tile = tilemap.GetTile(cellPos);
                        if (tile != null && tile is TypedTile)
                        {
                            tiles.Add(cellPos);
                        }
                    }
                }
                
                index++;
            }
        }
        
    }
}
