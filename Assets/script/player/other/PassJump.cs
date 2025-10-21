using Assets.script.player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.script.nav
{
    public class PassJump : IOldPassable
    {

        private float timerOffMeshLink;
        private float timeOffMeshLinkJump = 0.7f;
        private AnimationCurve curve;

        public PassJump()
        {
            // 初始化跳跃的曲线
            curve = new AnimationCurve();
            curve.AddKey(0,0);
            curve.AddKey(0.5f, 1f);
            curve.AddKey(1, 0);
        }

        public void Move(OffMeshLinkData data, Transform transform, Action onFinsh)
        {
            //throw new NotImplementedException();

            // 播放爬楼梯的动画
            transform.GetComponent<Animator>().SetBool("isJump", true);

            // 开始移动  用1.5s的时间，让timerOffMeshLink从0-》1
            timerOffMeshLink += Time.deltaTime / timeOffMeshLinkJump;

            transform.GetComponent<Animator>().SetFloat("jumpTimer", timerOffMeshLink);
            // 跳跃的时候  需要求差值
            Vector3 left = Vector3.Cross(Vector3.up, data.endPos - data.startPos);
            Vector3 up = Vector3.Cross(data.endPos - data.startPos, left);
            Vector3 offSetY = up.normalized * curve.Evaluate(timerOffMeshLink) * 2;

            // 修改位置
            transform.position = Vector3.
                Lerp(data.startPos, data.endPos, timerOffMeshLink) + offSetY;
            // 修改方向

            Vector3 direction = data.endPos - data.startPos;
            direction.y = 0;
            transform.forward = Vector3.MoveTowards(transform.forward, direction, 30 * Time.deltaTime);


            if (timerOffMeshLink >= 1)
            {

                onFinsh?.Invoke(); 
                // 重置数据
                //agent.CompleteOffMeshLink();
                timerOffMeshLink = 0;
                transform.GetComponent<Animator>().SetBool("isJump", false);
               // isMoveToStartPos = false;
            }

        }
 
    }
}