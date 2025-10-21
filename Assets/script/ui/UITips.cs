using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UITips : MonoBehaviour
{

    private void Update()
    {
       // TimerUtils.Update();
    }

    public GameObject tipPanel;
    public void ShowHeadTips(bool show, string tip)
    {
        try
        {
            tipPanel.SetActive(show);
            if (!show) return;

            GameObject msgObj = tipPanel.transform.GetChild(0).gameObject;
            Text textTip = msgObj.GetComponent<Text>();
            if (textTip == null) return;
            textTip.text = tip;

            StopDelay();
            StartCoroutine(DelayDo());
        }
        catch (Exception e)
        {
            print(e);
        }
      
    }

    IEnumerator DelayDo()
    {
        yield return new WaitForSeconds(3f);
        DelayedMethod();
    }

    private void DelayedMethod()
    {
        Debug.Log("DelayedMethod  DelayedMethod  DelayedMethod");
        ShowHeadTips(false, null);
    }

    public void StopDelay()
    {
     
        StopCoroutine(DelayDo());
    }
}
