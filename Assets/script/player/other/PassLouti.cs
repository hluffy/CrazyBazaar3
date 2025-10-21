using Assets.script.player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.script.nav
{

    public class PassLouti : IOldPassable
    {
        private float timerOffMeshLink;
        private float timeOffMeshLink = 1.2f;

        public void Move(OffMeshLinkData data, Transform transform, Action onFinsh)
        {
            // 播放爬楼梯的动画
            transform.GetComponent<Animator>().SetBool("isPalouti", true);


            // 开始移动  用1.5s的时间，让timerOffMeshLink从0-》1
            timerOffMeshLink += Time.deltaTime / timeOffMeshLink;
            // 修改位置
            transform.position = Vector3.Lerp(data.startPos, data.endPos, timerOffMeshLink);
            // 修改方向

            Vector3 direction = data.endPos - data.startPos;
            direction.y = 0;
            transform.forward = Vector3.MoveTowards(transform.forward, direction, 30 * Time.deltaTime);


            if (timerOffMeshLink >= 1)
            {
                // 重置数据
                onFinsh?.Invoke();
                timerOffMeshLink = 0;
                transform.GetComponent<Animator>().SetBool("isPalouti", false);
                 
            }

        }

       
    }
}