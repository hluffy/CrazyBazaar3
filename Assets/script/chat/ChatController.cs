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
    /// ��¡һ��GameObject
    /// </summary>
    public void InstantiateList(ChatEntity chatEntity)
    {
        Debug.Log("����һ���ұߵ�����");
        GameObject chatRight= GameObject.Instantiate(rightObject, parentTransForm);
        GameObject msgObj=chatRight.transform.GetChild(1).gameObject;// ����
        Text text=msgObj.GetComponent<Text>();
        text.text = chatEntity.msg;
        
    }
    void Start()
    {
        Debug.Log("����������, global.SetChatPanelActive ����Ϊtrue");
        datas.Add("Hi����һ�μ��棬������");
        datas.Add("����Ҫ��ʲô");
       
    }

    public void OnChatValueChange()
    {
       
    }
    public void OnChatCloseClick()
    {
        // ����������򿪣��͹ر���
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
        Debug.Log("�������ر��� global.SetChatPanelActive ����Ϊfalse");
        
    }
    private void OnDestroy()
    {
       
    }
    
}
