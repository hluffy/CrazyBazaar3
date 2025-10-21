using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// 将所有非透明像素改成指定颜色，保留 Alpha
public static class SpriteCache
{
    private static Dictionary<int, Sprite> cache = new Dictionary<int, Sprite>();
    private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public static Sprite GetColoredSprite(Sprite original, Color from1, Color to1, Color from2, Color to2)
    {
        int key = original.GetInstanceID() ^ from1.GetHashCode() ^ from2.GetHashCode();
        if (cache.TryGetValue(key, out Sprite cached))
            return cached;

        Sprite newSprite = ReplaceColors(original, from1, to1, from2, to2);
        cache[key] = newSprite;
        return newSprite;
    }

    public static Sprite FindFromCache(string cacheKey)
    {
        
        // if (cacheKey != null && spriteCache.ContainsKey(cacheKey))
        // {
        //     // Debug.Log("------内存找到:"+cacheKey);
        //     return spriteCache[cacheKey];
        // }
        // 去assets找
        // Sprite sprite = Resources.Load<Sprite>("tileMix/" + cacheKey);
        Sprite sprite = FileCache.LoadSprite(cacheKey);
        // Debug.Log("------缓存结果:"+(sprite==null));
        return sprite;
    }
  
    // 合成两个Sprite，返回新Sprite
    public static Sprite CombineSprites(List<Sprite> sprites, int w, string cacheKey = null)
    {
        // Debug.Log("--------CombineSprites ");

        // 确定新纹理大小 (这里取最大宽高)
        int width = w;
        int height = w;
        // Debug.Log("--------CombineSprites :" + width + ":" + height);
        // 创建空白纹理
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Color32[] pixels = new Color32[width * height];

        Color clear = new Color(0, 0, 0, 0);
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                result.SetPixel(x, y, clear);
        // 叠加两个纹理像素（简单覆盖叠加，复杂需求可自定义blend）
        foreach (Sprite sprite in sprites)
        {

            if (sprite == null) continue;
            // Texture2D tex1 = sprite.texture;
            // 必须使用GetReadableTexture， 不然sprite.texture; 获取的是整个图的Texture2D
            Texture2D tex1 = GetReadableTexture(sprite);
            BlendOnto(result, tex1);

        }

        result.Apply();

        // 创建新 Sprite
        Sprite combinedSprite =
         Sprite.Create(result, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f),w);

        if (cacheKey != null)
            spriteCache[cacheKey] = combinedSprite;

        // Debug.Log("混合结束");
        return combinedSprite;
    }
public static void BlendOnto(Texture2D target, Texture2D sprite)
{
   
    int w = (int)target.width;
    int h = (int)target.height;

        for (int y = 0; y < h; y++)
        {
            // for (int x = 0; x < w; x++)
            // {

            //     Color src = sprite.GetPixel(x, y);

            //     if (src.a > 0) target.SetPixel(x, y, src);

            // }
             for (int x = 0; x < w; x++)
            {
            Color src = sprite.GetPixel(x, y);
            Color dst = target.GetPixel(x, y);

            // 预乘 alpha 混合
            float outA = src.a + dst.a * (1 - src.a);
            float outR = src.r * src.a + dst.r * (1 - src.a);
            float outG = src.g * src.a + dst.g * (1 - src.a);
            float outB = src.b * src.a + dst.b * (1 - src.a);

            target.SetPixel(x, y, new Color(outR, outG, outB, outA));
            }
        }
}
 
    /// <summary>
    /// 将所有非透明像素改成指定颜色，保留 Alpha
    /// </summary>
    public static Sprite RecolorNonTransparent(Sprite original, Color newColor)
    {
        int key = original.GetInstanceID() ^ newColor.GetHashCode();
        if (cache.TryGetValue(key, out Sprite cached))
            return cached;

        Sprite newSprite = ReplaceNonTransparent(original, newColor);
        cache[key] = newSprite;
        return newSprite;
    }

    private static Sprite ReplaceColors(Sprite sprite, Color from1, Color to1, Color from2, Color to2)
    {
        Texture2D source = GetReadableTexture(sprite);
        Color[] pixels = source.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            Color c = pixels[i];
            float alpha = c.a;

            if (IsCloseTo(c, from1)) c = to1;
            else if (IsCloseTo(c, from2)) c = to2;

            c.a = alpha;
            pixels[i] = c;
        }

        Texture2D newTex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        newTex.SetPixels(pixels);
        newTex.Apply();

        return Sprite.Create(newTex, sprite.rect, new Vector2(0.5f, 0.5f), sprite.pixelsPerUnit);
    }

    // 
    private static Sprite ReplaceNonTransparent(Sprite sprite, Color newColor)
    {
        Texture2D source = GetReadableTexture(sprite);
        Color[] pixels = source.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            Color c = pixels[i];
            if (c.a > 0.01f) // 只改非透明像素
            {
                pixels[i] = new Color(newColor.r, newColor.g, newColor.b, c.a);
            }
        }

        Texture2D newTex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        newTex.SetPixels(pixels);
        newTex.Apply();

        return Sprite.Create(newTex, sprite.rect, new Vector2(0.5f, 0.5f), sprite.pixelsPerUnit);
    }

    private static Texture2D GetReadableTexture(Sprite sprite)
    {
        Texture2D newTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        Color[] colors = sprite.texture.GetPixels(
            (int)sprite.rect.x,
            (int)sprite.rect.y,
            (int)sprite.rect.width,
            (int)sprite.rect.height);
        newTex.SetPixels(colors);
        newTex.Apply();
        return newTex;
    }

    private static bool IsCloseTo(Color a, Color b, float tolerance = 0.2f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }
}
