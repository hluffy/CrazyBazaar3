using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MsgType
{
    msg, // ��ͨ��Ϣ
    address,// ��ַ
    product, // ��Ʒ

}
public class ChatEntity 
{
    public int id; // �������ݿ�id
    public int msgId; // ����Ϣid
    public string msg;
    public int fromId;
    public int toId;
    public MsgType msgType;// ��Ϣ����  


}
public class UserEntity
{
    public int id;
    public string name;
    public string desc;
   
}

