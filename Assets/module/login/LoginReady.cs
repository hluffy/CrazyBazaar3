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
        Debug.Log("������Ϸ");
        // ������ʷ��Դ
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
        Debug.Log("slide ��ֵ�仯��" + slider.value);
        if (slider.value < 100) return;

        // ����100�����ٵ�¼���棬������Ϸ
        Debug.Log("slide ��ֵ�仯 ����100��");
        gameObject.SetActive(false);
        //Destroy(gameObject);

    }

    public void StartNewGameClick()
    {
        Debug.Log("��ʼ����Ϸ");
        // ��ʼ����Ϸ����������ѡ�����
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
