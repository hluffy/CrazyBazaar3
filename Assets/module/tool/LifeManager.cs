using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLife
{
    public int id;
    public int age;
    public string name;
    public int type;
    public System.Action action;
    //public float time;
    public float delay_time;

    public float timer = 0;
}
// ����������
public class LifeManager : MonoBehaviour
{
    #region ��̬�ֶ�
    #endregion


    #region ��̬�ֶ�
    // npc ������
    private void createNPC()
    {

    }
    // ��ȡ���յ�npc�¼���
    private void createNPCEvent()
    {
        // ũ�£�ս�£�
    }
    // ��ȡ���յ�npc��Ϊ��
    private void createNPCDaily()
    {
        //  ��ˣ�Ŀ�ĵأ����У���ң��ˣ�����������ؼң�
        //  �ֵأ�Ŀ�ĵأ����в�԰���¸��¼����������ۣ�
        //  ���ˣ�Ŀ�ĵأ����У�Я������

        //  �ˣ����Լ��֣�Ұ��ɣ�͵��

    }
    #region ��̬����

    // player��������10��20,60
    private void createPlayer()
    {

    }
    #endregion
    // ���������
    private void createAnimal()
    {

    }

    // ֲ�������
    private void createPlant()
    {

    }
    #endregion
}