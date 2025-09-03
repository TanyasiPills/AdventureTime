using System;
using UnityEngine;

[Serializable]
public class LinearStats : BaseStats
{
    public float baseHp;
    public float baseArmor;
    public float baseAttackDamge;
    public float baseArmorPen;

    public float hpScaling;
    public float armorScaling;
    public float attackDamageScaling;
    public float armorPenScaling;
    public override float Armor(int level)
    {
        return baseArmor + (level - 1) * armorScaling;
    }

    public override float ArmorPen(int level)
    {
        return baseArmorPen + (level - 1) * armorPenScaling;
    }

    public override float AttackDamge(int level)
    {
        return baseAttackDamge + (level - 1) * attackDamageScaling;
    }

    public override float Hp(int level)
    {
        return baseHp + (level - 1) * hpScaling;
    }
}
