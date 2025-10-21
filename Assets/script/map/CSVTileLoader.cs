using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.IO;

public class CSVTileLoader : MonoBehaviour
{
    public Tilemap tilemap; // 拖入场景中的Tilemap对象
    public Tile[] tileTypes; // 索引对应CSV中的数字

    void Start()
    {
        LoadCSVAndGenerateMap();
    }

    void LoadCSVAndGenerateMap()
    {
        // 从Resources加载CSV
        TextAsset csvData = Resources.Load<TextAsset>("Maps/50_50_tile");
        if (csvData == null)
        {
            Debug.LogError("CSV文件未找到！");
            return;
        }
       
        // 解析CSV数据
        string[] rows = csvData.text.Split('\n');
        int height = rows.Length;
        int width = rows[0].Split(',').Length;
        print(rows[0]);
        print(height);
        // 从下往上生成（Unity坐标系）
        // for (int y = 1; y < height; y++)
        for (int y = 1; y < height; y++)
        {
            string[] cols = rows[y].Split(',');

            string xPos = cols[0];
            string yPos = cols[1];
            string biomeType = cols[2];
            string tileType = cols[3];
            // print(xPos + " ," + yPos + " ," + biomeType + " ," + tileType);
            Vector3Int position = new Vector3Int(int.Parse(xPos), int.Parse(yPos), 0);

            switch (tileType)
            {

                case "water":
                    tilemap.SetTile(position, tileTypes[0]);
                    continue;
                case "stone":
                    tilemap.SetTile(position, tileTypes[1]);
                    continue;
                case "grass":
                    tilemap.SetTile(position, tileTypes[2]);
                    continue;
                default:
                    tilemap.SetTile(position, tileTypes[2]);
                    continue;

            }

        }
    }
}