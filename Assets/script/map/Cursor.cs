
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class Cursor : MonoBehaviour
{

    public Tilemap tilemap;
    public Tile tile;

    public bool open = false;
    void Update()
    {


        // var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // // Vector3Int tilePos = GetWorldPosTile(mouseWorldPos);
        // Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
        // Vector3 cellCenterWorldPos = tilemap.GetCellCenterWorld(cellPosition);

        // print("xxxx" + cellCenterWorldPos);

        // transform.position = cellCenterWorldPos + new Vector3(0.5f, 0.5f, -1);
        if (!open)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == tilemap.gameObject)
            {
                Vector3 hitPoint = hit.point;
                Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);
                Vector3 cellCenterWorldPos = tilemap.GetCellCenterWorld(cellPosition);

                Debug.Log("3D Tilemap cell position: " + cellPosition);
                Debug.Log("World position: " + cellCenterWorldPos);



                transform.position = cellCenterWorldPos + new Vector3(0f, 0.5f, -0.5f);

                if (Input.GetMouseButton(0))
                {
                    tilemap.SetTile(cellPosition, tile);

                }
                else if (Input.GetMouseButton(1))
                {
                    tilemap.SetTile(cellPosition, null);
                }
            }

        }


    }

    public static Vector3Int GetWorldPosTile(Vector3 worldPos)
    {
        int xInt = Mathf.FloorToInt(worldPos.x);
        int yInt = Mathf.FloorToInt(worldPos.y);
        return new(xInt, yInt, 0);
    }

    public void check()
    {
        if (Input.GetMouseButtonDown(0)) // 当鼠标左键点击时
        {

        }
    }
}