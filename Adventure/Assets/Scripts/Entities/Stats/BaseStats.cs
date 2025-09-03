using System;
using UnityEngine;

[Serializable]
public abstract class BaseStats
{
    public abstract float Hp(int level);
    public abstract float Armor(int level);
    public abstract float AttackDamge(int level);
    public abstract float ArmorPen(int level);
}
