using UnityEngine;
using UnityEditor;

public class NoiseGenerator : EditorWindow
{
    int width = 512;
    int height = 512;
    float scale = 20f;
    int seed = 0;

    [MenuItem("Tools/Generate Noise Texture")]
    static void Init()
    {
        NoiseGenerator window = (NoiseGenerator)EditorWindow.GetWindow(typeof(NoiseGenerator));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Noise Texture Settings", EditorStyles.boldLabel);
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        scale = EditorGUILayout.FloatField("Scale", scale);
        seed = EditorGUILayout.IntField("Seed", seed);

        if (GUILayout.Button("Generate & Save"))
        {
            GenerateNoiseTexture();
        }
    }

    void GenerateNoiseTexture()
    {
        Texture2D tex = new Texture2D(width, height);
        System.Random prng = new System.Random(seed);
        float offsetX = prng.Next(-100000, 100000);
        float offsetY = prng.Next(-100000, 100000);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = offsetX + (float)x / width * scale;
                float yCoord = offsetY + (float)y / height * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                tex.SetPixel(x, y, new Color(sample, sample, sample));
            }
        }

        tex.Apply();

        // 保存成 PNG 文件
        byte[] bytes = tex.EncodeToPNG();
        string path = EditorUtility.SaveFilePanel("Save Noise Texture", "Assets", "NoiseTex.png", "png");
        if (path.Length != 0)
        {
            System.IO.File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
            Debug.Log("Noise Texture saved to " + path);
        }
    }
}
