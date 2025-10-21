using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    // 攻击动画触发
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Stats>() != null && hit.transform != player.transform)
            {
                Stats stats = hit.GetComponent<Stats>();
                player.stats.DoDamage(stats);
            }
        }
        AnimationTrigger();
    }

    // 拾取动画触发
    private void PickupTrigger()
    {
        GameObject obj = player.target;

        if (obj != null && obj.GetComponent<ItemObject>() != null)
        {
            obj.GetComponent<ItemObject>().PickupItem(); // 地上捡起来
            player.target = null;
        }

        AnimationTrigger();
    }
}
