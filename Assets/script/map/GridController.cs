using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public Transform cursor;
    [SerializeField]
    private float maxDistance = 25f;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Transform plantTilemap;

    [SerializeField]
    private GameObject cropPrefab;

    private Tilemap tilemap;

    private InventoryManager inventory;

    void Start()
    {
        cursor.gameObject.SetActive(false);

        tilemap = GetComponentInChildren<Tilemap>();

        inventory = InventoryManager.Instance;
    }
    void Update()
    {
        ItemSlotData itemSlotData = inventory.selectItemSlotData;
        if (itemSlotData != null && itemSlotData.itemData != null && itemSlotData.quantity > 0)
        {
            ItemData itemData = itemSlotData.itemData;
            if (itemData is LandData)
            {
                LandData landData = itemData as LandData;
                if (landData.tile != null)
                {
                    TypedTile tile = landData.tile;

                    cursor.gameObject.SetActive(true);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
                    {
                        if (hit.collider.gameObject == tilemap.gameObject)
                        {
                            Vector3 hitPoint = hit.point;
                            Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);
                            Vector3 cellCenterWorldPos = tilemap.GetCellCenterWorld(cellPosition);

                            cursor.position = cellCenterWorldPos;

                            Vector3Int tilePosition = tilemap.WorldToCell(cellCenterWorldPos);

                            if (Input.GetMouseButtonDown(0))
                            {
                                if (PlayerManager.instance.IsPointerOverUIObject())
                                    return;

                                TileBase tileBase = tilemap.GetTile(tilePosition);
                                Debug.Log(tileBase == tile);
                                if (tileBase == tile)
                                    return;

                                tilemap.SetTile(tilePosition, tile);
                                tile.RefreshTile(tilePosition, tilemap);

                                Vector3Int[] neighbors = TypedTile.neighbors;

                                for (int i = 0; i < neighbors.Length; i++)
                                {
                                    Vector3Int newPosition = tilePosition + neighbors[i];
                                    TypedTile typedTile = tilemap.GetTile(newPosition) as TypedTile;
                                    typedTile?.RefreshTile(newPosition, tilemap);
                                }

                                inventory.DesreaseSelectItemSlotData(1);
                            }
                            return;
                        }

                    }
                }
            }
            else if (itemData is SeedData)
            {
                SeedData seedData = itemData as SeedData;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
                {
                    Vector3 hitPoint = hit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (PlayerManager.instance.IsPointerOverUIObject())
                            return;

                        GameObject cropObject = Instantiate(cropPrefab, plantTilemap);
                        cropObject.transform.position = hitPoint;
                        CropBehaviour cropPlanted = cropObject.GetComponent<CropBehaviour>();
                        cropPlanted.Plant(seedData);
                        inventory.DesreaseSelectItemSlotData(1);
                    }
                }
            }
        }
        cursor.gameObject.SetActive(false);
    }
}
