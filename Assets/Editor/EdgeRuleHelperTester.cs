using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
public class EdgeRuleHelperTester : EditorWindow
{
    Sprite spriteA;
    Sprite spriteB;
	Sprite spriteC;
    Color colorA = Color.green;

	Sprite combine;

	private int id = 0; 
	private int id_index = 0; 

	private int currId = 0; 
	string neihbours = "00000000";

	

    [MenuItem("Tools/Test EdgeRuleHelper")]
    public static void ShowWindow()
    {
        GetWindow<EdgeRuleHelperTester>("EdgeRuleHelper 测试");
    }

	void OnGUI()
	{
		spriteA = (Sprite)EditorGUILayout.ObjectField("spriteA", spriteA, typeof(Sprite), false);
		spriteB = (Sprite)EditorGUILayout.ObjectField("spriteB", spriteB, typeof(Sprite), false);
		spriteC = (Sprite)EditorGUILayout.ObjectField("spriteC", spriteC, typeof(Sprite), false);

		combine = (Sprite)EditorGUILayout.ObjectField("combine", combine, typeof(Sprite), false);
		colorA = EditorGUILayout.ColorField("目标颜色", colorA);

		currId = EditorGUILayout.IntField("currId", currId);
		neihbours = EditorGUILayout.TextField("neihbours", neihbours);
		if (GUILayout.Button("获取id-index"))
		{
			// int[] nei = { 4, 0, 3, 0, 3, 3, 3, 1 };
			int[] nei = neihbours.Select(c => int.Parse(c.ToString())).ToArray();
			Debug.Log("input："+ string.Join(", ", nei));
			Dictionary<int, int> result = EdgeRuleHelper.GetEdgeIndex(currId, nei); 

			string dictStr = string.Join(", ", result.Select(kv => kv.Key + ":" + kv.Value));
			Debug.Log("result："+dictStr);

			combine = EdgeRuleHelper.MixSprite(result);

		}

		if (GUILayout.Button("混合sprite"))
		{
			List<Sprite> sprites = new List<Sprite>();

			sprites.Add(spriteA);
			sprites.Add(spriteB);
			sprites.Add(spriteC);
			Sprite xx=SpriteCache.FindFromCache("1213123");
			if (xx != null)
			{
				combine = xx;
			}
			else
			{
				combine = SpriteCache.CombineSprites(sprites, 64, "1213123");
			}
			

		}
		if (GUILayout.Button("导出图片"))
		{
			var tex = combine.texture;
			byte[] pngData = tex.EncodeToPNG();
			File.WriteAllBytes("Assets/Resources/tileMix/tile_123.png", pngData);
			AssetDatabase.Refresh();


		}
		id = EditorGUILayout.IntField("id", id);
		id_index = EditorGUILayout.IntField("id_index", id_index);
			if (GUILayout.Button("打印多图中的某个小图"))
		{
			// Sprite aa = EdgeRuleHelper.GetSpriteFromResource(0, 0);


			combine = EdgeRuleHelper.GetSpriteFromResource(id, id_index);

			// Texture2D xxx = aa.texture;
			// int width = xxx.width;
			// int height = xxx.height;
			// Debug.Log("--------CombineSprites :" + width + ":" + height);
		}
    }
}
