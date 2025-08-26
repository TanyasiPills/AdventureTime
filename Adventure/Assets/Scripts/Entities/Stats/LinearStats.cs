using UnityEngine;

[CreateAssetMenu(fileName = "LinearStats", menuName = "Base Stats/LinearStats")]
public class LinearStats : BaseStats
{
    public int baseHp;
    public int baseArmor;
    public int baseAttackDamge;
    public int baseArmorPen;

    public int hpScaling;
    public int armorScaling;
    public int attackDamageScaling;
    public int armorPenScaling;
    public override int Armor(int level)
    {
        return baseArmor + (level - 1) * armorScaling;
    }

    public override int ArmorPen(int level)
    {
        return baseArmorPen + (level - 1) * armorPenScaling;
    }

    public override int AttackDamge(int level)
    {
        return baseAttackDamge + (level - 1) * attackDamageScaling;
    }

    public override int Hp(int level)
    {
        return baseHp + (level - 1) * hpScaling;
    }
}
