using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    // 修改器
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (var modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    // 添加修改器
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    // 移除修改器
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
