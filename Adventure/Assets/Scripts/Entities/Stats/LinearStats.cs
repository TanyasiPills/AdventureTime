using UnityEngine;

[CreateAssetMenu(fileName = "LinearStats", menuName = "Base Stats/LinearStats")]
public class LinearStats : BaseStats
{
    public int baseHp;
    public int baseArmor;
    public int baseAttackDamge;
    public int baseArmorPen;

    public int hpScaling;
    public int armorSclaing;
    public int attackDamageScaling;
    public int armorPenScaling;
    public override int Armor(int level)
    {
        throw new System.NotImplementedException();
    }

    public override int ArmorPen(int level)
    {
        throw new System.NotImplementedException();
    }

    public override int AttackDamge(int level)
    {
        throw new System.NotImplementedException();
    }

    public override int Hp(int level)
    {
        throw new System.NotImplementedException();
    }
}
