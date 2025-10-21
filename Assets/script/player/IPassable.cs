using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IPassable
{

 
    void Move(Vector3 dirTarget,Transform transform,Action onFinsh);

}
