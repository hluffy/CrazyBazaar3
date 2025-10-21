using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// 将所有非透明像素改成指定颜色，保留 Alpha
public static class FileCache
{

	public static void SaveInAssets(Sprite combine, string mixName)
	{
		Texture2D tex = combine.texture;
		byte[] pngData = tex.EncodeToPNG();
		string savePath = Path.Combine(Application.persistentDataPath, mixName+".png");
		Debug.Log("-----保存地址："+savePath);
		// string savePath = C:/Users/18801/AppData/LocalLow/DefaultCompany/CrazyBazaar\tile_2-0_4-7.png
		File.WriteAllBytes(savePath, pngData);
		 
	 
	}
	public static Sprite LoadSprite(string fileName)
    {
        // 先去Assets/Resources/combine文件夹下找
        //  Debug.Log("去Assets/Resources/combine: " + fileName+".png");
        // fileName = "tile_0-0_1-1";
        Sprite sprite0 = Resources.Load<Sprite>( "combine/"+fileName);
        if (sprite0 != null)
        {
            // Debug.Log("我在Resources下找到了: " + fileName+".png");
            return sprite0;
        }
        // 去文件合成文件夹找，
        string filePath = Path.Combine(Application.persistentDataPath, fileName+".png");
        
        // 1. 检查文件是否存在
        if (File.Exists(filePath))
        {
            // 2. 读取 PNG 数据
            byte[] pngData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngData); // 自动解析 PNG

            // 3. 创建 Sprite 并设置 PPU=64
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f), // 中心点
                64 // Pixels Per Unit
            );

            return sprite;
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }
}