using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MsgType
{
    msg, // 普通消息
    address,// 地址
    product, // 商品

}
public class ChatEntity 
{
    public int id; // 本地数据库id
    public int msgId; // 云消息id
    public string msg;
    public int fromId;
    public int toId;
    public MsgType msgType;// 消息类型  


}
public class UserEntity
{
    public int id;
    public string name;
    public string desc;
   
}

