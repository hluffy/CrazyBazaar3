using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

public class TilemapIDMaskWindow : EditorWindow
{
    private Tilemap tilemap;
    private int chunkSize = 128;
    private string outputFolder = MapProperty.idMaskFolder;

    [MenuItem("Tools/生成 Tilemap ID Mask")]
    public static void ShowWindow()
    {
        GetWindow<TilemapIDMaskWindow>("Tilemap ID Mask Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tilemap ID Mask 生成器", EditorStyles.boldLabel);

        tilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", tilemap, typeof(Tilemap), true);
        chunkSize = EditorGUILayout.IntField("Chunk Size", chunkSize);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (GUILayout.Button("生成 ID Mask"))
        {
            if (tilemap == null)
            {
                EditorUtility.DisplayDialog("错误", "请先指定一个 Tilemap！", "确定");
                return;
            }

            GenerateIDMasks();
        }
    }

    private Color GetTileID(TileBase typedTile)
    {
        if (typedTile == null) return new Color32(0, 0, 0, 255);
        TypedTile tile = typedTile as TypedTile;
        
          if (tile == null) return new Color32(0, 0, 0, 255);
        // string tileName = tile.name.ToLower();

        // if (tileName.Contains("grass")) return 1;
        // if (tileName.Contains("stone")) return 2;
        // if (tileName.Contains("water")) return 3;
        // byte a =(byte)( ((int)tile.tileType) );
        // Debug.Log("byte:"+a);
        // return new Color32(a, 0, 0, 255);
        switch (tile.tileType)
        {
            case TypedTile.TileType.None:
                return new Color32(255, 255, 0, 255);
            case TypedTile.TileType.Grass:
                return new Color32(0, 255, 0, 255);
            case TypedTile.TileType.Stone:
                return new Color32(170, 0, 0, 255);
            case TypedTile.TileType.Sea:
                return new Color32(0, 255, 255, 255);
            case TypedTile.TileType.Maple:
                return new Color32(255, 0, 255, 255);
            default:
                return new Color32(255, 255, 255, 255);


        }
       
    }

    private void GenerateIDMasks()
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        BoundsInt bounds = tilemap.cellBounds;
        int mapWidth = bounds.size.x;
        int mapHeight = bounds.size.y;

        int chunkCountX = Mathf.CeilToInt((float)mapWidth / chunkSize);
        int chunkCountY = Mathf.CeilToInt((float)mapHeight / chunkSize);

 

        for (int cx = 0; cx < chunkCountX; cx++)
        {
            for (int cy = 0; cy < chunkCountY; cy++)
            {
                int startX = bounds.xMin + cx * chunkSize;
                int startY = bounds.yMin + cy * chunkSize;

                int currentChunkWidth = Mathf.Min(chunkSize, bounds.xMax - startX);
                int currentChunkHeight = Mathf.Min(chunkSize, bounds.yMax - startY);

                Texture2D tex = new Texture2D(chunkSize, chunkSize, TextureFormat.RGBA32, false);

                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        Vector3Int pos = new Vector3Int(startX + x, startY + y, 0);
                        TileBase tile = tilemap.GetTile(pos);
                        Color32 idColor = GetTileID(tile);
                        tex.SetPixel(x, y, idColor);
                    }
                }

                tex.Apply();

                byte[] pngData = tex.EncodeToPNG();
                string fileName = $"IDMask_{cx}_{cy}.png";
                File.WriteAllBytes(Path.Combine(outputFolder, fileName), pngData);

                Debug.Log($"生成 Chunk [{cx},{cy}] ID mask: {fileName}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", "所有 ID Mask 生成完成！", "确定");
    }
}
