using System.Collections;
using UnityEngine;


// 当前用户的通用角色信息
public enum UserState
{
    Idil,
    Wsad, // 前后左右移动
    Jump,
    Running,
    Navi, // 右键移动
    Chat,

}

public static class User
{
    public static int uId;
    public static string name;
    public static string desc_;

    public static int blood;
    public static int hungry;
    public static UserState userState;



}