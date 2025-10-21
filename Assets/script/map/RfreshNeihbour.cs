using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;
using System.IO;

public class RfreshNeihbour : MonoBehaviour
{
	public Tilemap tilemap;
	public GameObject textObject;


	public void SetTile(Vector3Int position, TypedTile tile, int mouse)
	{
		int currId = (int)tile.tileType;
		// 获取到邻居
	
		tilemap.SetTile(position, tile);
		
		for (int i = 0; i < neighbors.Length; i++)
		{
			
			 tilemap.RefreshTile(position + neighbors[i]);
			 
		}


		// setSprite(tile);
	}
	private static readonly Vector3Int[] neighbors =
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
 
	void setSprite(Sprite combine)
	{
		SpriteRenderer spriteRenderer = textObject.GetComponent<SpriteRenderer>();
      

        // 赋值 Sprite
        spriteRenderer.sprite = combine;
	}
 
}
