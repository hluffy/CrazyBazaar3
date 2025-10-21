using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public System.Threading.Timer tmr;


    // camera ����
    public void Start()
    {
        Debug.Log("GameInit GameInit GameInit");
        // ��������
    }

    public void ContinueGame()
    {

        Debug.Log("������Ϸ ��ʼ");
        //���س���
    }
    public void StartNewGame()
    {
        Debug.Log("��������Ϸ ��ʼ");
        Debug.Log("������Ϸ ��ʼ");


        StartCoroutine(ReadyRes());

        // ������Ƶ
        // ����ģ��

        // ������Դ

        // ��ѹ��Դ

        // У����Դ

        // �ͷ���Դ

    }

    public void Update()
    {

        //TimerUtils.Update();
    }
    IEnumerator ReadyRes()
    {

        Debug.Log("������Ϸ ReadyRes");
        // ׼����ͼ,ֲ��߲˵�
       // GameObject map = GameObject.FindWithTag("planemap");
        

        GameObject planeMap = GameObject.FindWithTag("floor");
       
        // ���׼��

        // ����NPC

        // ��������

        // ����ֲ��

        // ���ɶ���

        // ��ȡ���ݿ�


        TimerUtils.DelayInvoke("test", () =>
        {
            Debug.Log(" ��ʱ ReadyRes");
        }, 5f);
        yield return null;

        Debug.Log("������Ϸ ReadyRes ����");
        // ����ʱ��
    }


    private void OnDisable()
    {

    }
    private void OnDestroy()
    {

    }
}
