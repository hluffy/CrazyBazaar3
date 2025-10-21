using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.script.player
{
    public interface IOldPassable 
    {

        void Move(OffMeshLinkData data, Transform transform, Action onFinsh);
    }
}