// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using UnityEngine;
// using UnityEngine.Tilemaps;

// // 创建 覆盖图，可以传递mask ID给shader
// public partial class Tile_cursor : MonoBehaviour
// {


//     // public DualGridTilemap dualGridTilemap;

//     public Tilemap tilemap;

//     public TypedTile left;
//     public TypedTile right;

//     public float maxDistance = 25f;
//     public LayerMask layerMask;

//     public TypedTile testTileBase;

//     public RfreshNeihbour rfreshNeihbour;
//     public bool open=false;
     

//     void Start()
//     {
     
//     }

//     void Update()
//     {
//         //  if (!open)
//         // {
//         //     return;
//         // }
//         // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         // RaycastHit hit;

//         // if (Physics.Raycast(ray, out hit,maxDistance,layerMask))
//         // {
//         //     if (hit.collider.gameObject == tilemap.gameObject)
//         //     {
//         //         Vector3 hitPoint = hit.point;
//         //         Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);
//         //         Vector3 cellCenterWorldPos = tilemap.GetCellCenterWorld(cellPosition);

//         //         transform.position = cellCenterWorldPos;

//         //         Vector3Int tilePosition = tilemap.WorldToCell(transform.position);

//         //         if (Input.GetMouseButtonDown(0))
//         //         {
//         //             tilemap.SetTile(tilePosition, testTileBase);
//         //             testTileBase.RefreshTile(tilePosition, tilemap);
//         //         }



//         //         // if (Input.GetMouseButton(0))
//         //         // {
//         //         //     rfreshNeihbour.SetTile(cellPosition, left, 0);

//         //         // }
//         //         // else if (Input.GetMouseButton(1))
//         //         // {
//         //         //     rfreshNeihbour.SetTile(cellPosition, right, 1);
//         //         // }
//         //     }

//         // }

//         // var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//         // Vector3Int tilePos = GetWorldPosTile(mouseWorldPos);
//         // transform.position = tilePos + new Vector3(0.5f, 0.5f, -1);

//         // if (Input.GetMouseButtonUp(0))
//         // {
//         //     rfreshNeihbour.SetTile(tilePos, left,0);
//         // }
//         // else if (Input.GetMouseButtonUp(1))
//         // {
//         //     // SetTile(tilePos, null);
//         //     rfreshNeihbour.SetTile(tilePos, right,1);
//         // }
//     }

//     public static Vector3Int GetWorldPosTile(Vector3 worldPos)
//     {
//         int xInt = Mathf.FloorToInt(worldPos.x);
//         int yInt = Mathf.FloorToInt(worldPos.y);
//         return new(xInt, yInt, 0);
//     }
// }
