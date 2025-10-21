using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
 

public class LoginReady : MonoBehaviour
{
    public Button btn_startNewGame;
    public Button btn_continueGame;
    public Slider slider;

    public GameObject startNewGameUI;



    void Start()
    {
       // slider = transform.GetComponent<Slider>();
       // btn_startNewGame = transform.GetComponent<Button>();
       // btn_continueGame = transform.GetComponent<Button>();


       
    }

   

    // Update is called once per frame
    void Update()
    {
        if(slider.value != aaa)
        {
            slider.value = aaa;
        }
      
    }


  
    public void ContinueGameClick()
    {
        Debug.Log("继续游戏");
        // 加载历史资源
        slider.gameObject.SetActive(true);
        initSlider();
    }
    private Timer tmr;
    private int aaa = 0;
    private void initSlider()
    {
        int temp = 0;
          
        tmr = new Timer(new TimerCallback((obj) =>
        {
            temp += 2;

            if (temp > 100)
            {
                tmr.Dispose();
                return;
            }
            aaa = temp;
          

        }), null, 0, 100);
    }

    public void OnSliderValueChange()
    {
        Debug.Log("slide 数值变化：" + slider.value);
        if (slider.value < 100) return;

        // 到达100后，销毁登录界面，进入游戏
        Debug.Log("slide 数值变化 到达100：");
        gameObject.SetActive(false);
        //Destroy(gameObject);

    }

    public void StartNewGameClick()
    {
        Debug.Log("开始新游戏");
        // 开始新游戏，进入任务选择界面
        startNewGameUI.SetActive(true);
    }

    private void OnDisable()
    {
        Debug.Log("LoginReady OnDisable");
        if (tmr!= null){
            tmr.Dispose();
        }
        
    }
    private void OnDestroy()
    {
        Debug.Log("LoginReady OnDestroy");
    }
}
