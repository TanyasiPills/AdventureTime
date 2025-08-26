using UnityEngine;

public abstract class BaseStats : ScriptableObject
{
    public abstract int Hp(int level);
    public abstract int Armor(int level);
    public abstract int AttackDamge(int level);
    public abstract int ArmorPen(int level);
}
