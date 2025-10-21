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

        // �رյ�ǰui����
        gameObject.SetActive(false);

        // �ر�splash����
        GameObject bg_splash = GameObject.FindWithTag("bg_splash");
        bg_splash.SetActive(false);
    }

    public void OnDelete()
    {
        Debug.Log("ɾ����ť");
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
