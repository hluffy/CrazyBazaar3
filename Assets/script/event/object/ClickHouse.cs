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

        // �жϺ������ľ��룬����̫Զ�Ļ��� ��ʾ���
        if (Vector3.Distance(transform.position, player.transform.position) > 8)
        {
            
           
            ShowHeadTips(true, "���������ڼ���");
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
                Debug.Log(" ��ʱ MusicFinished ");
                MusicFinished();
            }, music.clip.length);
        }
    }
    private void MusicFinished()
    {
        // ��������������������
        if (houseEntity.HouseState == HouseState.AtHome)
        {
            ShowChat();
            return;
        }

        ShowHeadTips(true, "�������������ڼ�");
     // Invoke("DelayedMethod", 2f);
        
        TimerUtils.DelayInvoke("DelayedMethod", () =>
        {
            Debug.Log(" ��ʱ DelayedMethod ");
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

        GameObject msgObj = tipPanel.transform.GetChild(0).gameObject;// ����
        Text textTip = msgObj.GetComponent<Text>();
        if (textTip == null) return;
        textTip.text = tip;

    }

}
