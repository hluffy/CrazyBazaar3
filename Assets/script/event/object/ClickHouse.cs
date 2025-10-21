using UnityEngine;
using UnityEngine.UI;

public class ClickHouse 
{
    private HouseEntity houseEntity;
    public HouseEntity InitHouse()
    {
        houseEntity = new HouseEntity();
        houseEntity.Id = 1;
        houseEntity.HouseState = HouseState.AtHome;
        return houseEntity;
    }
    public void OnHouseClick(Transform player, Transform transform )
    {
        
        player.transform.LookAt(transform.position);

        // 判断和树精的距离，距离太远的话， 提示玩家
        if (Vector3.Distance(transform.position, player.transform.position) > 8)
        {
            
           
            ShowHeadTips(true, "树精先生在家吗");
           // Invoke("DelayedMethod", 2f);
            return;
        }
        houseEntity.HouseState = (Random.Range(0, 2) == 0) ? HouseState.AtHome : HouseState.OutHome;

        int aa = Random.Range(0, 2);
       

        ShowHeadTips(false, null);

        AudioSource music = transform.GetComponent <AudioSource> ();
        if (music != null)
        {
            if (music.isPlaying)
            {
                music.Stop();
                TimerUtils.CancelInvoke("MusicFinished");
                TimerUtils.CancelInvoke("DelayedMethod");
                TimerUtils.Update();
            }
            music.Play();
            
           // Invoke("MusicFinished", music.clip.length);
            TimerUtils.DelayInvoke("MusicFinished", () =>
            {
                Debug.Log(" 延时 MusicFinished ");
                MusicFinished();
            }, music.clip.length);
        }
    }
    private void MusicFinished()
    {
        // 树精先生敲门声音结束
        if (houseEntity.HouseState == HouseState.AtHome)
        {
            ShowChat();
            return;
        }

        ShowHeadTips(true, "树精先生好像不在家");
     // Invoke("DelayedMethod", 2f);
        
        TimerUtils.DelayInvoke("DelayedMethod", () =>
        {
            Debug.Log(" 延时 DelayedMethod ");
            DelayedMethod();
        },2f);
    }
    private void DelayedMethod()
    {
        
        ShowHeadTips(false, null);
    }


    public void ShowChat()
    {
        User.userState = UserState.Chat;
        GameObject UI = GameObject.Find("Canvas");
        GameObject chatPanel = UI.transform.Find("Chat").gameObject;
        chatPanel.SetActive(true);
    }   
    public void ShowHeadTips(bool show, string tip)
    {
        GameObject UI = GameObject.Find("Canvas");
        GameObject tipPanel = UI.transform.Find("Tips").gameObject;
        tipPanel.SetActive(show);
        if (!show) return;

        GameObject msgObj = tipPanel.transform.GetChild(0).gameObject;// 文字
        Text textTip = msgObj.GetComponent<Text>();
        if (textTip == null) return;
        textTip.text = tip;

    }

}
