using UnityEngine;
using UnityEngine.Tilemaps;

public class RotateObjectTilemap : MonoBehaviour
{
    public Transform objectTilemap; // 房子蔬菜的 Tilemap
    public Camera mainCamera;
}

    // void Start()
    // {
    //     foreach (Transform child in objectTilemap)
    //     {
    //         // 原地绕Y轴旋转
    //         child.rotation = mainCamera.transform.rotation;
    //     }
    // }

    // void LateUpdate()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
    //     {
    //         foreach (Transform child in objectTilemap)
    //         {
    //             // 原地绕Y轴旋转
    //             child.rotation = mainCamera.transform.rotation;
    //         }
    //     }

      
    // }
// }
