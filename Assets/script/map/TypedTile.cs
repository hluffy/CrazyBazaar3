using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "TypedTile", menuName = "2D/Tiles/Typed Tile")]
public class TypedTile : Tile
{
    // None = -1 是默认地皮
    public enum TileType { None = 0, Grass = 1, Stone = 2, Sea = 4, Maple = 3 }
    public TileType tileType=TileType.Grass;
    // public int priority = 0; // 优先级，优先级大的，覆盖在其他上面
     
 
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // 刷新自己
        base.RefreshTile(position, tilemap);
        // for (int i = 0; i < k_Neighbors.Length; i++)
        // {
        //     Vector3Int nPos = position + k_Neighbors[i];
        //     tilemap.RefreshTile(nPos);
        // }
    }
    public static readonly Vector3Int[] neighbors =
		{
		Vector3Int.up, // 上
		Vector3Int.right, // 右
        Vector3Int.down, // 下
        Vector3Int.left, // 左
		new Vector3Int(-1,  1, 0), // 左上
		new Vector3Int( 1,  1, 0), // 右上
		new Vector3Int( 1, -1, 0), // 右下
		new Vector3Int(-1, -1, 0), // 左下
    };
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        // tileData.color = EdgeRuleHelper.tileColors[(int)tileType];
        // Color originalColor = tileData.color;
        // //例：如果上下左右都是 null，就换一个 sprite

        // int[] nei = new int[8];
        // int currId = (int)this.tileType;
        // for (int i = 0; i < neighbors.Length; i++)
        // {
        //     TypedTile neibour = tilemap.GetTile(position + neighbors[i]) as TypedTile;
        //     if (neibour != null && (int)neibour.tileType > currId)
        //     {
        //         nei[i] = (int)neibour.tileType;
        //     }
        // }
        
		// // 获取到纹理的id的对应的index
		// Dictionary<int, int> id_Index = EdgeRuleHelper.GetEdgeIndex(currId, nei);
		// //
		// Sprite combine = EdgeRuleHelper.MixSprite(id_Index);

		// tileData.sprite = combine;
		 
    }

}