using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance;

    public Player player;
    public GameObject flowChartObject;
    public GameObject bossCharacterObject;

    public Flowchart flowchart { get; private set; }
    public Character bossCharacter{ get; private set; }

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

    void Start()
    {
        flowchart = flowChartObject.GetComponent<Flowchart>();

        bossCharacter = bossCharacterObject.GetComponent<Character>();
    }

    public void SetBossCharacter(string _nametext, Sprite _sprite)
    {
        bossCharacter.SetStandardText(_nametext);
        // bossCharacter.Portraits[0] = _sprite;
    }

    public bool IsInteractiveUIElement(GameObject obj)
    {
        // 检查是否是真正需要阻止角色控制的交互式 UI 元素
        Button button = obj.GetComponent<Button>();
        Slider slider = obj.GetComponent<Slider>();
        Toggle toggle = obj.GetComponent<Toggle>();
        InputField inputField = obj.GetComponent<InputField>();
        Scrollbar scrollbar = obj.GetComponent<Scrollbar>();
        Dropdown dropdown = obj.GetComponent<Dropdown>();
        Image image = obj.GetComponent<Image>();

        return button != null || slider != null || toggle != null ||
            inputField != null || scrollbar != null || dropdown != null || image != null;
    }
    
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        // 过滤掉 Canvas 本身，只检查实际的 UI 元素
        foreach (RaycastResult result in results)
        {
            // 检查是否是实际的 UI 元素（而不是 Canvas 背景）
            if (IsInteractiveUIElement(result.gameObject))
            {
                return true;
            }
        }
        
        return false;
    }

    public void LoadData(GameData _data)
    {
        player.transform.position = _data.playerPosition;
    }

    public void SaveData(ref GameData _data)
    {
        _data.playerPosition = player.transform.position;
    }
}
