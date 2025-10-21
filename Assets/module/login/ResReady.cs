using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ResReady : MonoBehaviour
{
 

    public Slider slider;

    public ResReady()
    {
        Debug.Log("≥ı ºªØ ResReady");
    }

    private Timer tmr;
    private void Start()
    {
        Debug.Log("Start ResReady");
        slider = transform.GetComponent<Slider>();

        
        int temp = 0;
        tmr = new  Timer(new TimerCallback((obj) =>
        {
            temp += 2;

            if (temp > 100)
            {
                tmr.Dispose();
                return;
            }
            aaa = temp;
            Debug.Log(" —≠ª∑ ReadyRes" + temp);
           

        }), null, 0, 100);
    }
    private int aaa = 0;

    private void Update()
    {
        Debug.Log(" Update ReadyRes" + aaa);

        slider.value = aaa;
    }

 
    private void OnDisable()
    {
        tmr.Dispose();

    }
    private void OnDestroy()
    {
      
    }
}
