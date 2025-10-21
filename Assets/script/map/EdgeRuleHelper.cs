using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;
using System.IO;

public static class EdgeRuleHelper
{
	public static readonly Dictionary<int, Color> tileColors = new Dictionary<int, Color>
	{
		{ (int)TypedTile.TileType.None, new Color(1f, 1f, 1f) },
		{ (int)TypedTile.TileType.Grass,new Color(0f, 1f, 0f) },
		{ (int)TypedTile.TileType.Stone,new Color(1f, 1f, 0f)},
		{ (int)TypedTile.TileType.Sea,  new Color(0f, 0f, 1f) },
		{ (int)TypedTile.TileType.Maple, new Color(1f, 0f, 0f) } // 类似橙红色
    };
 
    public static readonly Dictionary<int, string> IdToName = new  Dictionary<int, string>
    {
        { 0, "None" }, // 基础地图
        { 1, "Grass" },
        { 2, "Stone" },
        { 3, "Maple" },
        { 4, "Sea" }
    };
 
	// 优先级

	// 8 邻居方向（顺序：上、右、下、左、左上、右上、右下、左下）
	// 例如 neighbors = [1, 0, 1, 1, 0, 0, 0, 0]
	// 1 代表有相同类型的地皮，0 代表不同或空。
	// 1 代表邻居优先级高的地皮，0 代表邻居优先级低，或空，或者地皮相同。
	// 当邻居优先级高的时候，需要修改当前地皮
	// 当颜色一致的时候，优先冲库存里查找，不一致的时候，代码混合处理
	private static Dictionary<string, int> baseTable = new Dictionary<string, int>()
	{
		{ "0000", 0 },  // 孤立

        { "1000", 1 },  // 上
        { "0100", 2 },  // 右
        { "0010", 3 },  // 下
        { "0001", 4 },  // 左

        { "1100", 5 },  // 上+右
        { "0110", 6 },  // 右+下
        { "0011", 7 },  // 下+左
        { "1001", 8 },  // 上+左
        { "1010", 9 },  // 上+下
        { "0101", 10 }, // 左+右

        { "1110", 12 }, // 上+右+下
        { "0111", 13 }, // 右+下+左
        { "1011", 14 }, // 下+左+上
        { "1101", 11 }, // 左+上+右
        { "1111", 15 } // 十字
	};

	private static Dictionary<string, int> allTable = new Dictionary<string, int>()
	{
		{ "00000000", 0 },  // 无角
        { "00001000", 16 },  // 左上
        { "00000100", 17 },  // 右上
        { "00000001", 18 },  // 坐下
		{ "00000010", 19 },  // 右下

        { "00001100", 20 },  // 上+右
		{ "00001010", 21 },  // 上+下
		{ "00001001", 22 },  // 上+左 18
        { "00000110", 23 },  // 右+下 19
		{ "00000101", 24}, // 左+右 20
        { "00000011", 25 },  // 下+左 
       

        { "00001110", 26 }, // 上+右+下
        { "00000111", 27 }, // 右+下+左
        { "00001011", 28 }, // 下+左+上
        { "00001101", 29 }, // 左+上+右 ===================缺少
        { "00001111", 30 }, // 十字

		{ "10000010", 31 },  // 上+右下
		{ "10000001", 32 },  // 孤立
		{ "10000011", 33 },  // 孤立

		{ "01001000", 34 },  // 孤立
		{ "01000001", 35 },  // 右+左下
		{ "01001001", 36 },  // 右+左下

		{ "00101000", 37 },  // 孤立
		{ "00100100", 38 },  // 下+右上
		{ "00101100", 39 },  // 下+右上
	 


		{ "00010100", 40 },  // 孤立.    ===================缺少
		{ "00010010", 41 },  // 左+右下.  ===================缺少
		{ "00010110", 42 },  // 左+右下.   ===================缺少

		{ "11000001", 43 },  // 上+右+左下
		{ "01101000", 44 },  // 孤立
		{ "00110100", 45 },  // 孤立
		{ "10010010", 46 },  // 孤立

		// { "1010 0010", 44 },  // 不需要
		// { "0101 0010", 44 },  // 不需要
    };
	private static int[] first4 = new int[4];
	private static int[] last4 = new int[4];
	// nei 优先级差  neibour.priority - curr.priority


