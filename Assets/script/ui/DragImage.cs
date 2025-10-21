using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DragImage : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;

    private bool isDragging = false;
    private Sprite sprite;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void SetupDragImage(Sprite _sprite)
    {
        if (_sprite == null)
            return;
        sprite = _sprite;
        image.sprite = sprite;
        image.color = Color.clear;
        gameObject.SetActive(true);

        Invoke(nameof(SetColor), 0.1f);
    }

    private void SetColor()
    {
        image.color = Color.white;
    }


    void OnEnable()
    {
        isDragging = true;
        rectTransform.position = GetPos();
    }

    void OnDisable()
    {
        isDragging = false;
        sprite = null;
        image.sprite = null;
    }

    private void Update()
    {
        if (isDragging && sprite != null)
        {
            // rectTransform.anchoredPosition = GetPos();

            Vector2 position = Input.mousePosition;
        
            // 如果Canvas是ScreenSpace-Camera或World模式，需要转换坐标
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    rectTransform.parent as RectTransform,
                    Input.mousePosition,
                    canvas.worldCamera,
                    out Vector3 worldPos
                );
                position = worldPos;
            }
            
            rectTransform.position = position;
        }
    }

    private Vector2 GetPos()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 movePos
            );
        return movePos;
    }
}
