using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "MyTile", menuName = "2D/Tiles/MyTile")]
public class MyTile : Tile
{
    // None = -1 是默认地皮
    public enum TileType { None = 0, Grass = 1, Stone = 2, Sea = 4, Maple = 3 }
    public TileType tileType=TileType.Grass;
     
  

}