	public static Sprite MixSprite(Dictionary<int, int> idIndex)
	{
		if (idIndex.Count == 1)
		{
			// 单色
			var (key, value) = idIndex.First();
			// Debug.Log("-----MixSprite 单色："+key+"_"+value);
			
			Sprite data = GetSpriteFromResource(key, value);
			return data;
		}

		var sortedByKey = idIndex.OrderBy(kv => kv.Key);


		// 混合资源库里查找
		string mixName = "tile_" +
			string.Join("_", sortedByKey.Select(kv => $"{kv.Key}-{kv.Value}"));

		// Debug.Log("=====MixSprite :"+mixName);
		Sprite sprite=SpriteCache.FindFromCache(mixName);
		if (sprite != null) return sprite;

		List<Sprite> spriteList = new List<Sprite>();
		foreach (var item in sortedByKey)
		{
				// 去Assets/Resource/tile/Stone.png 里找基础图
				Sprite data = GetSpriteFromResource(item.Key, item.Value);
				if(data!=null) spriteList.Add(data);
		}
		// 去拼接，

		Sprite combine = SpriteCache.CombineSprites(spriteList, 64, mixName);
		FileCache.SaveInAssets(combine,mixName); //保存到本地文件夹
		return combine;

 
	}
	
		// 被覆盖掉的低优先级的被归于0。可能得到结果[2,3,0,1,1,3,4,2] ---> [2,3,0,1,0,0,4,2]
		// 这样可以减少图形叠加处理
		// 将 [2,3,0,1,0,0,4,2]。归纳处理成:
		// 	2,{1,0,0,0,0,0,0,1}
		// 	3,{0,1,0,0,0,0,0,0}
		// 	1,{0,0,0,1,0,0,0,0}
		// 	4,{0,0,0,0,0,0,1,0}
	public static Dictionary<int, int> GetEdgeIndex(int currId, int[] nei)
	{
		if (nei == null || nei.Length != 8)
			throw new System.ArgumentException("neighbors 数组必须是长度 8 的 0/1 数组！");


		// 左侧放id， 右侧放index，index是值在multi纹理的第几张图
		Dictionary<int, int> idIndexs = new Dictionary<int, int>();
		idIndexs.Add(currId, 0);

		int index = 0;
		// ####  邻居都是0
		bool allZero = nei.All(x => x == 0);
		if (allZero) return idIndexs; // 四周全部是0


		// 高优先级覆盖低优先级
		// Debug.Log("------------GetEdgeIndex0:"+string.Join(", ", nei));
		for (int i = 0; i < 8; i++)
		{
			if (nei[i] <= currId) nei[i] = 0;

			if (nei[i] > 0 && i < 4)
			{
				if (nei[i] >= nei[i + 4]) nei[i + 4] = 0;
				if (i != 3 && nei[i] >= nei[i + 5]) nei[i + 5] = 0;
				if (i == 3 && nei[i] >= nei[4]) nei[4] = 0;
			}
		}

		// Debug.Log("------------GetEdgeIndex1:"+string.Join(", ", nei));
		// #### 邻居同色
		var filtered = nei.Where(x => x != 0).ToArray();

		allZero = nei.All(x => x == 0);
		if (allZero) return idIndexs; // 四周全部是0

		bool allSame = filtered.Length == 0 || filtered.All(x => x == filtered[0]);

		if (allSame)
		{   // 邻居同色
			// Debug.Log("------------2 allSame:");
			int[] result11 = nei.Select(x => x > 0 ? 1 : x).ToArray(); // 将大于1的转成1
			string newnei = string.Join("", result11);
			index = allTable.GetValueOrDefault(newnei, -1);
			if (index == -1) index = baseTable.GetValueOrDefault(newnei.Substring(0, 4), -1);
			int id = nei.FirstOrDefault(x => x > 0);
			idIndexs.Add(id, index);
			return idIndexs;
		}
		// Debug.Log("------------GetEdgeIndex3:"+string.Join(", ", nei));
		// #### 邻居多色，需要根据优先级，判断覆盖
		// 被覆盖掉的低优先级的被归于0。可能得到结果[2,3,0,1,1,3,4,2] ---> [2,3,0,1,0,0,4,2]
		// 这样可以减少图形叠加处理
		// 将 [2,3,0,1,0,0,4,2]。归纳处理成:
		// 	2,{1,0,0,0,0,0,0,1}
		// 	3,{0,1,0,0,0,0,0,0}
		// 	1,{0,0,0,1,0,0,0,0}
		// 	4,{0,0,0,0,0,0,1,0}
		Dictionary<int, int[]> result = Calculat(nei);

		foreach (var pair in result)
		{
			// Debug.Log($"{pair.Key},{{{string.Join(",", pair.Value)}}}");

			int id = pair.Key;
			int[] value = pair.Value;
			string newnei = string.Join("", value);
			index = allTable.GetValueOrDefault(newnei, -1);
			// Debug.Log("------------4:"+newnei);
			// Debug.Log("------------5:"+newnei.Substring(0, 4));
			if (index == -1) index = baseTable.GetValueOrDefault(newnei.Substring(0, 4), -1);

			idIndexs.Add(id, index);
		}

		return idIndexs;
	}

