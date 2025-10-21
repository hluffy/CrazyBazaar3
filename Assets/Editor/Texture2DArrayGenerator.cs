using UnityEngine;
using UnityEditor;
using System.IO;

public class Texture2DArrayGenerator
{
    [MenuItem("Assets/Create/Texture2DArray From Selection")]
    public static void CreateTexture2DArray()
    {
        Object[] selectedTextures = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);

        if (selectedTextures.Length == 0)
        {
            Debug.LogWarning("No textures selected.");
            return;
        }

        Texture2D firstTex = (Texture2D)selectedTextures[0];
        int width = firstTex.width;
        int height = firstTex.height;
        TextureFormat format = firstTex.format;

        Texture2DArray textureArray = new Texture2DArray(width, height, selectedTextures.Length, format, false);
        textureArray.wrapMode = TextureWrapMode.Clamp;
        textureArray.filterMode = FilterMode.Bilinear;

        for (int i = 0; i < selectedTextures.Length; i++)
        {
            Texture2D tex = (Texture2D)selectedTextures[i];

            if (tex.width != width || tex.height != height)
            {
                Debug.LogError($"Texture {tex.name} has a different size. All textures must be the same size.");
                return;
            }

            if (!tex.isReadable)
            {
                Debug.LogError($"Texture {tex.name} is not readable. Please enable 'Read/Write' in the import settings.");
                return;
            }

            Graphics.CopyTexture(tex, 0, 0, textureArray, i, 0);
        }

        string path = EditorUtility.SaveFilePanelInProject("Save Texture2DArray", "NewTexture2DArray", "asset", "Enter a file name for the Texture2DArray.");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(textureArray, path);
            AssetDatabase.SaveAssets();
            Debug.Log("Texture2DArray saved to: " + path);
        }
    }
}
