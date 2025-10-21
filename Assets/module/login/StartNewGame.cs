using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartNewGame : MonoBehaviour
{
    

    public void startNewGame()
    {
        Debug.Log("startNewGame startNewGame");

        GameInit pp = gameObject.GetComponent<GameInit>();
      
        pp.StartNewGame();

        // 关闭当前ui界面
        gameObject.SetActive(false);

        // 关闭splash界面
        GameObject bg_splash = GameObject.FindWithTag("bg_splash");
        bg_splash.SetActive(false);
    }

    public void OnDelete()
    {
        Debug.Log("删除按钮");
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
