using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> datas = new List<string>();
    public GameObject leftObject;
    public GameObject rightObject;
    public Transform parentTransForm;
    public InputField input;
    public Transform scrollView;

    /// <summary>
    /// 克隆一个GameObject
    /// </summary>
    public void InstantiateList(ChatEntity chatEntity)
    {
        Debug.Log("复制一个右边的物体");
        GameObject chatRight= GameObject.Instantiate(rightObject, parentTransForm);
        GameObject msgObj=chatRight.transform.GetChild(1).gameObject;// 文字
        Text text=msgObj.GetComponent<Text>();
        text.text = chatEntity.msg;
        
    }
    void Start()
    {
        Debug.Log("聊天面板打开了, global.SetChatPanelActive 设置为true");
        datas.Add("Hi，第一次见面，外乡人");
        datas.Add("你想要点什么");
       
    }

    public void OnChatValueChange()
    {
       
    }
    public void OnChatCloseClick()
    {
        // 如果聊天面板打开，就关闭他
        GameObject UI = GameObject.Find("Canvas");
        GameObject theDialog = UI.transform.Find("Chat").gameObject;
        theDialog.SetActive(false);

        User.userState = UserState.Idil;
    }
    public void OnChatAdd()
    {
        print("asddddddddddddd:"+ input.text);
        if (StringUtility.IsNullOrWhiteSpace(input.text))
        {
            return;
        }
        ChatEntity chatEntity = new ChatEntity();
        chatEntity.msg = input.text;
       

        InstantiateList(chatEntity);

        
    }
    private void OnEnable()
    {
       
    }
    private void OnDisable()
    {
        Debug.Log("聊天面板关闭了 global.SetChatPanelActive 设置为false");
        
    }
    private void OnDestroy()
    {
       
    }
    
}
