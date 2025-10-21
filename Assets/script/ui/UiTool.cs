using System.Collections;
using UnityEngine;

namespace Assets.script.game.ui
{
    public class UiTool : MonoBehaviour
    {

      
        public static void ResetUI(string name,bool active)
        {
            GameObject UI = GameObject.Find("Canvas");
            GameObject chatPanel = UI.transform.Find(name).gameObject;
            chatPanel.SetActive(active);
        }
       /* public static void GotCHild(GameObject gameObject, bool active)
        {
            GameObject msgObj = gameObject.transform.GetChild(1).gameObject;// 文字
            Text text = msgObj.GetComponent<Text>();
            text.text = chatEntity.msg;
        }*/
    }
}