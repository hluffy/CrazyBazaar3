using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class GlobalUtil : MonoBehaviour
public class GlobalUtil 
{
    public static GlobalUtil global;
 
    public static GlobalUtil getInstance()
    {
        if (global == null)
        {
            global = new GlobalUtil();
        }
      
        return global;
    }
 
     
}