	public static Sprite GetSpriteFromResource(int id, int index)
	{
		// Debug.Log(" 去Assets/Resource/tile/里找基础图 "+id+":"+index);
		if (index < 0) return null;
		string name= IdToName[id];
		// Debug.Log(" 去Assets/Resource/tile/里找基础图 :"+id+":"+index+":"+name);
		Sprite[] grassSprites = Resources.LoadAll<Sprite>("tile/" + name);
		// Debug.Log(" 去Assets/Resource/tile/里找基础图 :"+grassSprites.Length);
		
		return grassSprites[index];
	}

	private static readonly Vector3Int[] up_Neighbors =
		{
		Vector3Int.up, // 上
		Vector3Int.right, // 右
        Vector3Int.down, // 下
        Vector3Int.left, // 左
    };
	private static readonly Vector3Int[] k_Neighbors =
	   {
		new Vector3Int(-1,  1, 0), // 左上
        new Vector3Int( 1,  1, 0), // 右上
        new Vector3Int( 1, -1, 0), // 右下
        new Vector3Int(-1, -1, 0), // 左下
    };
	// 1 代表邻居优先级高的地皮，0 代表邻居优先级低，或空，或者地皮相同。
	public static int[] GetNeighborsStates(Vector3Int currPosition, int priority, Tilemap tilemap
			 )
	{
		int[] nei = new int[8];
		 
		for (int i = 0; i < up_Neighbors.Length; i++)
		{
			TypedTile neibour = tilemap.GetTile(currPosition + up_Neighbors[i]) as TypedTile;
			if (neibour != null && (int)neibour.tileType > priority)
			{
				nei[i] = (int)neibour.tileType;
			 
			}

		}
		for (int i = 0; i < k_Neighbors.Length; i++)
		{
			TypedTile neibour = tilemap.GetTile(currPosition + k_Neighbors[i]) as TypedTile;
			if (neibour != null && (int)neibour.tileType > priority)
			{
				nei[4 + i] = (int)neibour.tileType;
				 
			}
		}
		return nei;
	} 
	

	private static  Dictionary<int, int[]>   Calculat(int[] nei)
    {
        // int[] nei = { 2, 3, 0, 1, 0, 0, 4, 2 };
        int length = nei.Length;

        Dictionary<int, int[]> dict = new Dictionary<int, int[]>();

        for (int i = 0; i < length; i++)
        {
            int value = nei[i];

            if (value == 0) continue; // 跳过值为0的

            if (!dict.ContainsKey(value))
            {
                dict[value] = new int[length];  // 初始化全 0 数组
            }

            dict[value][i] = 1;  // 设置当前位置为 1
        }
		return dict;
        // 输出查看结果
        // foreach (var pair in dict)
        // {
        //     Console.WriteLine($"{pair.Key},{{{string.Join(",", pair.Value)}}}");
        // }
    }
}
