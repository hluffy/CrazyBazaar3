using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public Sprite sprite;
    public string animName;

    public Weapon(Sprite _sprite, string _animName)
    {
        sprite = _sprite;
        animName = _animName;
    }
}
