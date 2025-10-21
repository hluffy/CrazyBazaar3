using UnityEngine;
using UnityEditor;
using System.IO;

public class ControlMapGenerator : EditorWindow
{
    int texSize = 256;  // 图片大小
    string savePath = "Assets/ControlMap.png";

    [MenuItem("Tools/Generate Test Control Map")]
    public static void ShowWindow()
    {
        GetWindow<ControlMapGenerator>("Control Map Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Generate a RGBA Control Map", EditorStyles.boldLabel);

        texSize = EditorGUILayout.IntField("Texture Size", texSize);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
    }

    void Generate()
    {
        Texture2D tex = new Texture2D(texSize, texSize, TextureFormat.RGBA32, false);

        for (int y = 0; y < texSize; y++)
        {
            for (int x = 0; x < texSize; x++)
            {
                // 分成四个区域，每个区域一个通道
                Color c = Color.black;

                if (x < texSize / 2 && y < texSize / 2)
                    c = new Color(1, 0, 0, 0); // R 区域
                else if (x >= texSize / 2 && y < texSize / 2)
                    c = new Color(0, 1, 0, 0); // G 区域
                else if (x < texSize / 2 && y >= texSize / 2)
                    c = new Color(0, 0, 1, 0); // B 区域
                else
                    c = new Color(0, 0, 0, 1); // A 区域

                tex.SetPixel(x, y, c);
            }
        }

        tex.Apply();

        // 保存 PNG
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);
        AssetDatabase.Refresh();

        Debug.Log("Control map generated at: " + savePath);
    }
}
