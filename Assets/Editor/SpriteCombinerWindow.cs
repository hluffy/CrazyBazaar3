// ===============================
// ✅ 编辑器工具（Sprite 合成器）
// ===============================
using UnityEngine;
using UnityEditor;

public class SpriteCombinerWindow : EditorWindow
{
    private Sprite[] inputSprites = new Sprite[4];
    private int outputSize = 256;

    private static GameObject persistentCameraGO;
    private static Camera persistentCam;
    private static RenderTexture persistentRT;

    [MenuItem("Tools/Sprite 合并工具")]
    public static void ShowWindow()
    {
        GetWindow<SpriteCombinerWindow>("Sprite 合并");
    }

    void OnGUI()
    {
        GUILayout.Label("输入最多 4 个 Sprite", EditorStyles.boldLabel);

        for (int i = 0; i < inputSprites.Length; i++)
        {
            inputSprites[i] = (Sprite)EditorGUILayout.ObjectField($"图层 {i + 1}", inputSprites[i], typeof(Sprite), false);
        }

        outputSize = EditorGUILayout.IntPopup("输出大小", outputSize, new[] {"128", "256", "512", "1024"}, new[] {128, 256, 512, 1024});

        if (GUILayout.Button("合成并保存"))
        {
            CombineAndSave();
        }
    }

    void CombineAndSave()
    {
        var validSprites = System.Array.FindAll(inputSprites, s => s != null);
        if (validSprites.Length == 0)
        {
            Debug.LogWarning("没有输入 Sprite");
            return;
        }

        EnsurePersistentCamera(outputSize);

        // 创建临时对象并设置 Layer
        GameObject[] tempGOs = new GameObject[validSprites.Length];
        for (int i = 0; i < validSprites.Length; i++)
        {
            var go = new GameObject("TempSprite" + i);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = validSprites[i];
            sr.sortingOrder = i;
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.position = Vector3.zero;
            tempGOs[i] = go;
        }

        // 渲染
        persistentCam.Render();
        RenderTexture.active = persistentRT;

        Texture2D result = new Texture2D(outputSize, outputSize, TextureFormat.ARGB32, false);
        result.ReadPixels(new Rect(0, 0, outputSize, outputSize), 0, 0);
        result.Apply();

        RenderTexture.active = null;

        foreach (var go in tempGOs)
            DestroyImmediate(go);

        // 保存纹理到项目中
        string path = EditorUtility.SaveFilePanelInProject("保存合成图", "CombinedSprite", "png", "选择保存路径");
        if (!string.IsNullOrEmpty(path))
        {
            var bytes = result.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
            Debug.Log("保存成功: " + path);
        }
    }

    void EnsurePersistentCamera(int size)
    {
        if (persistentCameraGO == null)
        {
            persistentCameraGO = new GameObject("__PersistentSpriteCam");
            persistentCameraGO.hideFlags = HideFlags.HideAndDontSave;
            persistentCam = persistentCameraGO.AddComponent<Camera>();
            persistentCam.clearFlags = CameraClearFlags.SolidColor;
            persistentCam.backgroundColor = new Color(0, 0, 0, 0);
            persistentCam.orthographic = true;
            persistentCam.cullingMask = 1 << LayerMask.NameToLayer("UI");
        }

        persistentCam.orthographicSize = size / 2f;

        if (persistentRT == null || persistentRT.width != size)
        {
            persistentRT = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
            persistentRT.Create();
            persistentCam.targetTexture = persistentRT;
        }
    }
}

// ===============================
// ✅ 挂载到 Tile 的调用方式（运行时）
// ===============================
// public class TileSpriteCombiner : MonoBehaviour
// {
//     public Sprite[] tileSprites; // 挂上要合并的 Sprite
//     public int size = 256;

//     void Start()
//     {
//         Texture2D tex = SpriteCombinerWindowHelper.CombineSprites(tileSprites, size);
//         var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
//         GetComponent<SpriteRenderer>().sprite = sprite;
//     }
// }

// public static class SpriteCombinerWindowHelper
// {
//     public static Texture2D CombineSprites(Sprite[] sprites, int size)
//     {
//         GameObject camGO = new GameObject("TempTileCam");
//         Camera cam = camGO.AddComponent<Camera>();
//         cam.clearFlags = CameraClearFlags.SolidColor;
//         cam.backgroundColor = new Color(0, 0, 0, 0);
//         cam.orthographic = true;
//         cam.orthographicSize = size / 2f;
//         cam.cullingMask = 1 << LayerMask.NameToLayer("UI");

//         RenderTexture rt = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
//         rt.Create();
//         cam.targetTexture = rt;

//         GameObject[] tempGOs = new GameObject[sprites.Length];
//         for (int i = 0; i < sprites.Length; i++)
//         {
//             var go = new GameObject("TileSprite" + i);
//             var sr = go.AddComponent<SpriteRenderer>();
//             sr.sprite = sprites[i];
//             sr.sortingOrder = i;
//             go.layer = LayerMask.NameToLayer("UI");
//             tempGOs[i] = go;
//         }

//         cam.Render();
//         RenderTexture.active = rt;

//         Texture2D result = new Texture2D(size, size, TextureFormat.ARGB32, false);
//         result.ReadPixels(new Rect(0, 0, size, size), 0, 0);
//         result.Apply();

//         RenderTexture.active = null;

//         foreach (var go in tempGOs) Object.Destroy(go);
//         Object.Destroy(camGO);
//         Object.Destroy(rt);

//         return result;
//     }
// }
