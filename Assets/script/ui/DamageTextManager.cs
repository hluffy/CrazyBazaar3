using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;

    [Header("Damage Text Prefab")]
    [SerializeField] private GameObject damageTextPrefab;

    [Header("Canvas")]
    [SerializeField] private Canvas canvas;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ShowDamageText(int _damage, Vector3 _worldPosition, Color _color)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_worldPosition + Vector3.up * 3.5f);

        // 实例化时使用Vector3.zero位置，避免坐标系混淆
        GameObject damageTextObject = Instantiate(damageTextPrefab, Vector3.zero, Quaternion.identity);
        damageTextObject.transform.SetParent(canvas.transform, false);

        // 设置正确的锚点和位置
        RectTransform rectTransform = damageTextObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.position = screenPosition;
        }

        DamageTextMeshPro damageText = damageTextObject.GetComponent<DamageTextMeshPro>();
        if (damageText != null)
        {
            damageText.SetupDamage(_damage,_color);
        }   
    }
}
