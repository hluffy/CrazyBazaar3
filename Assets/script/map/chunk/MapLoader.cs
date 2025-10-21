using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class MapLoader
{
    public static int[,] LoadCSV(string path)
    {
        string[] lines = File.ReadAllLines(path);
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;

        int[,] map = new int[rows, cols];
        for (int y = 0; y < rows; y++)
        {
            string[] parts = lines[y].Split(',');
            for (int x = 0; x < cols; x++)
            {
                map[y, x] = int.Parse(parts[x]);
            }
        }
        return map;
    }
}
