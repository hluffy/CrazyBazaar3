using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DAnimationTriggers : MonoBehaviour
{
    private Player3D player;

    void Start()
    {
        player = Player3DManager.instance.player;
    }

    private void AttackTrigger()
    {
        player.stateMachine.currentState.AnimationFinishTrigger();
    }
}
